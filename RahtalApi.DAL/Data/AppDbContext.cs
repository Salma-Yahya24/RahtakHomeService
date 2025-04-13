using Microsoft.EntityFrameworkCore;
using RahtakApi.Entities.Models;

namespace RahtakApi.DAL.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Users> Users { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<BookingDetails> BookingDetails { get; set; }
    public DbSet<BookingStatus> BookingStatuses { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    public DbSet<Payments> Payments { get; set; }
    public DbSet<Reviews> Reviews { get; set; }
    public DbSet<ServiceGroups> ServiceGroups { get; set; }
    public DbSet<ServiceProviders> ServiceProviders { get; set; }
    public DbSet<ServiceProviderType> ServiceProviderTypes { get; set; }
    public DbSet<SubService> SubServices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // العلاقة بين ServiceProviders و ServiceProviderType
        modelBuilder.Entity<ServiceProviders>()
            .HasOne(sp => sp.ServiceProviderType)
            .WithMany()
            .HasForeignKey(sp => sp.ServiceProviderTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        // العلاقة بين BookingDetails و الكيانات الأخرى
        modelBuilder.Entity<BookingDetails>()
            .HasOne(bd => bd.SubService)
            .WithMany()
            .HasForeignKey(bd => bd.SubServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BookingDetails>()
            .HasOne(bd => bd.Booking)
            .WithMany(b => b.BookingDetails)
            .HasForeignKey(bd => bd.BookingId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BookingDetails>()
            .HasOne(bd => bd.ServiceProvider)
            .WithMany()
            .HasForeignKey(bd => bd.ServiceProviderId)
            .OnDelete(DeleteBehavior.Restrict);

        // العلاقة بين SubService و ServiceProvider
        modelBuilder.Entity<SubService>()
            .HasOne(ss => ss.ServiceProvider)
            .WithMany()
            .HasForeignKey(ss => ss.ServiceProviderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
