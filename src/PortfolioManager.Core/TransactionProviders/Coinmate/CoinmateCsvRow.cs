namespace PortfolioManager.Core.TransactionProviders.Coinmate
{
    public class CoinmateCsvRow
    {
        public decimal Amount { get; set; }
        public string AmountCurrency { get; set; }
        public string Date { get; set; }
        public decimal Fee { get; set; }
        public string FeeCurrency { get; set; }
        public decimal Total { get; set; }
        public string TotalCurrency { get; set; }
        public string Type { get; set; }
    }
}