using AutoMapper;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using servicesharing.Data.Entities;

namespace servicesharing.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        // DbSet за моделите
        public DbSet<Location> Locations { get; set; }
        public DbSet<Mechanic> Mechanics { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Допълнителни конфигурации за вашите модели, ако е необходимо
            modelBuilder.Entity<Mechanic>().ToTable("Mechanics");
            modelBuilder.Entity<Service>().ToTable("Services");
            modelBuilder.Entity<Reservation>().ToTable("Reservations");
            modelBuilder.Entity<Review>().ToTable("Reviews");
        }
    }
}
