using StockMarketAnalytics.Api.DTOs;

namespace StockMarketAnalytics.Api.Services
{
    public interface IHistoricalPriceService
    {
        Task<IReadOnlyList<HistoricalPriceResponseDto>> GetByInstrumentAsync(
        int instrumentId, DateTime? from, DateTime? to, int limit,
        CancellationToken cancellationToken);
    }
}
