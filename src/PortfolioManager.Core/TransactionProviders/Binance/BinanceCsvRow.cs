namespace PortfolioManager.Core.TransactionProviders.Binance
{
    public class BinanceCsvRow
    {
        public string Amount { get; set; }
        public string Date { get; set; }
        public string Executed { get; set; }
        public string Fee { get; set; }
        public string Pair { get; set; }
        public string Price { get; set; }
        public string Side { get; set; }
    }
}