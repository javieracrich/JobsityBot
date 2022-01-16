using JobsityBot.Core;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace JobsityBot.Services;

public interface IQueueService
{
    void PublishToQueue(string message, int roomId);
}

public class QueueService : IQueueService
{
    private readonly QueueOptions options;

    public QueueService(IOptions<QueueOptions> options)
    {
        this.options = options.Value;
    }

    public void PublishToQueue(string message, int roomId)
    {
        var factory = new ConnectionFactory()
        {
            HostName = options.Url,
            UserName = options.UserName,
            Password = options.Password,
        };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(queue: options.QueueName,
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        channel.BasicPublish(exchange: "",
                             routingKey: options.QueueName,
                             basicProperties: null,
                             body: GetMessageBody(message, roomId));
    }

    private static byte[] GetMessageBody(string message, int roomId)
    {
        var msg = new QueueMessage(message, roomId);

        var json = JsonSerializer.Serialize(msg);

        return Encoding.UTF8.GetBytes(json);
    }
}