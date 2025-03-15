using LogisticaVertical.Infrastructure.Parsers;
using System.Text;
using static Logistica.Tests.CreateFileTestsFixture;

namespace Logistica.Tests
{
    [Collection("ProductBaseCollection")]
    public class HighPerformanceFileParserTests
    {
        private readonly HighPerformanceFileParser _fileParser;
        private readonly CreateFileTestsFixture _fixture;

        public HighPerformanceFileParserTests(CreateFileTestsFixture createFileTestsFixture)
        {
            _fileParser = new HighPerformanceFileParser();
            _fixture = createFileTestsFixture;
        }

        [Fact(DisplayName = "Parse with valid file")]
        [Trait("Categoria", "FileParser")]
        public void Parse_ValidFile_ReturnsCorrectUsersAndOrders()
        {
            // Arr
            var fileContent = _fixture.GenerateFile();

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent));

            // Act
            var users = _fileParser.Parse(stream);

            // Assert
            Assert.Equal(7, users.Count);

            var user1 = users.First(u => u.Id == 57);
            Assert.Equal(57, user1.Id);
            Assert.Equal("Elidia Gulgowski IV", user1.Name);
            Assert.Single(user1.Orders);
            Assert.Equal(0, user1.Orders.First().Products.First().IdProduct); // productId deve ser 0
            Assert.Equal(1417.25m, user1.Orders.First().Total);
            Assert.Equal(new DateTime(2021, 09, 19), user1.Orders.First().Date);

            var user2 = users.First(u => u.Id == 1);
            Assert.Equal(1, user2.Id);
            Assert.Equal("Palmer Prosacco", user2.Name);
            Assert.Single(user2.Orders);
            Assert.Equal(3, user2.Orders.First().Products.First().IdProduct); // productId deve ser 0
            Assert.Equal(1836.74m, user2.Orders.First().Total);
            Assert.Equal(new DateTime(2021, 03, 08), user2.Orders.First().Date);
        }

        [Fact(DisplayName = "Parse emptyfile")]
        [Trait("Categoria", "FileParser")]
        public void Parse_EmptyFile_ReturnsEmptyList()
        {
            // Arr
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(string.Empty));

            // Act
            var users = _fileParser.Parse(stream);

            // Assert
            Assert.Empty(users);
        }

        [Fact(DisplayName = "Parse invalid line with exception")]
        [Trait("Categoria", "FileParser")]
        public void Parse_InvalidLine_ThrowsFormatExceptionAndContinuesProcessing()
        {
            // Arrange
            var fileContent = _fixture.GenerateFileError();
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent));

            // Act
            var users = _fileParser.Parse(stream);

            // Assert
            // Verifica se o método continuou o processamento após a exceção
            Assert.Equal(2, users.Count); // Dois usuários devem ser processados

            var user1 = users.First(u => u.Id == 57);
            Assert.Equal(57, user1.Id);
            Assert.Equal("Elidia Gulgowski IV", user1.Name);
            Assert.Single(user1.Orders);
            Assert.Equal(0, user1.Orders.First().Products.First().IdProduct); // productId deve ser 0
            Assert.Equal(1417.25m, user1.Orders.First().Total);
            Assert.Equal(new DateTime(2021, 09, 19), user1.Orders.First().Date);

            var user2 = users.First(u => u.Id == 1);
            Assert.Equal(1, user2.Id);
            Assert.Equal("Palmer Prosacco", user2.Name);
            Assert.Single(user2.Orders);
            Assert.Equal(3, user2.Orders.First().Products.First().IdProduct); // productId deve ser 0
            Assert.Equal(1836.74m, user2.Orders.First().Total);
            Assert.Equal(new DateTime(2021, 03, 08), user2.Orders.First().Date);
        }
    }
}