using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luizalabs.Domain.Entities
{
    public class Order
    {
        public int Id { get; init; }
        public decimal Total { get; set; }
        public DateTime Date { get; init; }
        public List<Product> Products { get; } = new();
        public int UserId { get; init; }
    }
}
