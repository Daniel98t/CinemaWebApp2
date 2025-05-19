using CinemaWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaWebApp.Data
{
    public class CinemaContext : DbContext
    {
        public CinemaContext(DbContextOptions<CinemaContext> options) : base(options) { }

        public DbSet<Film> Films { get; set; } = null!;
        public DbSet<Salong> Salonger { get; set; } = null!;
        public DbSet<Föreställning> Föreställningar { get; set; } = null!;
        public DbSet<Bokning> Bokningar { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Anpassa decimal för SQLite
            modelBuilder.Entity<Film>()
                .Property(f => f.Price)
                .HasConversion<double>(); // Konvertera decimal till double för att fungera med SQLite

            // Relation mellan Föreställning och Bokning
            modelBuilder.Entity<Föreställning>()
                .HasMany(f => f.Bokningar)
                .WithOne(b => b.Föreställning)
                .HasForeignKey(b => b.FöreställningId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}