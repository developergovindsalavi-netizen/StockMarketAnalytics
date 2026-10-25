using StockMarketAnalytics.Api.Data;
using StockMarketAnalytics.Api.DTOs;
using Microsoft.EntityFrameworkCore;

namespace StockMarketAnalytics.Api.Services
{
    public class HistoricalPriceService : IHistoricalPriceService
    {
        private readonly AppDbContext _context;

        public HistoricalPriceService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<HistoricalPriceResponseDto>>
        GetByInstrumentAsync(
            int instrumentId,
            CancellationToken cancellationToken)
        {
            return await _context.HistoricalPrice
                .AsNoTracking()
                .Where(s => s.InstrumentId == instrumentId)
                .OrderBy(s => s.CandleTimeUtc)
                .Select(s => new HistoricalPriceResponseDto
                {
                    CandleTimeUtc = s.CandleTimeUtc,
                    Open = s.Open,
                    High = s.High,
                    Low = s.Low,
                    Close = s.Close,
                    Volume = s.Volume
                })
                .ToListAsync();
        }
    }
}
