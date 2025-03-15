using LogisticaVertical.Domain.Dtos;
using LogisticaVertical.Domain.Entities;
using LogisticaVertical.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Xunit;

namespace SeuProjeto.IntegrationTests
{
    public class OrdersControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly Mock<IFileParser> _mockFileParser;

        public OrdersControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            // Adiciona os mocks
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockFileParser = new Mock<IFileParser>();

            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove os serviços reais
                    var orderRepoDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IOrderRepository));
                    if (orderRepoDescriptor != null)
                        services.Remove(orderRepoDescriptor);

                    var fileParserDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IFileParser));
                    if (fileParserDescriptor != null)
                        services.Remove(fileParserDescriptor);

                    services.AddSingleton(_mockOrderRepository.Object);
                    services.AddSingleton(_mockFileParser.Object);
                });
            });
        }


        [Fact(DisplayName = "Upload valid file")]
        [Trait("Categoria", "FileParser")]
        public async Task Upload_ValidFile_ReturnsAcceptedAndProcessesFile()
        {
            // Arrange
            var client = _factory.CreateClient();
            var fileContent = @"0000000070                                         Palmer Prosacco00000007530000000003       1836.7420210308
0000000075                                            Bobbie Batz00000007980000000002       1578.5720211116";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent));
            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(stream), "file", "orders.txt");

            var parsedUsers = new List<User>(); // Replace with the actual type parsed by IFileParser
            _mockFileParser.Setup(x => x.Parse(It.IsAny<Stream>())).Returns(parsedUsers);

            // Act
            var response = await client.PostAsync("/api/orders/upload", content);

            // Assert
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
            _mockFileParser.Verify(x => x.Parse(It.IsAny<Stream>()), Times.Once);
            _mockOrderRepository.Verify(x => x.BulkInsertAsync(parsedUsers), Times.Once);
        }

        [Fact(DisplayName = "Get without parameters")]
        [Trait("Categoria", "FileParser")]
        public async Task Get_NoParameters_ReturnsOkWithOrdersFromRepository()
        {
            // Arrange
            var client = _factory.CreateClient();
            var expectedOrders = new List<OrderResponse>
            {
                new OrderResponse(70, "Palmer Prosacco", new List<OrderDto>
                {
                    new OrderDto(753, "1836.74", "20210308", new List<ProductDto>())
                }),
                new OrderResponse(75, "Bobbie Batz", new List<OrderDto>
                {
                    new OrderDto(798, "1578.57", "20211116", new List<ProductDto>())
                })
            };

            _mockOrderRepository.Setup(x => x.GetOrdersAsync(null, null, null))
                .ReturnsAsync(expectedOrders);

            // Act
            var response = await client.GetAsync("/api/orders");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

            var actualOrders = await JsonSerializer.DeserializeAsync<IEnumerable<OrderResponse>>(
                await response.Content.ReadAsStreamAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(actualOrders);
            Assert.Collection(actualOrders,
                order =>
                {
                    Assert.Equal(expectedOrders[0].User_Id, order.User_Id);
                    Assert.Equal(expectedOrders[0].Name, order.Name);
                    Assert.Collection(order.Orders,
                        o =>
                        {
                            Assert.Equal(expectedOrders[0].Orders.First().Order_Id, o.Order_Id);
                            Assert.Equal(expectedOrders[0].Orders.First().Total, o.Total);
                            Assert.Equal(expectedOrders[0].Orders.First().Date, o.Date);
                        });
                },
                order =>
                {
                    Assert.Equal(expectedOrders[1].User_Id, order.User_Id);
                    Assert.Equal(expectedOrders[1].Name, order.Name);
                    Assert.Collection(order.Orders,
                        o =>
                        {
                            Assert.Equal(expectedOrders[1].Orders.First().Order_Id, o.Order_Id);
                            Assert.Equal(expectedOrders[1].Orders.First().Total, o.Total);
                            Assert.Equal(expectedOrders[1].Orders.First().Date, o.Date);
                        });
                });

            _mockOrderRepository.Verify(x => x.GetOrdersAsync(null, null, null), Times.Once);
        }

        [Fact(DisplayName = "Get with order id")]
        [Trait("Categoria", "FileParser")]
        public async Task Get_WithOrderId_ReturnsOkWithFilteredOrdersFromRepository()
        {
            // Arrange
            var client = _factory.CreateClient();
            var orderId = 70;
            var expectedOrders = new List<OrderResponse>
            {
                new OrderResponse(70, "Palmer Prosacco", new List<OrderDto>
                {
                    new OrderDto(753, "1836.74", "20210308", new List<ProductDto>())
                })
            };

            _mockOrderRepository.Setup(x => x.GetOrdersAsync(orderId, null, null)).ReturnsAsync(expectedOrders);

            // Act
            var response = await client.GetAsync($"/api/orders?orderId={orderId}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var actualOrders = await JsonSerializer.DeserializeAsync<IEnumerable<OrderResponse>>(
                await response.Content.ReadAsStreamAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(actualOrders);
            Assert.Single(actualOrders);
            Assert.Equal(expectedOrders.First().User_Id, actualOrders.First().User_Id);

            _mockOrderRepository.Verify(x => x.GetOrdersAsync(orderId, null, null), Times.Once);
        }

        [Fact(DisplayName = "Get with date Range")]
        [Trait("Categoria", "FileParser")]
        public async Task Get_WithDateRange_ReturnsOkWithFilteredOrdersFromRepository()
        {
            // Arrange
            var client = _factory.CreateClient();
            var startDate = new DateTime(2021, 03, 01);
            var endDate = new DateTime(2021, 03, 31);
            var expectedOrders = new List<OrderResponse>
            {
                new OrderResponse(70, "Palmer Prosacco", new List<OrderDto>
                {
                    new OrderDto(753, "1836.74", "20210308", new List<ProductDto>())
                })
            };

            _mockOrderRepository.Setup(x => x.GetOrdersAsync(null, startDate, endDate))
                .ReturnsAsync(expectedOrders);

            // Act
            var response = await client.GetAsync($"/api/orders?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var actualOrders = await JsonSerializer.DeserializeAsync<IEnumerable<OrderResponse>>(
                await response.Content.ReadAsStreamAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(actualOrders);
            Assert.Single(actualOrders);
            Assert.Equal(expectedOrders.First().User_Id, actualOrders.First().User_Id);

            _mockOrderRepository.Verify(x => x.GetOrdersAsync(null, startDate, endDate), Times.Once);
        }

        [Fact(DisplayName = "Upload valid file")]
        [Trait("Categoria", "FileParser")]
        public async Task UploadFile_ShouldReturnAccepted()
        {
            // Arrange
            var client = _factory.CreateClient();
            var fileContent = new StringBuilder()
                .AppendLine("0000000001                              Palmer Prosacco00000007530000000003     1836.7420210308")
                .AppendLine("0000000075                                  Bobbie Batz00000007980000000002     1578.5720211116")
                .AppendLine("0000000049                               Ken Wintheiser00000005230000000003      586.7420210903")
                .AppendLine("0000000014                                 Clelia Hills00000001460000000001      673.4920211125")
                .AppendLine("0000000057                          Elidia Gulgowski IV00000006200000000000     1417.2520210919")
                .AppendLine("0000000080                                 Tabitha Kuhn00000008770000000003      817.1320210612")
                .AppendLine("0000000023                                  Logan Lynch00000002530000000002      322.1220210523")
                .ToString();

            var content = new MultipartFormDataContent();
            var fileBytes = Encoding.UTF8.GetBytes(fileContent);
            var fileStreamContent = new ByteArrayContent(fileBytes);
            fileStreamContent.Headers.ContentType = MediaTypeHeaderValue.Parse("text/plain");
            content.Add(fileStreamContent, "file", "orders.txt");

            // Act
            var response = await client.PostAsync("api/orders/upload", content);

            // Assert
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
        }
    }
}




