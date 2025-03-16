
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LogisticaVertical.Domain.Dtos;
using LogisticaVertical.Domain.Interfaces;
using Moq;
using NBomber.Contracts;
using NBomber.CSharp;
using Oracle.ManagedDataAccess.Types;
using Xunit;

public class OrderRepositoryLoadTests
{
    private readonly Mock<IOrderRepository> _orderRepository;

    public OrderRepositoryLoadTests()
    {
        _orderRepository = new Mock<IOrderRepository>();
    }

    [Fact(DisplayName = "Load test 10 seconds")]
    [Trait("Categoria", "GetOrder")]
    public void LoadTest_GetOrdersAsync()
    {
        // arr
        _orderRepository.Setup(repo => repo.GetOrdersAsync(It.IsAny<int?>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>()))
            .ReturnsAsync(new List<OrderResponse>()); // Simulando resposta vazia

        //act
        var scenario = Scenario.Create("GetOrders_LoadTest", async context =>
        {
            await _orderRepository.Object.GetOrdersAsync(null, null, null);
            return Response.Ok();
        })
            .WithWarmUpDuration(TimeSpan.FromSeconds(5))
            .WithLoadSimulations(
                // 10 requisições por segundo durante 10s
                Simulation.Inject(rate: 10, interval: TimeSpan.FromSeconds(1), during: TimeSpan.FromSeconds(10)) 
            );

        //assert
        var result = NBomberRunner.RegisterScenarios(scenario).Run();
        Assert.True(result.AllOkCount > 0);
    }
}
