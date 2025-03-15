
using LogisticaVertical.Domain.Dtos;
using LogisticaVertical.Domain.Entities;

namespace LogisticaVertical.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task BulkInsertAsync(IEnumerable<User> users);
        Task<IEnumerable<OrderResponse>> GetOrdersAsync(int? orderId, DateTime? startDate, DateTime? endDate);
    }
}
