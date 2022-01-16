using JobsityBot.Core;
using JobsityBot.Services;

namespace JobsityBot.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.Configure<StooqOptions>(config.GetSection("Stooq"));
        services.Configure<QueueOptions>(config.GetSection("Queue"));
        services.AddScoped<IStooqService, StooqService>();
        services.AddScoped<IQueueService, QueueService>();
    }
}
