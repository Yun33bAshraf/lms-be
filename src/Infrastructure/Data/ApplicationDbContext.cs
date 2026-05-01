using System.Reflection;
using LMS.Application.Common.Interfaces;
using LMS.Domain.Entities;
using LMS.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ThemeEnum = LMS.Domain.Enums.Theme;
using LanguageCode = LMS.Domain.Enums.LanguageCode;

namespace LMS.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
    // Core User Entities
    public DbSet<User> User => Set<User>();
    public DbSet<UserProfile> UserProfile => Set<UserProfile>();
    public DbSet<UserPreference> UserPreference => Set<UserPreference>();
    public DbSet<LoginAttempt> LoginAttempts => Set<LoginAttempt>();
    public DbSet<ActionHistory> ActionHistory => Set<ActionHistory>();
    
    // Library Management Entities
    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Publisher> Publishers => Set<Publisher>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<BookCopy> BookCopies => Set<BookCopy>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<Loan> Loans => Set<Loan>();
    public DbSet<Reservation> Reservations => Set<Reservation>();
    public DbSet<Fine> Fines => Set<Fine>();
    public DbSet<LibrarySettings> LibrarySettings => Set<LibrarySettings>();
    
    // Existing Entities
    public DbSet<Category> Category => Set<Category>();
    public DbSet<EntityType> EntityType => Set<EntityType>();
    public DbSet<FileStore> FileStore => Set<FileStore>();
    public DbSet<Logs> Logs => Set<Logs>();
    public DbSet<RefreshToken> RefreshToken => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Exclude unnecessary tables by mapping them to null
        builder.Entity<IdentityRole<int>>().ToTable((string?)null); // Role table
        builder.Entity<IdentityUserRole<int>>().ToTable((string?)null); // UserRole table
        builder.Entity<IdentityUserClaim<int>>().ToTable((string?)null); // UserClaim table
        builder.Entity<IdentityUserToken<int>>().ToTable((string?)null); // UserToken table
        builder.Entity<IdentityRoleClaim<int>>().ToTable((string?)null); // RoleClaim table
        builder.Entity<IdentityUserLogin<int>>().ToTable((string?)null); // UserLogin table (if unused)

        builder.Entity<User>(entity =>
        {
            // User → UserProfile (1–1)
            entity.HasOne(u => u.Profile)
                .WithOne(p => p.User)
                .HasForeignKey<UserProfile>(p => p.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            // User → UserPreference (1–1)
            entity.HasOne(u => u.Preference)
                .WithOne(p => p.User)
                .HasForeignKey<UserPreference>(p => p.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            // Indexes
            entity.HasIndex(u => u.UserType);
            entity.HasIndex(u => u.IsActive);
        });

        builder.Entity<UserProfile>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.HasIndex(p => p.UserId)
                .IsUnique();

            entity.Property(p => p.FirstName)
                .HasMaxLength(100);

            entity.Property(p => p.LastName)
                .HasMaxLength(100);

            entity.Property(p => p.DisplayName)
                .HasMaxLength(150);
        });

        builder.Entity<UserPreference>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.HasIndex(p => p.UserId)
                .IsUnique();

            entity.Property(p => p.LanguageId)
                .IsRequired()
                .HasDefaultValue((int)LanguageCode.EN);

            entity.Property(p => p.ThemeId)
                .IsRequired()
                .HasDefaultValue((int)ThemeEnum.Light);
        });


        // Tenant Configuration
        builder.Entity<Tenant>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Subdomain).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Subdomain).IsUnique();
            entity.Property(e => e.LogoUrl).HasMaxLength(200);
        });

        // Book Configuration
        builder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.ISBN).IsRequired().HasMaxLength(20);
            entity.HasIndex(e => e.ISBN).IsUnique();
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Language).HasMaxLength(100);
            entity.Property(e => e.Format).HasMaxLength(50);

            // Relationships
            entity.HasOne(e => e.Publisher)
                .WithMany(p => p.Books)
                .HasForeignKey(e => e.PublisherId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Tenant)
                .WithMany(t => t.Books)
                .HasForeignKey(e => e.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            // Many-to-many relationships will be configured with junction tables
        });

        // Author Configuration
        builder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Biography).HasMaxLength(1000);
            entity.Property(e => e.Nationality).HasMaxLength(100);

            entity.HasOne(e => e.Tenant)
                .WithMany(t => t.Authors)
                .HasForeignKey(e => e.TenantId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Publisher Configuration
        builder.Entity<Publisher>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.PostalCode).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.Website).HasMaxLength(500);

            entity.HasOne(e => e.Tenant)
                .WithMany(t => t.Publishers)
                .HasForeignKey(e => e.TenantId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Genre Configuration
        builder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.ColorCode).HasMaxLength(50);

            // Self-referencing for hierarchical genres
            entity.HasOne(e => e.ParentGenre)
                .WithMany(e => e.SubGenres)
                .HasForeignKey(e => e.ParentGenreId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Tenant)
                .WithMany(t => t.Genres)
                .HasForeignKey(e => e.TenantId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // BookCopy Configuration
        builder.Entity<BookCopy>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Barcode).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.Barcode).IsUnique();
            entity.Property(e => e.Location).HasMaxLength(100);

            entity.HasOne(e => e.Book)
                .WithMany(b => b.Copies)
                .HasForeignKey(e => e.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Tenant)
                .WithMany(t => t.BookCopies)
                .HasForeignKey(e => e.TenantId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Member Configuration
        builder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.MembershipNumber).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.MembershipNumber).IsUnique();
            entity.Property(e => e.Notes).HasMaxLength(500);

            entity.HasOne(e => e.User)
                .WithOne(u => u.MemberProfile)
                .HasForeignKey<Member>(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Tenant)
                .WithMany(t => t.Members)
                .HasForeignKey(e => e.TenantId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Loan Configuration
        builder.Entity<Loan>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Notes).HasMaxLength(500);

            entity.HasOne(e => e.BookCopy)
                .WithMany(bc => bc.Loans)
                .HasForeignKey(e => e.BookCopyId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Member)
                .WithMany(m => m.Loans)
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Cascade);

            // User relationships removed to avoid shadow property conflicts
            // User data can be accessed through the foreign key IDs

            entity.HasOne(e => e.Tenant)
                .WithMany(t => t.Loans)
                .HasForeignKey(e => e.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes for performance
            entity.HasIndex(e => new { e.BookCopyId, e.Status });
            entity.HasIndex(e => new { e.MemberId, e.Status });
            entity.HasIndex(e => e.DueDate);
        });

        // Reservation Configuration
        builder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Notes).HasMaxLength(500);

            entity.HasOne(e => e.Book)
                .WithMany(b => b.Reservations)
                .HasForeignKey(e => e.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Member)
                .WithMany(m => m.Reservations)
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.FulfilledByBookCopy)
                .WithMany()
                .HasForeignKey(e => e.FulfilledByBookCopyId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.Tenant)
                .WithMany(t => t.Reservations)
                .HasForeignKey(e => e.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes for performance
            entity.HasIndex(e => new { e.BookId, e.Status });
            entity.HasIndex(e => new { e.MemberId, e.Status });
            entity.HasIndex(e => e.Priority);
        });

        // Fine Configuration
        builder.Entity<Fine>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.PaymentMethod).HasMaxLength(200);
            entity.Property(e => e.PaymentReference).HasMaxLength(500);

            entity.HasOne(e => e.Loan)
                .WithMany(l => l.Fines)
                .HasForeignKey(e => e.LoanId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Member)
                .WithMany(m => m.Fines)
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Cascade);

            // User relationship removed to avoid shadow property conflicts

            entity.HasOne(e => e.Tenant)
                .WithMany(t => t.Fines)
                .HasForeignKey(e => e.TenantId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // LibrarySettings Configuration
        builder.Entity<LibrarySettings>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.LibraryName).HasMaxLength(200);
            entity.Property(e => e.LibraryAddress).HasMaxLength(500);
            entity.Property(e => e.LibraryPhone).HasMaxLength(100);
            entity.Property(e => e.LibraryEmail).HasMaxLength(200);

            entity.HasOne(e => e.Tenant)
                .WithOne(t => t.LibrarySettings)
                .HasForeignKey<LibrarySettings>(e => e.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            // Ensure one settings record per tenant
            entity.HasIndex(e => e.TenantId).IsUnique();
        });

        // ActionHistory Configuration
        builder.Entity<ActionHistory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.OldValues).HasMaxLength(1000);
            entity.Property(e => e.NewValues).HasMaxLength(1000);

            entity.HasOne(e => e.Tenant)
                .WithMany()
                .HasForeignKey(e => e.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            // User relationship removed to avoid shadow property conflicts

            // Indexes for performance
            entity.HasIndex(e => new { e.TenantId, e.ActionTypeId });
            entity.HasIndex(e => e.PerformedAt);
        });

        // Configure junction tables for many-to-many relationships
        builder.Entity<Book>().HasMany(b => b.Authors).WithMany(a => a.Books)
            .UsingEntity(j => j.ToTable("BookAuthors"));

        builder.Entity<Book>().HasMany(b => b.Genres).WithMany(g => g.Books)
            .UsingEntity(j => j.ToTable("BookGenres"));

        builder.Entity<Logs>().ToTable("Logs", t => t.ExcludeFromMigrations());

        builder.Entity<Logs>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnType("bigint").ValueGeneratedOnAdd();
            entity.Property(e => e.Message).HasColumnType("longtext");
            entity.Property(e => e.Template).HasColumnType("longtext");
            entity.Property(e => e.Level).HasMaxLength(128);
            entity.Property(e => e.TimeStamp)
                .HasColumnName("Timestamp")
                .HasColumnType("varchar(100)")
                .IsRequired();
            entity.Property(e => e.Exception).HasColumnType("longtext");
            entity.Property(e => e.Properties).HasColumnType("longtext");

            entity.HasIndex(e => e.TimeStamp);
            entity.HasIndex(e => e.Level);

            entity.Ignore(e => e.CreatedAt);
            entity.Ignore(e => e.CreatedBy);
            entity.Ignore(e => e.ModifiedAt);
            entity.Ignore(e => e.ModifiedBy);
            entity.Ignore(e => e.DeletedAt);
        });
    }
}
