using System.Reflection;
using Project.Application.Common.Interfaces;
using Project.Domain.Entities;
using Project.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ThemeEnum = Project.Domain.Enums.Theme;

namespace Project.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<UserProfile> UserProfile => Set<UserProfile>();
    public DbSet<UserPreference> UserPreference => Set<UserPreference>();
    public DbSet<Category> Category => Set<Category>();
    public DbSet<EntityType> EntityType => Set<EntityType>();
    public DbSet<FileStore> FileStore => Set<FileStore>();
    public DbSet<Logs> Logs => Set<Logs>();

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
