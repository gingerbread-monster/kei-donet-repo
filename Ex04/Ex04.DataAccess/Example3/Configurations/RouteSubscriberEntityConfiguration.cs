using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ex04.DataAccess.Example3.Entities;

namespace Ex04.DataAccess.Example3.Configurations
{
    class RouteSubscriberEntityConfiguration :
        IEntityTypeConfiguration<RouteSubscriberEntity>
    {
        public void Configure(EntityTypeBuilder<RouteSubscriberEntity> builder)
        {
            builder.HasKey(routeSubscriber => routeSubscriber.Id);
            builder.Property(routeSubscriber => routeSubscriber.Id)
                .ValueGeneratedOnAdd();

            builder.HasOne(routeSubscriber => routeSubscriber.User)
                .WithMany(user => user.RouteSubscriptions)
                .HasForeignKey(routeSubscriber => routeSubscriber.UserId);

            builder.HasOne(routeSubscriber => routeSubscriber.Route)
                .WithMany(route => route.RouteSubscribers)
                .HasForeignKey(routeSubscriber => routeSubscriber.RouteId);
        }
    }
}
