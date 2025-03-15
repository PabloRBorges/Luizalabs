using System.Text;

namespace Logistica.Tests
{
    public class CreateFileTestsFixture 
    {

        [CollectionDefinition(nameof(ProductBaseCollection))]
        public class ProductBaseCollection : ICollectionFixture<CreateFileTestsFixture> { }

        public string GenerateFile()
        {
            var fileContent = new StringBuilder()
                .AppendLine("0000000001                              Palmer Prosacco00000007530000000003     1836.7420210308")
                .AppendLine("0000000075                                  Bobbie Batz00000007980000000002     1578.5720211116")
                .AppendLine("0000000049                               Ken Wintheiser00000005230000000003      586.7420210903")
                .AppendLine("0000000014                                 Clelia Hills00000001460000000001      673.4920211125")
                .AppendLine("0000000057                          Elidia Gulgowski IV00000006200000000000     1417.2520210919")
                .AppendLine("0000000080                                 Tabitha Kuhn00000008770000000003      817.1320210612")
                .AppendLine("0000000023                                  Logan Lynch00000002530000000002      322.1220210523")
                .ToString();
            return fileContent.ToString();
        }

        public string GenerateFileError()
        {
            var fileContent = new StringBuilder()
                    .AppendLine("0000000001                              Palmer Prosacco00000007530000000003     1836.7420210308") // Linha válida
                    .AppendLine("InvalidLine") // Linha inválida (causará Exception)
                    .AppendLine("0000000057                          Elidia Gulgowski IV00000006200000000000     1417.2520210919") // Linha válida
                    .ToString();
            return fileContent.ToString();
        }
    }
}
