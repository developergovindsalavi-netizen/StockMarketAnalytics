using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockMarketAnalytics.Api.DTOs;
using StockMarketAnalytics.Api.Services;

namespace StockMarketAnalytics.Api.Controllers
{
    [Route("api/instruments/{instrumentId:int}/historical-prices")]
    [ApiController]
    public class HistoricalPricesController : ControllerBase
    {
        private readonly IHistoricalPriceService _historicalPriceService;

        public HistoricalPricesController(
       IHistoricalPriceService historicalPriceService)
        {
            _historicalPriceService = historicalPriceService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<HistoricalPriceResponseDto>>> GetByInstrument(
        int instrumentId,
        CancellationToken cancellationToken)
        {
            if(instrumentId<=0)
            {
                return BadRequest("Instrument ID must be greater than zero.");
            }

            var prices = await _historicalPriceService.GetByInstrumentAsync(
            instrumentId,
            cancellationToken);

            return Ok(prices);
        }
    }
}
