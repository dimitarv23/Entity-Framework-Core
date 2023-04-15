namespace Boardgames.Data
{
    using Boardgames.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class BoardgamesContext : DbContext
    {
        public BoardgamesContext()
        {
        }

        public BoardgamesContext(DbContextOptions options)
            : base(options)
        {
        }

        public virtual DbSet<Boardgame> Boardgames { get; set; }
        public virtual DbSet<Seller> Sellers { get; set; }
        public virtual DbSet<Creator> Creators { get; set; }
        public virtual DbSet<BoardgameSeller> BoardgamesSellers { get; set; }

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
            modelBuilder.Entity<BoardgameSeller>()
                .HasKey(pk => new { pk.BoardgameId, pk.SellerId });

            modelBuilder.Entity<BoardgameSeller>()
                .HasOne(x => x.Boardgame)
                .WithMany(x => x.BoardgamesSellers)
                .HasForeignKey(x => x.BoardgameId);

            modelBuilder.Entity<BoardgameSeller>()
                .HasOne(x => x.Seller)
                .WithMany(x => x.BoardgamesSellers)
                .HasForeignKey(x => x.SellerId);
        }
    }
}
