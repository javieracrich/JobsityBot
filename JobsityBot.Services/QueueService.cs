using JobsityBot.Core;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;

namespace JobsityBot.Services;

public class QueueService : IQueueService
{
    private readonly QueueOptions options;

    public QueueService(IOptions<QueueOptions> options)
    {
        this.options = options.Value;
    }

    public void PublishToQueue(string message)
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

        var body = Encoding.UTF8.GetBytes(message!);

        channel.BasicPublish(exchange: "",
                             routingKey: options.QueueName,
                             basicProperties: null,
                             body: body);
    }

}

public interface IQueueService
{
    void PublishToQueue(string message);
}