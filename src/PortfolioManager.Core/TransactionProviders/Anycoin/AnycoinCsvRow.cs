namespace PortfolioManager.Core.TransactionProviders.Anycoin
{
    public class AnycoinCsvRow
    {
        public string Date { get; set; }
        public string Symbol { get; set; }
        public string Action { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
    }
}