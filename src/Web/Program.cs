using System.Text;
using System.IO.Compression;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Serilog;
using LMS.Infrastructure.Data;
using LMS.Infrastructure.Data.Configurations;
using LMS.Web.Middleware;
using LMS.Application;
using LMS.Infrastructure;
using LMS.Web;

var builder = WebApplication.CreateBuilder(args);

var mvcBuilder = builder.Services.AddControllers();

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables(); // Enable env var overrides

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigins", builder =>
    {
        builder
            .WithOrigins(
                "https://localhost:7223",
                "http://localhost:3000",
                "http://localhost:5161",
                "https://tsdev.strahlenstudios.com",
                "https://ts.techlign.com")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

builder.Host.UseSerilog((hostingContext, services, loggerConfiguration) =>
{
    var config = hostingContext.Configuration;

    loggerConfiguration
        .ReadFrom.Configuration(config) 
        .Enrich.FromLogContext()
        .Enrich.WithProperty("Application", "LMS");
});

// JWT Authentication Configuration
var jwtSection = builder.Configuration.GetSection("Jwt");
string secretKey = jwtSection["SecretKey"] ?? "";
if (string.IsNullOrEmpty(secretKey))
{
    throw new Exception("JWT SecretKey is missing in the configuration.");
}

var key = Encoding.ASCII.GetBytes(secretKey);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// Add services to the container.
builder.Services.AddKeyVaultIfConfigured(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebServices();
builder.Services.Configure<AppConfig>(builder.Configuration.GetSection("AppConfig"));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<AppConfig>>().Value);
builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "LMS API", 
        Version = "v1",
        Description = "Library Management System API",
        Contact = new OpenApiContact
        {
            Name = "API Support",
            Email = "support@example.com"
        }
    });
    
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
    });
    
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer",
                },
                Scheme = "Bearer",
                Name = "Bearer",
                In = ParameterLocation.Header,
            }, new List<string>()
        },
    });

    // Include XML comments if available
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

builder.Services.AddControllers();

// Add API versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("X-Version"),
        new QueryStringApiVersionReader("version"));
    options.ReportApiVersions = true;
});

// mvcBuilder.AddVersionedApiExplorer(options =>
// {
//     options.GroupNameFormat = "'v'VVV";
//     options.SubstituteApiVersionInUrl = true;
// });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add response compression
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
    {
        "application/json",
        "application/javascript",
        "text/css",
        "text/html",
        "text/plain",
        "text/xml"
    });
});

builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest;
});

builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest;
});

// Add memory monitoring health check
builder.Services.AddHealthChecks()
    .AddCheck("Memory", () => 
    {
        var memory = GC.GetTotalMemory(false);
        var memoryMB = memory / (1024 * 1024);
        
        return memoryMB > 500 ? 
            HealthCheckResult.Degraded($"Memory usage: {memoryMB:N0} MB") :
            HealthCheckResult.Healthy($"Memory usage: {memoryMB:N0} MB");
    });

var app = builder.Build();

// Configure the HTTP request pipeline in correct order

// 1. Exception handling (should be first)
app.UseMiddleware<ErrorHandlingMiddleware>();

// 2. Request logging (track all requests)
app.UseMiddleware<RequestLoggingMiddleware>();

// 3. Rate limiting
app.UseMiddleware<RateLimitingMiddleware>();

// 4. HTTPS redirection
app.UseHttpsRedirection();

// 4. Static files
app.UseStaticFiles();

// 5. CORS (should be before authentication)
app.UseCors("AllowOrigins");

// 6. Response compression
app.UseResponseCompression();

// 7. Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// 8. Health checks (before routing)
app.UseHealthChecks("/health");

// 9. Swagger/OpenAPI (development only)
if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // Production HSTS
    app.UseHsts();
}

// 10. Routing and endpoints
app.Map("/", () => Results.Redirect("/swagger/index.html"));
app.MapControllers();

app.Run();

public partial class Program { }
