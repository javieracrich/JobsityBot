using Jobsity;
using JobsityBot.Services;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace JobsityBot.Controllers;

[ApiController]
[Route("[controller]")]
public class StockController : ControllerBase
{
    private readonly IStooqService stooqService;
    private readonly IPublishEndpoint publishEndpoint;

    public StockController(IStooqService stooqService, IPublishEndpoint publishEndpoint)
    {
        this.stooqService = stooqService;
        this.publishEndpoint = publishEndpoint;
    }

    [HttpGet]
    public async Task<IActionResult> GetStockQuoteAsync([FromQuery] string code, [FromQuery] int roomId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(code) || roomId == 0)
        {
            return Ok();
        }
        var response = await this.stooqService.GetData(code);

        await this.publishEndpoint.Publish(new BotMessage(response, roomId), null, cancellationToken);

        return Ok();
    }
}