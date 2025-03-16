using System.Diagnostics.CodeAnalysis;

namespace LogisticaVertical.Domain.Dtos
{
    [ExcludeFromCodeCoverage]
    public record OrderResponse(
        int User_Id,
        string Name,
        IEnumerable<OrderDto> Orders);

    [ExcludeFromCodeCoverage]
    public record OrderDto(
        int Order_Id,
        string Total,
        string Date,
        IEnumerable<ProductDto> Products);

    [ExcludeFromCodeCoverage]
    public record ProductDto(int Product_Id, string Value);
}
