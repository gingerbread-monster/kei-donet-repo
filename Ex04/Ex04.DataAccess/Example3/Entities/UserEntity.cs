using System.Collections.Generic;

namespace Ex04.DataAccess.Example3.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public UserPaymentInfoEntity PaymentInfo { get; set; }

        public ICollection<RouteSubscriberEntity> RouteSubscriptions { get; set; }
    }
}
