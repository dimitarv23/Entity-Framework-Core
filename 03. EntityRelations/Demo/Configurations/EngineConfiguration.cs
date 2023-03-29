using Demo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.Configurations
{
    public class EngineConfiguration : IEntityTypeConfiguration<Engine>
    {
        public void Configure(EntityTypeBuilder<Engine> modelBuilder)
        {
            //modelBuilder
            //  .ToTable("Engines");

            modelBuilder
                .Property(x => x.Model)
                .HasColumnType("nvarchar(50)");
        }
    }
}
