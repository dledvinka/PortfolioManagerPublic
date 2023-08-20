namespace PortfolioManager.Core.TransactionProviders.Coinmate
{
    using TinyCsvParser.Mapping;

    public class CoinmateCsvTransactionMapping : CsvMapping<CoinmateCsvRow>
    {
        public CoinmateCsvTransactionMapping(bool before2019Type)
            : base()
        {
            var typeColumnIndex = before2019Type ? 2 : 3;

            MapProperty(1, x => x.Date);
            MapProperty(typeColumnIndex, x => x.Type);
            MapProperty(typeColumnIndex + 1, x => x.Amount);
            MapProperty(typeColumnIndex + 2, x => x.AmountCurrency);
            MapProperty(typeColumnIndex + 5, x => x.Fee);
            MapProperty(typeColumnIndex + 6, x => x.FeeCurrency);
            MapProperty(typeColumnIndex + 7, x => x.Total);
            MapProperty(typeColumnIndex + 8, x => x.TotalCurrency);
        }
    }
}