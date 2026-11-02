using StockMarketAnalytics.Api.DTOs;
using StockMarketAnalytics.Api.Models;

namespace StockMarketAnalytics.Api.Services
{
    public interface IInstrumentService
    {
        Task<PagedResponseDto<InstrumentResponseDto>> GetAllAsync(
         int pageNumber,
         int pageSize,
         string? search,
         string? exchange,
         CancellationToken cancellationToken);
    }
}
