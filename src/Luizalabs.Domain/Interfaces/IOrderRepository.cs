using Luizalabs.Application.Dtos;
using Luizalabs.Domain.Entities;

namespace Luizalabs.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task BulkInsertAsync(IEnumerable<User> users);
        Task<IEnumerable<OrderResponse>> GetOrdersAsync(int? orderId, DateTime? startDate, DateTime? endDate);
    }
}
