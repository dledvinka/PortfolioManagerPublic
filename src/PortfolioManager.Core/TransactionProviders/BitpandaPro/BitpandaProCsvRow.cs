namespace PortfolioManager.Core.TransactionProviders.BitpandaPro
{
    public class BitpandaProCsvRow
    {
        public decimal Amount { get; set; }
        public string AmountCurrency { get; set; }
        public string Market { get; set; }
        public decimal Price { get; set; }
        public string PriceCurrency { get; set; }
        public string Time { get; set; }
        public string Type { get; set; }
    }
}