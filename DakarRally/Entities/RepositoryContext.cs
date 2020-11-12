using Entities.Enums;
using Entities.Extensions;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleStatistic> VehicleStatistics { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }
        public DbSet<Race> Races { get; set; }
        public DbSet<Simulation> Simulations { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VehicleStatistic>().Property(p => p.Status).HasDefaultValue(VehicleStatus.ReadyToStart);

            modelBuilder.SeedVehicleTypes();
            modelBuilder.Entity<Vehicle>().HasQueryFilter(v => !v.IsDeleted);
        }

    }
}
