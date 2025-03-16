using LogisticaVertical.Domain.Entities;
using LogisticaVertical.Domain.Interfaces;
using LogisticaVertical.Infrastructure.Data;
using LogisticaVertical.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;


namespace Logistitca.Tests
{
    public class OrderRepositoryTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly OrderRepository _repository;
        private readonly Mock<IOrderRepository> _repositoryMock;

        public OrderRepositoryTests()
        {
            _repositoryMock = new Mock<IOrderRepository>(); //mock

            // Configurando o DbContext para usar o provider InMemory
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new AppDbContext(options);
            _repository = new OrderRepository(_context);
        }


        [Fact(DisplayName = "Test bulk insert mock")]
        [Trait("Categoria", "Upload")]
        public async Task BulkInsertAsync_ShouldInsertUsers()
        {
            // Arrange: cria a lista de usuários a serem inseridos
            var users = new List<User>
            {
                new User { Id = 1, Name = "Usuário 1" },
                new User { Id = 2, Name = "Usuário 2" }
            };

            // Configura o mock para simular o comportamento do método BulkInsertAsync
            _repositoryMock
                .Setup(r => r.BulkInsertAsync(It.IsAny<IEnumerable<User>>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act: chama o método no mock
            await _repositoryMock.Object.BulkInsertAsync(users);

            // Assert: verifica se o método foi chamado corretamente
            _repositoryMock.Verify(r => r.BulkInsertAsync(It.IsAny<IEnumerable<User>>()), Times.Once);
        }


        [Fact(DisplayName = "Test get ")]
        [Trait("Categoria", "GetOrder")]
        public async Task GetOrdersAsync_ShouldReturnFilteredOrders()
        {
            // Arrange: inserir dados de teste
            // Inserir usuários
            var user1 = new User { Id = 1, Name = "Usuário 1" };
            var user2 = new User { Id = 2, Name = "Usuário 2" };
            _context.Users.AddRange(user1, user2);

            var idProduct1 = Guid.NewGuid();
            var idProduct2 = Guid.NewGuid();

            // Inserir pedidos e produtos
            var order1 = new Order
            {
                Id = 1,
                UserId = 1,
                Date = new DateTime(2022, 01, 01),
                Products = new List<Product>
                {
                    new Product { Id = idProduct1, IdProduct = 101, Value = 10.0m },
                    new Product { Id = idProduct2, IdProduct = 102, Value = 20.0m }
                }
            };

            var idProduct3 = Guid.NewGuid();

            var order2 = new Order
            {
                Id = 2,
                UserId = 2,
                Date = new DateTime(2022, 02, 01),
                Products = new List<Product>
                {
                    new Product { Id = idProduct3, IdProduct = 103, Value = 30.0m }
                }
            };

            _context.Orders.AddRange(order1, order2);
            await _context.SaveChangesAsync();

            // Act: aplicar filtro por orderId
            var resultadoPorId = await _repository.GetOrdersAsync(1, null, null);
            Assert.Single(resultadoPorId);
            Assert.Equal(1, resultadoPorId.First().User_Id);

            // Act: aplicar filtro por data
            var resultadoPorData = await _repository.GetOrdersAsync(null, new DateTime(2022, 01, 15), new DateTime(2022, 02, 15));
            Assert.Single(resultadoPorData);
            Assert.Equal(2, resultadoPorData.First().User_Id);

            // Act: sem filtros
            var resultadoTodos = await _repository.GetOrdersAsync(null, null, null);
            Assert.Equal(2, resultadoTodos.Count());
        }

        public void Dispose() => _context.Dispose();
    }
}
