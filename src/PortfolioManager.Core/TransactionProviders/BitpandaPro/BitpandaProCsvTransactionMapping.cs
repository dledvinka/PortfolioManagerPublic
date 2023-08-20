namespace PortfolioManager.Core.TransactionProviders.BitpandaPro
{
    using TinyCsvParser.Mapping;

    public class BitpandaProCsvTransactionMapping : CsvMapping<BitpandaProCsvRow>
    {
        public BitpandaProCsvTransactionMapping()
            : base()
        {
            MapProperty(2, x => x.Type);
            MapProperty(3, x => x.Market);
            MapProperty(4, x => x.Amount);
            MapProperty(5, x => x.AmountCurrency);
            MapProperty(6, x => x.Price);
            MapProperty(7, x => x.PriceCurrency);
            MapProperty(10, x => x.Time);
        }
    }
}