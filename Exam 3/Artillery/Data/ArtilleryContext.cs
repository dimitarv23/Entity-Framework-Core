namespace Artillery.Data
{
    using Artillery.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Reflection.Emit;

    public class ArtilleryContext : DbContext
    {
        public ArtilleryContext() 
        { 
        }

        public ArtilleryContext(DbContextOptions options)
            : base(options) 
        { 
        }

        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Manufacturer> Manufacturers { get; set; }
        public virtual DbSet<Shell> Shells { get; set; }
        public virtual DbSet<Gun> Guns { get; set; }
        public virtual DbSet<CountryGun> CountriesGuns { get; set; }

         protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CountryGun>()
                .HasKey(pk => new { pk.CountryId, pk.GunId });

            modelBuilder.Entity<CountryGun>()
                .HasOne(x => x.Country)
                .WithMany(c => c.CountriesGuns)
                .HasForeignKey(x => x.CountryId);

            modelBuilder.Entity<CountryGun>()
                .HasOne(x => x.Gun)
                .WithMany(g => g.CountriesGuns)
                .HasForeignKey(x => x.GunId);
        }
    }
}
