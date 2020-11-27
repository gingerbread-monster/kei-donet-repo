namespace Ex04.DataAccess.Example3.Entities
{
    public class UserPaymentInfoEntity
    {
        public int UserId { get; set; }
        public UserEntity User { get; set; }

        public string PaymentSystem { get; set; }

        public string BankAccountNumber { get; set; }
    }
}
