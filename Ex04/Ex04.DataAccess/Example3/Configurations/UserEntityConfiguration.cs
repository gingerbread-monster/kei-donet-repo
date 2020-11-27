using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ex04.DataAccess.Example3.Entities;

namespace Ex04.DataAccess.Example3.Configurations
{
    class UserEntityConfiguration :
        IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd();

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(320);

            builder.HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
