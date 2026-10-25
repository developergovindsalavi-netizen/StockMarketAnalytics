namespace StockMarketAnalytics.Api.DTOs
{
    public sealed class HistoricalPriceResponseDto
    {
        public DateTime CandleTimeUtc { get; init; }

        public decimal Open { get; init; }

        public decimal High { get; init; }

        public decimal Low { get; init; }

        public decimal Close { get; init; }

        public long Volume { get; init; }
    }
}
