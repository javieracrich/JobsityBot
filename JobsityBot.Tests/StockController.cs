using Jobsity;
using JobsityBot.Controllers;
using JobsityBot.Services;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace JobsityBot.Tests;

public class StockControllerTest
{
    [Fact]
    public async Task GetData_NullCode()
    {
        var stooqService = new Mock<IStooqService>();
        var publishEndpoint = new Mock<IPublishEndpoint>();

        var sut = new StockController(stooqService.Object, publishEndpoint.Object);

        var response = await sut.GetStockQuoteAsync(null!, 1, new CancellationToken());

        Assert.IsType<OkResult>(response);

        stooqService.Verify(x => x.GetData(It.IsAny<string>()), Times.Never);
        publishEndpoint.Verify(x => x.Publish(It.IsAny<BotMessage>(), null, It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetData_NotNullCode()
    {
        var stooqService = new Mock<IStooqService>();
        var publishEndpoint = new Mock<IPublishEndpoint>();

        var sut = new StockController(stooqService.Object, publishEndpoint.Object);

        var response = await sut.GetStockQuoteAsync("aapl.us", 1, new CancellationToken());

        Assert.IsType<OkResult>(response);

        stooqService.Verify(x => x.GetData(It.IsAny<string>()), Times.Once);
        publishEndpoint.Verify(x => x.Publish(It.IsAny<BotMessage>(), null, It.IsAny<CancellationToken>()), Times.Once);
    }
}
