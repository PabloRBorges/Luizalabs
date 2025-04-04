﻿using LogisticaVertical.Domain.Dtos;
using LogisticaVertical.Domain.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace LogisticaVertical.Application.Service
{
    public sealed class OrderService
    {
        private readonly IFileParser _fileParser;
        private readonly IOrderRepository _orderRepository;

        public OrderService(IFileParser fileParser, IOrderRepository orderRepository)
        {
            _fileParser = fileParser;
            _orderRepository = orderRepository;
        }

        [ExcludeFromCodeCoverage]
        public async Task ProcessFileAsync(Stream fileStream)
        {
            var users = _fileParser.Parse(fileStream);
            await _orderRepository.BulkInsertAsync(users);
        }

        [ExcludeFromCodeCoverage]
        public async Task<IEnumerable<OrderResponse>> GetOrdersAsync(int? orderId, DateTime? startDate, DateTime? endDate)
        {
            return await _orderRepository.GetOrdersAsync(orderId, startDate, endDate);
        }
    }
}
