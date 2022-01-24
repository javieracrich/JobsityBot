using Jobsity;
using JobsityBot.Api;
using JobsityBot.Services;
using MassTransit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace JobsityBot.Tests;

public class AppConsumerTests
{
    [Fact]
    public async Task GetData_NullCode()
    {
        //arrange
        var stooqService = new Mock<IStooqService>();
        var publishEndpoint = new Mock<IPublishEndpoint>();
        var sut = new AppConsumer(stooqService.Object, publishEndpoint.Object);
        var context = new Mock<ConsumeContext<AppMessage>>();
        var message = new AppMessage(null!, 0);
        context.Setup(x => x.Message).Returns(message);

        //act
        await sut.Consume(context.Object);

        //assert
        stooqService.Verify(x => x.GetData(It.IsAny<string>()), Times.Never);
        publishEndpoint.Verify(x => x.Publish(It.IsAny<BotMessage>(), null, It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetData_NotNullCode()
    {
        //arrange
        var stooqService = new Mock<IStooqService>();
        var publishEndpoint = new Mock<IPublishEndpoint>();
        var sut = new AppConsumer(stooqService.Object, publishEndpoint.Object);
        var context = new Mock<ConsumeContext<AppMessage>>();
        var message = new AppMessage("aapl.us", 1);
        context.Setup(x => x.Message).Returns(message);

        //act
        await sut.Consume(context.Object);

        //assert
        stooqService.Verify(x => x.GetData(It.IsAny<string>()), Times.Once);
        publishEndpoint.Verify(x => x.Publish(It.IsAny<BotMessage>(), null, It.IsAny<CancellationToken>()), Times.Once);
    }
}
