using Microsoft.EntityFrameworkCore;
using Ex04.DataAccess.Example2.Entities;
using Ex04.DataAccess.Example2.Configurations;

namespace Ex04.DataAccess.Example2
{
    public class Example2DbContext : DbContext
    {
        public DbSet<ClientEntity> Clients { get; set; }

        public DbSet<ProductEntity> Products { get; set; }

        public DbSet<OrderEntity> Orders { get; set; }

        public Example2DbContext(DbContextOptions options) 
            : base(options)
        {
            //Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderEntityConfiguration());

            modelBuilder.Entity<ClientEntity>()
                .Property(client => client.Email)
                .IsRequired();

            modelBuilder.Entity<ClientEntity>()
                .HasIndex(client => client.Email)
                .IsUnique();
        }
    }
}
