
using Microsoft.AspNetCore.Mvc;
using StockMarketAnalytics.Api.DTOs;
using StockMarketAnalytics.Api.Services;

namespace StockMarketAnalytics.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InstrumentsController : ControllerBase
{
    private readonly IInstrumentService _instrumentService;

    public InstrumentsController(IInstrumentService instrumentService)
    {
        _instrumentService = instrumentService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponseDto<InstrumentResponseDto>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] string? exchange = null,
        CancellationToken cancellationToken = default)
    {
        if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
        {
            return BadRequest("Page number must be >= 1 and page size must be between 1 and 100.");
        }

        var result = await _instrumentService.GetAllAsync(
            pageNumber,
            pageSize,
            search,
            exchange,
            cancellationToken);

        return Ok(result);
    }
}
