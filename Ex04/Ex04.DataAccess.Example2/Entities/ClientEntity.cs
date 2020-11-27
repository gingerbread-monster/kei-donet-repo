using System.Collections.Generic;

namespace Ex04.DataAccess.Example2.Entities
{
    public class ClientEntity
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public ICollection<OrderEntity> Orders { get; set; }
    }
}
