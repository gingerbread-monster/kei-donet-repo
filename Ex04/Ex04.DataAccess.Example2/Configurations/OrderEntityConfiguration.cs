using Ex04.DataAccess.Example2.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ex04.DataAccess.Example2.Configurations
{
    class OrderEntityConfiguration : IEntityTypeConfiguration<OrderEntity>
    {
        public void Configure(EntityTypeBuilder<OrderEntity> builder)
        {
            builder.HasOne(order => order.Product)
                .WithMany(product => product.Orders)
                .HasForeignKey(order => order.ProductId);

            builder.HasOne(order => order.Client)
                .WithMany(client => client.Orders)
                .HasForeignKey(order => order.ClientId);
        }
    }
}
