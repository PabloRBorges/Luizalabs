# Luizalabs

Como executar a solução:

Pré-requisitos:

.NET 6 (ou superior) instalado.
Um banco de dados SQL Server disponível.
Configurar a string de conexão no appsettings.json (chave "DefaultConnection").
Execução:

Compile a aplicação com dotnet build.
Execute a aplicação com dotnet run.
Utilize o Swagger UI (geralmente disponível em http://localhost:5000/swagger) para testar os endpoints.
Endpoints:

POST /api/orders/upload: Envie um arquivo via form-data (campo “file”) contendo os dados no formato legado.
GET /api/orders: Consulte os pedidos, passando opcionalmente parâmetros de query: orderId, dateStart e dateEnd (datas no formato que o .NET consiga interpretar, por exemplo, "2021-12-01").
Tecnologias e Padrões:

C# / .NET 6
ASP.NET Core Minimal APIs
Entity Framework Core para acesso e persistência no banco SQL.
Projeto estruturado com simplicidade, seguindo os princípios SOLID.
Testes unitários e automação (build e coverage) podem ser adicionados conforme necessidade.
Esta solução demonstra a lógica de leitura e transformação dos dados, a persistência em banco SQL e a exposição de uma API REST com filtros conforme o desafio técnico proposto.

Caso haja alguma dúvida ou necessidade de ajustes, fico à disposição para esclarecimentos!
