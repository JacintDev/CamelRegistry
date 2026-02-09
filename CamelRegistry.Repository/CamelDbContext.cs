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
                entity.Property(e => e.HumpCount).IsRequired().HasMaxLength(1);
            });

            modelBuilder.Entity<Camel>().HasData(
                new Camel
                {
                    Id = Guid.NewGuid(),
                    Name = "Default Camel",
                    Color = "Brown",
                    HumpCount = 2,
                    LastFed = DateTime.Now
                }
            );
            modelBuilder.Entity<Camel>().HasData(
               new Camel
               {
                   Id = Guid.NewGuid(),
                   Name = "Default Camel2",
                   Color = "Brown",
                   HumpCount = 1,
                   LastFed = new DateTime(2000, 1, 1)
               }
           );
        }

       
    }
}
