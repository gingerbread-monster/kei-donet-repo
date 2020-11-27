using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ex04.DataAccess.Example3.Entities;

namespace Ex04.DataAccess.Example3.Configurations
{
    class UserPaymentInfoEntityConfiguration :
        IEntityTypeConfiguration<UserPaymentInfoEntity>
    {
        public void Configure(EntityTypeBuilder<UserPaymentInfoEntity> builder)
        {
            builder.HasKey(paymentInfo => paymentInfo.UserId);

            builder.HasOne(paymentInfo => paymentInfo.User)
                .WithOne(user => user.PaymentInfo)
                .HasForeignKey<UserPaymentInfoEntity>(paymentInfo => paymentInfo.UserId);
        }
    }
}
