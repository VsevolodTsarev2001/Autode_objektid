using Microsoft.EntityFrameworkCore;
using Autode_objektid.Models;

namespace Autode_objektid.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        public DbSet<Car> Cars => Set<Car>();
        public DbSet<TripLog> TripLogs => Set<TripLog>();
        public DbSet<ContactInfo> ContactInfos => Set<ContactInfo>();
        public DbSet<Address> Addresses => Set<Address>();
        public DbSet<Driver> Drivers => Set<Driver>();
        public DbSet<Garage> Garages => Set<Garage>();
        public DbSet<MaintenanceRecord> MaintenanceRecords => Set<MaintenanceRecord>();
        public DbSet<FuelLog> FuelLogs => Set<FuelLog>();
        public DbSet<Assignment> Assignments => Set<Assignment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Driver>()
                .HasOne(d => d.ContactInfo)
                .WithMany()
                .HasForeignKey(d => d.ContactInfoId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Garage>()
                .HasOne(g => g.Address)
                .WithMany()
                .HasForeignKey(g => g.AddressId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<MaintenanceRecord>()
                .HasOne(m => m.Car)
                .WithMany()
                .HasForeignKey(m => m.CarId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FuelLog>()
                .HasOne(f => f.Car)
                .WithMany()
                .HasForeignKey(f => f.CarId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Assignment>()
                .HasOne(a => a.Car)
                .WithMany()
                .HasForeignKey(a => a.CarId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Assignment>()
                .HasOne(a => a.Driver)
                .WithMany()
                .HasForeignKey(a => a.DriverId)
                .OnDelete(DeleteBehavior.Cascade);

            // индексы для быстрых выборок
            modelBuilder.Entity<FuelLog>().HasIndex(f => new { f.CarId, f.Date });
            modelBuilder.Entity<MaintenanceRecord>().HasIndex(m => new { m.CarId, m.Date });
            modelBuilder.Entity<Assignment>().HasIndex(a => new { a.CarId, a.DriverId });
        }
    }
}
