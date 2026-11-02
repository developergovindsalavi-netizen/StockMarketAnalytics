using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockMarketAnalytics.Api.Services;
using StockMarketAnalytics.Api.DTOs;

namespace StockMarketAnalytics.Api.Controllers
{
    [Route("api/instruments/{instrumentId:int}/historical-prices")]
    [ApiController]
    public class HistoricalPricesController : ControllerBase
    {
        private readonly IHistoricalPriceService _historicalPriceService;

        private const int DefaultLimit = 500;
        private const int MaximumLimit = 1000;

        public HistoricalPricesController(
       IHistoricalPriceService historicalPriceService)
        {
            _historicalPriceService = historicalPriceService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<HistoricalPriceResponseDto>>> GetByInstrument(
        int instrumentId, [FromQuery] DateTime? from, [FromQuery] DateTime? to, [FromQuery]  int? limit,
        CancellationToken cancellationToken)
        {
            if(instrumentId<=0)
            {
                return BadRequest("Instrument ID must be greater than zero.");
            }

            if (from.HasValue && to.HasValue && from.Value.Date > to.Value.Date)
                return BadRequest("'from' date cannot be after 'to' date.");

            if(limit.HasValue && (limit.Value <1 || limit.Value > MaximumLimit))
                return BadRequest($"Limit must be between 1 and {MaximumLimit}.");

            var requestedLimit = limit ?? DefaultLimit;

            var prices = await _historicalPriceService.GetByInstrumentAsync(
            instrumentId,from,to, requestedLimit,
            cancellationToken);

            return Ok(prices);
        }
    }
}
