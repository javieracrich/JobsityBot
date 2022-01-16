using JobsityBot.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobsityBot.Controllers;

[ApiController]
[Route("[controller]")]
public class StockController : ControllerBase
{
    private readonly IStooqService stooqService;
    private readonly IQueueService queueService;

    public StockController(IStooqService stooqService, IQueueService queueService)
    {
        this.stooqService = stooqService;
        this.queueService = queueService;
    }

    [HttpGet]
    public async Task<IActionResult> GetStockQuoteAsync([FromQuery] string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return Ok();
        }
        var response = await this.stooqService.GetData(code);

        this.queueService.PublishToQueue(response);

        return Ok();
    }
}