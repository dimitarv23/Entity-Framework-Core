namespace Footballers.Data
{
    using Footballers.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class FootballersContext : DbContext
    {
        public FootballersContext() { }

        public FootballersContext(DbContextOptions options)
            : base(options) { }

        public virtual DbSet<Footballer> Footballers { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<Coach> Coaches { get; set; }
        public virtual DbSet<TeamFootballer> TeamsFootballers { get; set; }

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
            modelBuilder.Entity<TeamFootballer>()
                .HasKey(pk => new { pk.TeamId, pk.FootballerId });

            modelBuilder.Entity<TeamFootballer>()
                .HasOne(x => x.Team)
                .WithMany(t => t.TeamsFootballers)
                .HasForeignKey(x => x.TeamId);

            modelBuilder.Entity<TeamFootballer>()
                .HasOne(x => x.Footballer)
                .WithMany(f => f.TeamsFootballers)
                .HasForeignKey(x => x.FootballerId);
        }
    }
}
