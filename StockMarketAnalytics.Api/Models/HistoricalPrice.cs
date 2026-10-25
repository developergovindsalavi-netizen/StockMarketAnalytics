namespace StockMarketAnalytics.Api.Models
{
    public class HistoricalPrice
    {
        public int HistoricalPriceId { get; set; }
        public int InstrumentId { get; set; }
        public DateTime CandleTimeUtc { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public long Volume { get; set; }
        public Instrument Instrument { get; set; } = null;
    }
}
