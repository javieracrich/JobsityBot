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
    public async Task<IActionResult> GetStockQuoteAsync([FromQuery] string code, [FromQuery] int roomId)
    {
        if (string.IsNullOrWhiteSpace(code) || roomId == 0)
        {
            return Ok();
        }
        var response = await this.stooqService.GetData(code);

        this.queueService.PublishToQueue(response, roomId);

        return Ok();
    }
}