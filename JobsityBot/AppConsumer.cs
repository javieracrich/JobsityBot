using Jobsity;
using JobsityBot.Services;
using MassTransit;

namespace JobsityBot.Api;

public class AppConsumer : IConsumer<AppMessage>
{
    private readonly IStooqService stooqService;
    private readonly IPublishEndpoint publishEndpoint;

    public AppConsumer(IStooqService messageService, IPublishEndpoint publishEndpoint)
    {
        this.stooqService = messageService;
        this.publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<AppMessage> context)
    {
        if (string.IsNullOrWhiteSpace(context.Message.StockCode) ||
            context.Message.StockCode.Length > 20 ||
            context.Message.RoomId <= 0)
        {
            return; // do not send any message to queue.
        }

        var response = await this.stooqService.GetData(context.Message.StockCode);
        await this.publishEndpoint.Publish(new BotMessage(response, context.Message.RoomId), null);
    }
}