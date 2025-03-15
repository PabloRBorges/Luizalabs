using LogisticaVertical.Domain.Entities;
using LogisticaVertical.Domain.Interfaces;
using System.Globalization;
using System.Text;

namespace LogisticaVertical.Infrastructure.Parsers
{
    public class HighPerformanceFileParser : IFileParser
    {
        public IReadOnlyCollection<User> Parse(Stream fileStream)
        {
            var users = new Dictionary<int, User>();
            var orders = new Dictionary<(int UserId, int OrderId), Order>();

            using var reader = new StreamReader(fileStream, Encoding.UTF8, leaveOpen: true);

            while (reader.ReadLine() is { } line)
            {
                try
                {
                    ParseLine(line.AsSpan(), out var userId, out var userName, out var orderId,
                                    out var productId, out var value, out var date);

                    if (!users.TryGetValue(userId, out var user))
                    {
                        user = new User { Id = userId, Name = userName };
                        users.Add(userId, user);
                    }

                    var orderKey = (userId, orderId);
                    if (!orders.TryGetValue(orderKey, out var order))
                    {
                        order = new Order { Id = orderId, Date = date, UserId = userId };
                        orders.Add(orderKey, order);
                        user.Orders.Add(order);
                    }

                    order.Products.Add(new Product { IdProduct = productId, Value = value, OrderId = orderId });
                }
                catch (Exception ex)
                {
                    // Logar o erro e continuar o processamento ou retornar um erro, dependendo da necessidade
                    Console.WriteLine($"Erro ao processar a linha: {line}. Erro: {ex.Message}");
                }
            }

            CalculateOrderTotals(orders.Values);
            return users.Values.ToArray();
        }

        private static void ParseLine(
            ReadOnlySpan<char> line,
            out int userId,
            out string userName,
            out int orderId,
            out int productId,
            out decimal value,
            out DateTime date)
        {

            var userTest = line.Slice(0, 10).TrimStart('0');
            userId = userTest.IsEmpty ? 0 : int.Parse(userTest);

            var userNameSpan = line.Slice(10, 45).Trim();
            userName = userNameSpan.IsEmpty ? string.Empty : userNameSpan.ToString();

            var orderIdSpan = line.Slice(55, 10).TrimStart('0');
            orderId = orderIdSpan.IsEmpty ? 0 : int.Parse(orderIdSpan);

            var productIdSpan = line.Slice(65, 10).TrimStart('0');
            productId = productIdSpan.IsEmpty ? 0 : int.Parse(productIdSpan);

            value = decimal.Parse(line.Slice(75, 12).Trim(), CultureInfo.InvariantCulture);
            date = DateTime.ParseExact(line.Slice(87, 8), "yyyyMMdd", CultureInfo.InvariantCulture);
        }

        private static void CalculateOrderTotals(IEnumerable<Order> orders)
        {
            foreach (var order in orders)
            {
                order.Total = order.Products.Sum(p => p.Value);
            }
        }
    }
}
