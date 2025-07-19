
# Logistica Vertical - Luizalabs

A LogisticaVertical API fornece endpoints para upload e consulta de pedidos, permitindo a interação com dados de pedidos e produtos. Este projeto foi desenvolvido seguindo os princípios do SOLID, garantindo um código limpo, modular e de fácil manutenção.




### Autor

- [@pablorborges](https://github.com/PabloRBorges)

## Como Executar o Projeto LogisticaVertical

## Pré-requisitos
Antes de executar o projeto, certifique-se de ter instalado:
- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads)
- [Postman](https://www.postman.com/) (Opcional, para testar a API)

## Configuração do Banco de Dados
1. No arquivo `appsettings.json`, configure a string de conexão:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=SEU_SERVIDOR;Database=LogisticaVerticalDB;User Id=SEU_USUARIO;Password=SUA_SENHA;"
   }
   ```
2. Execute as migrações para criar o banco de dados:
   ```sh
   dotnet ef database update
   ```

## Executando o Projeto
1. No terminal, navegue até a pasta do projeto e execute:
   ```sh
   dotnet run
   ```
2.  A API será iniciada e poderá ser acessada em:
   - Swagger UI: `https://localhost:5001/swagger`
   - API Base: `https://localhost:5001/api`


## Estrutura Arquitetural do Projeto Logistica Vertical

## Visão Geral
Este projeto segue os princípios do SOLID para garantir um código limpo, modular e de fácil manutenção. Ele está estruturado seguindo uma abordagem de separação de responsabilidades, dividindo as camadas em **API, Application, Domain e Infrastructure**.

## Estrutura do Projeto

#### 1. `LogisticaVertical.API`
- Camada de apresentação (Presentation Layer)
- Contém os **Controllers**, que expõem os endpoints da API
- Usa **Swagger** para documentação automática
- Responsável por tratar as requisições HTTP

#### 2. `LogisticaVertical.Application`
- Camada de aplicação (Application Layer)
- Contém os **serviços** que implementam a lógica de negócios
- Usa **interfaces** para desacoplamento e facilitar a injeção de dependências
- Aplica o princípio da **Inversão de Dependência (D - SOLID)**

#### 3. `LogisticaVertical.Domain`
- Camada de domínio (Domain Layer)
- Define as **entidades** e **interfaces** principais do sistema
- Aplica **Encapsulamento e Regras de Negócio**
- Segue o princípio da **Responsabilidade Única (S - SOLID)** garantindo que cada classe tenha uma única razão para mudar

#### 4. `LogisticaVertical.Infrastructure`
- Camada de infraestrutura (Infrastructure Layer)
- Implementa **repositórios** para acesso ao banco de dados
- Contém a implementação do **Entity Framework Core** para persistência
- Aplica o princípio da **Substituição de Liskov (L - SOLID)** garantindo que classes derivadas podem substituir suas classes base sem alterar o comportamento esperado





## Documentação da API
#### Versão
- **v1**

## Endpoints:

### Upload de Pedidos

```http
  GET /api/orders/upload
```

- **Descrição:** Faz o upload de um arquivo contendo pedidos.
- **Request Body:**
  - `file` (multipart/form-data): Arquivo binário a ser enviado.
- **Respostas:**
  - `200 OK` - Upload realizado com sucesso.


### Retorna todas as orders

```http
  GET /api/orders
```

- **Descrição:** Obtém uma lista de pedidos com filtros opcionais.
- **Parâmetros de Consulta:**

| Parâmetro   | Tipo       | Descrição                           |
| :---------- | :--------- | :---------------------------------- |
| `id` | `int32` | ID do pedido (opcional)                              |
| `startDate` | `(date-time)` | Data inicial do filtro (opcional) |
| `endDate`   | `(date-time)` | Data final do filtro (opcional)|

- **Respostas:**
  - `200 OK` - Retorna uma lista de pedidos no formato JSON.

## Modelos de Dados

### `OrderResponse`
Resposta de um pedido associado a um usuário:
```json
{
  "user_Id": 1,
  "name": "Cliente Exemplo",
  "orders": [
    {
      "order_Id": 123,
      "total": "100.50",
      "date": "2025-03-16T12:00:00Z",
      "products": [
        {
          "product_Id": 456,
          "value": "50.25"
        }
      ]
    }
  ]
}
```

## Como Usar

### Requisitos
- Postman / qualquer cliente HTTP

### Exemplo de Requisição
#### Upload de Arquivo (cURL)
```sh
curl -X POST "http://seu-servidor.com/api/orders/upload" \
     -F "file=@caminho/do/arquivo.csv"
```

#### Consulta de Pedidos (cURL)
```sh
curl -X GET "http://seu-servidor.com/api/orders?startDate=2025-01-01&endDate=2025-03-16"
```


## Rodando os testes

O projeto conta com testes unitários e de integração:

- **Testes Unitários:** Localizados em tests/Logistica.Tests
- **Testes de Integração:** Localizados em tests/Logistica.Integration.Tests
- **Teste de carga:** Localizado em tests/Logistica.Tests

Utilize o comando abaixo para executar os testes:

```bash
  dotnet test
```

