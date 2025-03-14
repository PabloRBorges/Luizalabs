﻿using Luizalabs.Infrastructure.Parsers;
using System.Text;

public class HighPerformanceFileParserTests
{
    private readonly HighPerformanceFileParser _fileParser;

    public HighPerformanceFileParserTests()
    {
        _fileParser = new HighPerformanceFileParser();
    }

    [Fact(DisplayName = "Parse with valid file")]
    [Trait("Categoria", "FileParser")]
    public void Parse_ValidFile_ReturnsCorrectUsersAndOrders()
    {
        // Arr
        var fileContent = new StringBuilder()
            .AppendLine("0000000070                              Palmer Prosacco00000007530000000003     1836.7420210308")
            .AppendLine("0000000075                                  Bobbie Batz00000007980000000002     1578.5720211116")
            .AppendLine("0000000049                               Ken Wintheiser00000005230000000003      586.7420210903")
            .AppendLine("0000000014                                 Clelia Hills00000001460000000001      673.4920211125")
            .AppendLine("0000000057                          Elidia Gulgowski IV00000006200000000000     1417.2520210919")
            .AppendLine("0000000080                                 Tabitha Kuhn00000008770000000003      817.1320210612")
            .AppendLine("0000000023                                  Logan Lynch00000002530000000002      322.1220210523")
            .ToString();

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent));

        // Act
        var users = _fileParser.Parse(stream);

        // Assert
        Assert.Equal(2, users.Count);

        var user1 = users.First(u => u.Id == 1);
        Assert.Equal("Palmer Prosacco", user1.Name);
        Assert.Single(user1.Orders);
        Assert.Equal(512.24m, user1.Orders.First().Total);

        var user2 = users.First(u => u.Id == 2);
        Assert.Equal("Bobbie Batz", user2.Name);
        Assert.Single(user2.Orders);
        Assert.Equal(256.24m, user2.Orders.First().Total);
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
    public void Parse_InvalidLine_ThrowsException()
    {
        // Arr
        var invalidLine = "InvalidLine";
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(invalidLine));

        // Act & Assert
        Assert.Throws<FormatException>(() => _fileParser.Parse(stream));
    }
}