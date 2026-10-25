namespace StockMarketAnalytics.Api.Models
{
    public class Instrument
    {
        public int InstrumentId { get; set; }

        public string Symbol { get; set; } = string.Empty;

        public string CompanyName { get; set; } = string.Empty;

        public string Exchange { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public DateTime CreatedAtUtc { get; set; }
    }
}
