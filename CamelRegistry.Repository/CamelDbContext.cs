using CamelRegistry.Entities;
using Microsoft.EntityFrameworkCore;

namespace CamelRegistry.Repository
{
    public class CamelDbContext : DbContext
    {
        public DbSet<Camel> Camels { get; set; }

        public CamelDbContext(DbContextOptions<CamelDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Camel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Color).HasMaxLength(50);
                entity.Property(e => e.HumbCount).IsRequired().HasMaxLength(1);
            });
        }
    }
}
