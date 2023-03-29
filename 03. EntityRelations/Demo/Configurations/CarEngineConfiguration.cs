using Demo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.Configurations
{
    public class CarEngineConfiguration : IEntityTypeConfiguration<CarEngine>
    {
        public void Configure(EntityTypeBuilder<CarEngine> modelBuilder)
        {
            modelBuilder
              .ToTable("CarsEngines");

            modelBuilder
                .HasKey(x => new { x.CarId, x.EngineId });

            modelBuilder
                .HasOne(x => x.Car)
                .WithMany(x => x.CarsEngines)
                .HasForeignKey(x => x.CarId);

            modelBuilder
                .HasOne(x => x.Engine)
                .WithMany(x => x.CarsEngines)
                .HasForeignKey(x => x.EngineId);
        }
    }
}
