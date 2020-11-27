using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ex04.DataAccess.Example2.Entities
{
    public class ProductEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<OrderEntity> Orders { get; set; }
    }
}
