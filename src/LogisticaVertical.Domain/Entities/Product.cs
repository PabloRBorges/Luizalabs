namespace LogisticaVertical.Domain.Entities
{
    //public class Product
    //{
    //    public int Id { get; init; }
    //    public decimal Value { get; init; }
    //    public int OrderId { get; init; }
    //}

    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int IdProduct { get; set; }
        public decimal Value { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; } // Propriedade de navegação
    }
}
