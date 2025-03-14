using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luizalabs.Domain.Entities
{
    public class Product
    {
        public int Id { get; init; }
        public decimal Value { get; init; }
        public int OrderId { get; init; }
    }
}
