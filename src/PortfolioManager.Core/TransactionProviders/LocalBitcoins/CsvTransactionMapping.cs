namespace PortfolioManager.Core.TransactionProviders.LocalBitcoins
{
    using PortfolioManager.Core.Converters;
    using TinyCsvParser.Mapping;

    public class CsvTransactionMapping : CsvMapping<Transaction>
    {
        public CsvTransactionMapping()
            : base()
        {
            MapProperty(1, x => x.CreatedUtc);
            MapProperty(5, x => x.BuyAmount);
            MapProperty(10, x => x.SellAmount);
            MapProperty(13, x => x.SellAsset, new StringToCurrencyConverter());
        }
    }
}