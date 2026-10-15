using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace EFCodeFirst.API.Models
{
    public class LibraryContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Publisher> Publishers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "server=localhost;user=root;password=;database=library",
                ServerVersion.AutoDetect("server=localhost;user=root;password=;database=library")
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Publisher>(entity => {
                entity.HasKey(e => e.ID);
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Book>(entity => {
                entity.HasKey(e => e.ISBN);
                entity.Property(e => e.Title).IsRequired();
                entity.HasOne(d => d.Publisher).WithMany(p => p.Books);
            });
        }
    }
}
