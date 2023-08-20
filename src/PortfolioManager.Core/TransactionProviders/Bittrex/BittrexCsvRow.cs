namespace PortfolioManager.Core.TransactionProviders.Bittrex
{
    public class BittrexCsvRow
    {
        public string Closed { get; set; }
        public decimal Commission { get; set; }
        public string Exchange { get; set; }
        public string OrderType { get; set; }
        public decimal Price { get; set; }
        public decimal PricePerUnit { get; set; }
        public decimal Quantity { get; set; }
        public decimal QuantityRemaining { get; set; }
        public string TimeStamp { get; set; }
    }
}