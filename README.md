# Luizalabs

# LogisticaVertical

## Tecnologias Utilizadas
- ASP.NET Core 6.0
- Entity Framework Core
- SQL Server

## Como Executar
1. Clone o repositório.
2. Configure a connection string no `appsettings.json`.
3. Execute as migrações:
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
Inicie a aplicação:

```bash
dotnet run
```

# Endpoints
POST /api/orders/upload: Envie um arquivo de pedidos.

GET /api/orders: Consulte pedidos com filtros (orderId, startDate, endDate).

# Decisões Arquiteturais
-Separação em camadas (API, Application, Domain, Infrastructure).
-Uso do Entity Framework Core para ORM.
-Processamento de arquivo com agrupamento em memória.

# Notas Finais:

A solução segue os princípios SOLID e prioriza simplicidade.
O código está pronto para ser estendido com autenticação, logging e mais validações.
Os dados são persistidos em SQL Server para consultas eficientes.
