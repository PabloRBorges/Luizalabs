namespace LogisticaVertical.Domain.Dtos
{
    public record OrderResponse(
        int User_Id,
        string Name,
        IEnumerable<OrderDto> Orders);

    public record OrderDto(
        int Order_Id,
        string Total,
        string Date,
        IEnumerable<ProductDto> Products);

    public record ProductDto(int Product_Id, string Value);
}
