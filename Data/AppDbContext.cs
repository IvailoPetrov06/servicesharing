using AutoMapper;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using servicesharing.Models;

namespace servicesharing.Data
{
    public class AppDbContext : IdentityDbContext<Users>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSet за моделите
        public DbSet<Mechanic> Mechanics { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Profile> Profiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Допълнителни конфигурации за вашите модели, ако е необходимо
            modelBuilder.Entity<Mechanic>().ToTable("Mechanics");
            modelBuilder.Entity<Service>().ToTable("Services");
            modelBuilder.Entity<Reservation>().ToTable("Reservations");
            modelBuilder.Entity<Review>().ToTable("Reviews");
            modelBuilder.Entity<Profile>().ToTable("Profiles");
        }
    }
}
