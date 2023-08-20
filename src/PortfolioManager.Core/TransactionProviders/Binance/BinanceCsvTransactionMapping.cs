namespace PortfolioManager.Core.TransactionProviders.Binance
{
    using TinyCsvParser.Mapping;

    public class BinanceCsvTransactionMapping : CsvMapping<BinanceCsvRow>
    {
        public BinanceCsvTransactionMapping()
            : base()
        {
            MapProperty(0, x => x.Date);
            MapProperty(1, x => x.Pair);
            MapProperty(2, x => x.Side);
            MapProperty(3, x => x.Price);
            MapProperty(4, x => x.Executed);
            MapProperty(5, x => x.Amount);
            MapProperty(6, x => x.Fee);
        }
    }
}