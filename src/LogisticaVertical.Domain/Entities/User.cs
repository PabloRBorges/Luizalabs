namespace LogisticaVertical.Domain.Entities
{
    public class User
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public List<Order> Orders { get; } = new();
    }
}
