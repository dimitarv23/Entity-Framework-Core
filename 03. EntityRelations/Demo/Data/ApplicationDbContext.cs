using Demo.Configurations;
using Demo.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.Data
{
    public class ApplicationDbContext : DbContext
    {
        public virtual DbSet<Car> Cars { get; set; }

        public virtual DbSet<Engine> Engines { get; set; }

        public virtual DbSet<CarEngine> CarsEngines { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    @"Server=DIMITARPC;Database=EF_Demo;Integrated Security=True;Trust Server Certificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<Car>(new CarConfiguration());

            modelBuilder.ApplyConfiguration<Engine>(new EngineConfiguration());

            modelBuilder.ApplyConfiguration<CarEngine>(new CarEngineConfiguration());
        }
    }
}
