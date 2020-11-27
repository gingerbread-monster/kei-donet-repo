using System;

namespace Ex04.DataAccess.Example2.Entities
{
    public class OrderEntity
    {
        public int Id { get; set; }

        public DateTimeOffset OrderDateTime { get; set; }

        public int ClientId { get; set; }
        public ClientEntity Client  { get; set; }

        public int ProductId { get; set; }
        public ProductEntity Product{ get; set; }
    }
}
