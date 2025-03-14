using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luizalabs.Application.Dtos
{
    public record OrderResponse(
        int UserId,
        string Name,
        IEnumerable<OrderDto> Orders);

    public record OrderDto(
        int OrderId,
        string Total,
        string Date,
        IEnumerable<ProductDto> Products);

    public record ProductDto(int ProductId, string Value);
}
