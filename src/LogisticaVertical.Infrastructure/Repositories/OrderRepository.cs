using EFCore.BulkExtensions;
using LogisticaVertical.Domain.Dtos;
using LogisticaVertical.Domain.Entities;
using LogisticaVertical.Domain.Interfaces;
using LogisticaVertical.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LogisticaVertical.Infrastructure.Repositories
{
    public sealed class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context) => _context = context;

        public async Task BulkInsertAsync(IEnumerable<User> users)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    await _context.BulkInsertAsync(users.ToList(), options => options.IncludeGraph = true);
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        }

        public async Task<IEnumerable<OrderResponse>> GetOrdersAsync(
            int? orderId, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Orders
                .AsNoTracking()
                .Include(o => o.Products)
                .AsQueryable();

            #region Filtros
            if (orderId.HasValue)
                query = query.Where(o => o.Id == orderId);

            if (startDate.HasValue)
                query = query.Where(o => o.Date >= startDate);

            if (endDate.HasValue)
                query = query.Where(o => o.Date <= endDate);
            #endregion

            return await query
                .Select(o => new OrderResponse(
                    o.UserId,
                     _context.Users.Where(x => x.Id == o.UserId)
                                 .Select(x => x.Name)
                                 .FirstOrDefault(), // Correção: evitando acesso síncrono ao banco
                    new List<OrderDto> // Correção: Usando uma lista em vez de array
                    {
                    new OrderDto(
                        o.Id,
                        o.Total.ToString("0.00"),
                        o.Date.ToString("yyyy-MM-dd"),
                        o.Products.Select(p => new ProductDto(p.IdProduct, p.Value.ToString("0.00"))).ToList() // Correção: Materializando a lista
                    )})).ToListAsync();
        }
    }

}
