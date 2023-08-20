namespace PortfolioManager.Core.TransactionProviders.Bitpanda
{
    using TinyCsvParser.Mapping;

    public class BitpandaCsvTransactionMapping : CsvMapping<BitpandaCsvRow>
    {
        public BitpandaCsvTransactionMapping()
            : base()
        {
            MapProperty(1, x => x.Timestamp);
            MapProperty(2, x => x.TransactionType);
            MapProperty(4, x => x.AmountFiat);
            MapProperty(5, x => x.FiatCurrency);
            MapProperty(6, x => x.AmountAsset);
            MapProperty(7, x => x.Asset);
        }
    }
}