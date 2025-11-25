using ChargeHubAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChargeHubAPI.Infrastructure.Persistence;

public class ChargeHubDbContext : DbContext
{
    public ChargeHubDbContext(DbContextOptions<ChargeHubDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var user = modelBuilder.Entity<User>();
        user.HasKey(u => u.UserId);
        user.Property(u => u.UserId).HasMaxLength(10).IsRequired();
        user.Property(u => u.Identecation).HasMaxLength(20).IsRequired();
        user.Property(u => u.Username).HasMaxLength(100).IsRequired();
        user.Property(u => u.Name).HasMaxLength(150).IsRequired();
        user.Property(u => u.PhoneNumber).HasMaxLength(30).IsRequired();
        user.Property(u => u.Email).HasMaxLength(150).IsRequired();
        user.Property(u => u.SignupVerificationCode).HasMaxLength(6);
        user.Property(u => u.PasswordResetCode).HasMaxLength(6);
        user.Property(u => u.PasswordHash).IsRequired();

        user.HasIndex(u => u.Username).IsUnique();
        user.HasIndex(u => u.Identecation).IsUnique();
        user.HasIndex(u => u.PhoneNumber).IsUnique();
        user.HasIndex(u => u.Email).IsUnique();

        user.OwnsOne(u => u.Esp32, esp32 =>
        {
            esp32.Property(e => e.BtName).HasMaxLength(100).HasColumnName("Esp32BtName");
            esp32.Property(e => e.BtAddress).HasMaxLength(100).HasColumnName("Esp32BtAddress");
        });

        user.OwnsOne(u => u.StatusPosition, pos =>
        {
            pos.Property(p => p.North).HasColumnName("StatusNorth").HasColumnType("float");
            pos.Property(p => p.East).HasColumnName("StatusEast").HasColumnType("float");
            pos.Property(p => p.South).HasColumnName("StatusSouth").HasColumnType("float");
            pos.Property(p => p.West).HasColumnName("StatusWest").HasColumnType("float");
        });
    }
}



