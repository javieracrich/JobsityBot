using JobsityBot.Api;
using JobsityBot.Core;
using JobsityBot.Services;
using MassTransit;

namespace JobsityBot.Extensions;

public static class ServiceCollectionExtensions
{
    const string Queue = "Queue";
    const string Stooq = "Stooq";

    public static void AddServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.Configure<StooqOptions>(config.GetSection(Stooq));
        services.Configure<QueueOptions>(config.GetSection(Queue));
        services.AddScoped<IStooqService, StooqService>();
        services.AddMassTransitMiddleware(config);
    }

    private static void AddMassTransitMiddleware(this IServiceCollection services, IConfiguration config)
    {
        var options = new QueueOptions();
        config.GetSection(Queue).Bind(options);

        services.AddMassTransit(x =>
        {
            x.AddConsumer<AppConsumer>();
            x.UsingRabbitMq((ctx, config) =>
            {
                config.AutoDelete = false;
                config.Exclusive = false;
                config.Durable = true;
                config.AutoStart = true;
                config.Host(options.Url);
                config.ReceiveEndpoint(options.QueueName, c =>
                {
                    c.ConfigureConsumer<AppConsumer>(ctx);
                });
            });
        });

        services.AddMassTransitHostedService();
    }
}
