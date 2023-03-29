using Demo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Demo.Configurations
{
    internal class CarConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> modelBuilder)
        {
            //modelBuilder
            //  .ToTable("Cars");

            modelBuilder
                .HasKey(x => x.CarId);

            modelBuilder
                .Property(x => x.Brand)
                .HasColumnType("nvarchar(50)");

            modelBuilder
                .Property(x => x.Model)
                .HasColumnType("nvarchar(50)");

            modelBuilder
                .Ignore(x => x.RegistrationPlate);
        }
    }
}
