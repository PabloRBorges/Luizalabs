namespace LogisticaVertical.Domain.Entities
{
    //public class Order
    //{
    //    public int Id { get; init; }
    //    public decimal Total { get; set; }
    //    public DateTime Date { get; init; }
    //    public List<Product> Products { get; } = new();
    //    public int UserId { get; set; }
    //}

    public class Order
    {
        public int Id { get; set; }
        public decimal Total { get; set; }
        public DateTime Date { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } // Propriedade de navegação

        public ICollection<Product> Products { get; set; } = new List<Product>(); // Relacionamento 1:N
    }
}
