using Microsoft.EntityFrameworkCore;
using NoteMVCTestNeuro.Models;

namespace NoteMVCTestNeuro.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Note> Notes => Set<Note>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Note>(e =>
            {
                e.Property(x => x.Title).HasMaxLength(200).IsRequired();
                e.Property(x => x.Content).HasMaxLength(4000);
            });
        }
    }
}