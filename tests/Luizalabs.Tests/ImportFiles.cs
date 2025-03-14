using Xunit;
using System.IO;
using System.Text;
using Domain.Entities;
using Infrastructure.Parsers;

public class HighPerformanceFileParserTests
{
    private readonly HighPerformanceFileParser _fileParser;

    public HighPerformanceFileParserTests()
    {
        _fileParser = new HighPerformanceFileParser();
    }

    [Fact]
    public void Parse_ValidFile_ReturnsCorrectUsersAndOrders()
    {
        // Arrange
        var fileContent = new StringBuilder()
            .AppendLine("0000000002Medeiros          00000123450000000111 256.2420201201")
            .AppendLine("0000000001Zarelli          00000001230000000111 512.2420211201")
            .ToString();

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent));

        // Act
        var users = _parser.Parse(stream);

        // Assert
        Assert.Equal(2, users.Count);

        var user1 = users.First(u => u.Id == 1);
        Assert.Equal("Zarelli", user1.Name);
        Assert.Single(user1.Orders);
        Assert.Equal(512.24m, user1.Orders.First().Total);

        var user2 = users.First(u => u.Id == 2);
        Assert.Equal("Medeiros", user2.Name);
        Assert.Single(user2.Orders);
        Assert.Equal(256.24m, user2.Orders.First().Total);
    }

    [Fact]
    public void Parse_EmptyFile_ReturnsEmptyList()
    {
        // Arrange
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(string.Empty));

        // Act
        var users = _parser.Parse(stream);

        // Assert
        Assert.Empty(users);
    }

    [Fact]
    public void Parse_InvalidLine_ThrowsException()
    {
        // Arrange
        var invalidLine = "InvalidLine";
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(invalidLine));

        // Act & Assert
        Assert.ThrowsAsync<FormatException>(() => _parser.Parse(stream));
    }
}