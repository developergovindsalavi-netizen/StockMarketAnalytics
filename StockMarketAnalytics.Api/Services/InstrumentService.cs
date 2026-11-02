using Microsoft.EntityFrameworkCore;
using StockMarketAnalytics.Api.Data;
using StockMarketAnalytics.Api.DTOs;
using StockMarketAnalytics.Api.Models;

namespace StockMarketAnalytics.Api.Services
{
    public class InstrumentService : IInstrumentService
    {
        private readonly AppDbContext _context;

        public InstrumentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResponseDto<InstrumentResponseDto>> GetAllAsync(
        int pageNumber,
        int pageSize,
        string? search,
        string? exchange,
        CancellationToken cancellationToken)
        {
            var query = _context.Instruments.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchTerm = search.Trim();

                query = query.Where(x =>
                    x.Symbol.Contains(searchTerm) ||
                    x.CompanyName.Contains(searchTerm));
            }

            if (!string.IsNullOrWhiteSpace(exchange))
            {
                var exchangeCode = exchange.Trim().ToUpperInvariant();

                query = query.Where(x => x.Exchange == exchangeCode);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderBy(x => x.Symbol)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new InstrumentResponseDto
                {
                    InstrumentId = x.InstrumentId,
                    Symbol = x.Symbol,
                    CompanyName = x.CompanyName,
                    Exchange = x.Exchange,
                    IsActive = x.IsActive,
                    CreatedAtUtc = x.CreatedAtUtc
                })
                .ToListAsync(cancellationToken);

            return new PagedResponseDto<InstrumentResponseDto>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

    }
}
