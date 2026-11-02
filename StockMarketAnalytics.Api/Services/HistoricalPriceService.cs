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
            int instrumentId, DateTime? from, DateTime? to, int limit,
            CancellationToken cancellationToken)
        {
            var query = _context.HistoricalPrice
                .AsNoTracking()
                .Where(s => s.InstrumentId == instrumentId);

            if(from.HasValue)
            {
                query = query.Where(x=> x.CandleTimeUtc >= from);
            }

            if (to.HasValue)
            {
                query = query.Where(x => x.CandleTimeUtc <= to);
            }


            return await query.OrderBy(s => s.CandleTimeUtc)
                .Take(limit)
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
