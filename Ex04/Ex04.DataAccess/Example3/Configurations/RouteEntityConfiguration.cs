using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ex04.DataAccess.Example3.Entities;


namespace Ex04.DataAccess.Example3.Configurations
{
    class RouteEntityConfiguration :
        IEntityTypeConfiguration<RouteEntity>
    {
        public void Configure(EntityTypeBuilder<RouteEntity> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id).ValueGeneratedOnAdd();

            builder.Property(r => r.DateCreated)
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            builder.Property(r => r.Name)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
