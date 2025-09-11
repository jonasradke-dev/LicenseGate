using LicenseGate.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LicenseGate.Infrastructure.Persistence;

public class LicenseDbContext : DbContext
{
    public LicenseDbContext(DbContextOptions<LicenseDbContext> options) : base(options)
    {
        
    }

    public DbSet<License> Licenses { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<License>(entity =>
        {
            entity.ToTable("licenses");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .UseIdentityColumn();
            entity.Property(e => e.LicenseKey)
                .HasColumnName("license_key")
                .HasMaxLength(64)
                .IsRequired();

            entity.Property(e => e.DeviceFingerprint)
                .HasColumnName("device_fingerprint")
                .HasMaxLength(128);
            
            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasMaxLength(20)
                .HasConversion<string>()
                .IsRequired();

            entity.Property(e => e.ActivatedAt)
                .HasColumnName("activated_at")
                .HasColumnType("timestamp with time zone");
            
            entity.Property(e => e.ExpiresAt)
                .HasColumnName("expires_at")
                .HasColumnType("timestamp with time zone");
            
            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp with time zone")
                .IsRequired();
            
            entity.Property(e => e.LastHeartbeat)
                .HasColumnName("last_heartbeat")
                .HasColumnType("timestamp with time zone");
                
            entity.Property(e => e.Duration)
                .HasColumnName("duration")
                .HasColumnType("bigint")
                .HasConversion(
                    v => v.HasValue ? v.Value.Ticks : (long?)null,
                    v => v.HasValue ? new TimeSpan(v.Value) : null);
            
            entity.HasIndex(e => e.LicenseKey)
                .IsUnique()
                .HasDatabaseName("idx_license_key");
            
            entity.HasIndex(e => e.DeviceFingerprint)
                .HasDatabaseName("idx_device_fingerprint");
            
            entity.HasIndex(e => e.Status)
                .HasDatabaseName("idx_status");
            
            
            
            


        });
        base.OnModelCreating(modelBuilder);
    }

}