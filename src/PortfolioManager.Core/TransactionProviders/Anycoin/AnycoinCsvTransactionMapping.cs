namespace PortfolioManager.Core.TransactionProviders.Anycoin
{
    using TinyCsvParser.Mapping;

    public class AnycoinCsvTransactionMapping : CsvMapping<AnycoinCsvRow>
    {
        public AnycoinCsvTransactionMapping()
            : base()
        {
            MapProperty(1, x => x.Date);
            MapProperty(2, x => x.Symbol);
            MapProperty(3, x => x.Action);
            MapProperty(4, x => x.Quantity);
            MapProperty(5, x => x.Price);
        }
    }
}