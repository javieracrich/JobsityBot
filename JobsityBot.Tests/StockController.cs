using JobsityBot.Controllers;
using JobsityBot.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace JobsityBot.Tests
{
    public class StockControllerTest
    {
        [Fact]
        public async Task GetData_NullCode()
        {
            var stooqService = new Mock<IStooqService>();
            var queueService = new Mock<IQueueService>();

            var sut = new StockController(stooqService.Object, queueService.Object);

            var response = await sut.GetStockQuoteAsync(null!);

            Assert.IsType<OkResult>(response);

            stooqService.Verify(x => x.GetData(It.IsAny<string>()), Times.Never);
            queueService.Verify(x => x.PublishToQueue(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task GetData_NotNullCode()
        {
            var stooqService = new Mock<IStooqService>();
            var queueService = new Mock<IQueueService>();

            var sut = new StockController(stooqService.Object, queueService.Object);

            var response = await sut.GetStockQuoteAsync("aapl.us");

            Assert.IsType<OkResult>(response);

            stooqService.Verify(x => x.GetData(It.IsAny<string>()), Times.Once);
            queueService.Verify(x => x.PublishToQueue(It.IsAny<string>()), Times.Once);
        }
    }
}