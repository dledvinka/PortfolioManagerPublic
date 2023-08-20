namespace PortfolioManager.Core.TransactionProviders.Bittrex
{
    using TinyCsvParser.Mapping;

    public class BittrexCsvTransactionMapping : CsvMapping<BittrexCsvRow>
    {
        public BittrexCsvTransactionMapping()
            : base()
        {
            MapProperty(1, x => x.Exchange);
            MapProperty(2, x => x.TimeStamp);
            MapProperty(3, x => x.OrderType);
            MapProperty(5, x => x.Quantity);
            MapProperty(6, x => x.QuantityRemaining);
            MapProperty(7, x => x.Commission);
            MapProperty(8, x => x.Price);
            MapProperty(9, x => x.PricePerUnit);
            MapProperty(14, x => x.Closed);
        }
    }
}