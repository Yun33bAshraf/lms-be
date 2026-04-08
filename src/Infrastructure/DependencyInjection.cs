using System.Data.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Project.Application.Common.Interfaces;
using Project.Domain.Entities;
using Project.Domain.Enums;
using Project.Infrastructure.Data;
using Project.Infrastructure.Data.Interceptors;
using Project.Infrastructure.Identity;
using Project.Infrastructure.Repositories;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;

namespace Project.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = GetValidatedConnectionString(configuration);

        // Cache server version to avoid repeated detection
        var serverVersion = ServerVersion.AutoDetect(connectionString);

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        services.AddAuthorizationBuilder();

        // Use DbContext pooling for better memory management
        services.AddDbContextPool<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            
            // Performance optimizations
            options.EnableServiceProviderCaching();
            options.EnableSensitiveDataLogging(false);
            options.EnableDetailedErrors(false);
            
            // Query optimization
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            
            // MySQL configuration
            options.UseMySql(connectionString, serverVersion, mysqlOptions =>
            {
                mysqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
                mysqlOptions.CommandTimeout(30);
                mysqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
            });
            
        }, poolSize: 128);

        // Add Identity services with performance optimizations
        services.AddIdentityCore<User>(options =>
        {
            // Password requirements
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 8; // Increased for better security
            
            // User requirements
            options.User.RequireUniqueEmail = true;
            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            
            // Lockout settings
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;
            
            // Sign-in settings
            options.SignIn.RequireConfirmedEmail = false; // Set to true in production
            options.SignIn.RequireConfirmedPhoneNumber = false;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddSignInManager()
        .AddDefaultTokenProviders();

        // Configure security stamp validation for better performance
        services.Configure<SecurityStampValidatorOptions>(options =>
        {
            options.ValidationInterval = TimeSpan.FromMinutes(30); // Reduced frequency
        });

        // Configure authentication cookies
        services.ConfigureApplicationCookie(options =>
        {
            options.ExpireTimeSpan = TimeSpan.FromDays(7);
            options.SlidingExpiration = true;
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        });

        services.AddScoped<IDbRepository, DbRepository>();
        services.AddTransient(typeof(DbConnection), (IServiceProvider) => InitializeDatabase(connectionString));

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<ApplicationDbContextInitialiser>();

        services.AddAuthentication()
            .AddBearerToken(IdentityConstants.BearerScheme);

        services.AddSingleton(TimeProvider.System);
        services.AddTransient<IIdentityService, IdentityService>();

        // Add distributed caching
        AddCachingServices(services, configuration, connectionString);

        services.AddScoped(typeof(IQueryRepository<>), typeof(QueryRepository<>));

        services.AddScoped(typeof(IDataRepository<>), typeof(DataRepository<>));
        services.AddScoped<IEmailSenderRepository, EmailSenderRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<INotificationService, NotificationService>();

        // Environment-based service configuration
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        
        if (environment.Equals("Development", StringComparison.OrdinalIgnoreCase))
        {
            // Development-only services
            //services.AddScoped<IS3FileService, LocalFileService>();
            //services.AddScoped<IDeveloperService, DeveloperService>();
        }
        else
        {
            // Production services
            //services.AddScoped<IS3FileService, S3FileService>();
        }
        
        services.AddScoped<IFileUploadRepository, FileUploadRepository>();

        // Register fake IUser for seeding
        services.AddScoped<IUser, SeederUser>();

        // Add health checks based on environment
        if (environment.Equals("Production", StringComparison.OrdinalIgnoreCase))
        {
            services.AddHealthChecks()
                .AddMySql(connectionString);
        }

        return services;
    }

    private static void AddCachingServices(IServiceCollection services, IConfiguration configuration, string connectionString)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        
        // Add memory cache as fallback
        services.AddMemoryCache(options =>
        {
            options.SizeLimit = 1000; // Limit cache size
        });

        // Try to add Redis cache in production
        if (environment.Equals("Production", StringComparison.OrdinalIgnoreCase))
        {
            var redisConnectionString = configuration.GetConnectionString("Redis") 
                ?? Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING");
            
            if (!string.IsNullOrEmpty(redisConnectionString))
            {
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = redisConnectionString;
                    options.InstanceName = "Project_";
                });
            }
            else
            {
                // Fallback to distributed memory cache
                services.AddDistributedMemoryCache();
            }
        }
        else
        {
            // Development: Use distributed memory cache
            services.AddDistributedMemoryCache();
        }

        // Add cache service registration
        //services.AddScoped<ICacheService, CacheService>();
    }

    private static string GetValidatedConnectionString(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        if (!string.IsNullOrEmpty(connectionString))
        {
            // Replace environment variables if present
            connectionString = connectionString
                .Replace("{MYSQL_HOST}", Environment.GetEnvironmentVariable("MYSQL_HOST") ?? "localhost")
                .Replace("{MYSQL_USER}", Environment.GetEnvironmentVariable("MYSQL_USER") ?? "root")
                .Replace("{MYSQL_PASSWORD}", Environment.GetEnvironmentVariable("MYSQL_PASSWORD") ?? "");
        }
        
        // Fallback to environment variables if no connection string found
        if (string.IsNullOrEmpty(connectionString))
        {
            var host = Environment.GetEnvironmentVariable("MYSQL_HOST") ?? "localhost";
            var user = Environment.GetEnvironmentVariable("MYSQL_USER") ?? "root";
            var password = Environment.GetEnvironmentVariable("MYSQL_PASSWORD") ?? "";
            var database = Environment.GetEnvironmentVariable("MYSQL_DATABASE") ?? "Project_dev";
            
            connectionString = $"server={host};port=3306;user={user};password={password};database={database};Connection Timeout=60";
        }
        
        Guard.Against.NullOrEmpty(connectionString, message: "Connection string could not be determined from configuration or environment variables.");
        
        return connectionString;
    }

    private static object InitializeDatabase(string cconnectionString)
    {
        // [START mysql_connection]
        var connectionString = new MySqlConnectionStringBuilder(cconnectionString);
        DbConnection connection = new MySqlConnection(connectionString.ConnectionString);
        // [END mysql_connection]
        return connection;
    }

    public class SeederUser : IUser
    {
        public int Id => (int)UserType.Admin;
    }
}
