namespace PortfolioManager.Core.TransactionProviders.Bitpanda
{
    public class BitpandaCsvRow
    {
        public string AmountAsset { get; set; }
        public string AmountFiat { get; set; }
        public string Asset { get; set; }
        public string FiatCurrency { get; set; }
        public string Timestamp { get; set; }
        public string TransactionType { get; set; }
    }
}