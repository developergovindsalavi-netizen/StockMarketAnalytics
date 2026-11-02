namespace StockMarketAnalytics.Api.DTOs
{
    public class InstrumentResponseDto
    {
        public int InstrumentId { get; init; }

        public string Symbol { get; init; } = string.Empty;

        public string CompanyName { get; init; } = string.Empty;

        public string Exchange { get; init; } = string.Empty;

        public bool IsActive { get; init; }

        public DateTime CreatedAtUtc { get; init; }
    }
}
