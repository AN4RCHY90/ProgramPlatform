using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProgramPlatform.Models;

namespace ProgramPlatform.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<UserModel> UserModels { get; set; }
    public DbSet<RoleModel> RoleModels { get; set; }
    public DbSet<DeviceModel> DeviceModels { get; set; }
    public DbSet<SimNetworkModel> SimNetworkModels { get; set; }
    public DbSet<DeviceUpdateSettingsModel> DeviceUpdateSettingsModels { get; set; }
    public DbSet<ZohoTokenModel> OAuthTokens { get; set; }
    public DbSet<AccountModel> AccountModels { get; set; }

    /// <summary>
    /// Configures the model for the given context by defining entities and relationships.
    /// </summary>
    /// <param name="modelBuilder">The builder used to construct the model for the context being created.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Ignore Identity models for custom migration
        modelBuilder.Entity<IdentityUser>().ToTable("AspNetUsers", t => t.ExcludeFromMigrations());
        modelBuilder.Entity<IdentityRole>().ToTable("AspNetRoles", t => t.ExcludeFromMigrations());
        modelBuilder.Entity<IdentityUserRole<string>>().ToTable("AspNetUserRoles", t => t.ExcludeFromMigrations());
        modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("AspNetUserClaims", t => t.ExcludeFromMigrations());
        modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("AspNetUserLogins", t => t.ExcludeFromMigrations());
        modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("AspNetRoleClaims", t => t.ExcludeFromMigrations());
        modelBuilder.Entity<IdentityUserToken<string>>().ToTable("AspNetUserTokens", t => t.ExcludeFromMigrations());
        
        modelBuilder.Entity<UserModel>()
            .HasOne(u => u.Role)
            .WithMany()
            .HasForeignKey(u => u.RoleId);

        modelBuilder.Entity<DeviceModel>()
            .HasOne(d => d.SimCardNetwork)
            .WithMany()
            .HasForeignKey(d => d.SimId);

        modelBuilder.Entity<AccountModel>()
            .HasOne(a => a.Role)
            .WithMany()
            .HasForeignKey(a => a.RoleId);
        modelBuilder.Entity<UserModel>()
            .HasOne(u => u.Account)
            .WithMany()
            .HasForeignKey(u => u.AccountId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}