using JobsityBot.Core;
using JobsityBot.Services;
using MassTransit;

namespace JobsityBot.Extensions;

public static class ServiceCollectionExtensions
{
    const string Queue = "Queue";
    public static void AddServices(this IServiceCollection services, IConfiguration config)
    {
        var options = new StooqOptions();
        config.GetSection("Stooq").Bind(options);

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.Configure<StooqOptions>(config.GetSection("Stooq"));
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
            x.UsingRabbitMq((ctx, config) =>
            {
                config.AutoDelete = false;
                config.Exclusive = false;
                config.Durable = false;
                config.Host(options.Url);
            });
        });

        services.AddMassTransitHostedService();
    }
}
