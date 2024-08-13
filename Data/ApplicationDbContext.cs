using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProgramPlatform.Models;

namespace ProgramPlatform.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUserModel, ApplicationRoleModel, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<ApplicationUserModel> UserModels { get; set; }
    public DbSet<ApplicationRoleModel> RoleModels { get; set; }
    public DbSet<DeviceModel> DeviceModels { get; set; }
    public DbSet<SimNetworkModel> SimNetworkModels { get; set; }
    public DbSet<DeviceUpdateSettingsModel> DeviceUpdateSettingsModels { get; set; }
    public DbSet<ZohoTokenModel> OAuthTokens { get; set; }
    public DbSet<AccountModel> AccountModels { get; set; }

    /// <summary>
    /// Overrides the base OnModelCreating method to configure the database model.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<ApplicationUserModel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("AspNetUsers");
        });

        modelBuilder.Entity<ApplicationRoleModel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("AspNetRoles");
        });

        modelBuilder.Entity<IdentityUserRole<Guid>>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId });
            entity.ToTable("AspNetUserRoles");
        });

        modelBuilder.Entity<IdentityUserClaim<Guid>>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("AspNetUserClaims");
        });

        modelBuilder.Entity<IdentityUserLogin<Guid>>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });
            entity.ToTable("AspNetUserLogins");
        });

        modelBuilder.Entity<IdentityRoleClaim<Guid>>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("AspNetRoleClaims");
        });

        modelBuilder.Entity<IdentityUserToken<Guid>>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            entity.ToTable("AspNetUserTokens");
        });
        
        modelBuilder.Entity<ZohoTokenModel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ExpiresAt).HasColumnType("datetime2");
        });

        modelBuilder.Entity<AccountModel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.SubscriptionExpiry).HasColumnType("datetime2");
        });

        modelBuilder.Entity<DeviceModel>()
            .HasOne(d => d.SimCardNetwork)
            .WithMany()
            .HasForeignKey(d => d.SimId);

        modelBuilder.Entity<AccountModel>()
            .HasOne(a => a.Role)
            .WithMany()
            .HasForeignKey(a => a.RoleId);
        modelBuilder.Entity<ApplicationUserModel>()
            .HasOne(u => u.Account)
            .WithMany()
            .HasForeignKey(u => u.AccountId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}