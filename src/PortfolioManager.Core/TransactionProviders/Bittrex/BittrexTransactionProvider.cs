namespace PortfolioManager.Core.TransactionProviders.Bittrex
{
    using System.Globalization;
    using System.Text;
    using PortfolioManager.Core.Converters;
    using TinyCsvParser;

    public class BittrexTransactionProvider : ITransactionProvider
    {
        private readonly CoreSettings _settings;

        public BittrexTransactionProvider(CoreSettings settings) => _settings = settings;

        public IEnumerable<Transaction> GetAllTransactions()
        {
            var csvParserOptions = new CsvParserOptions(true, ',', 1, true);
            var csvMapper = new BittrexCsvTransactionMapping();
            var csvParser = new CsvParser<BittrexCsvRow>(csvParserOptions, csvMapper);
            var culture = CultureInfo.CreateSpecificCulture("en-US");

            var files = Directory.EnumerateFiles(_settings.BittrexProviderRootPath, "*.csv").OrderBy(fileName => fileName);
            foreach (var file in files)
            {
                var csvFilePath = Path.Combine(_settings.BittrexProviderRootPath, file);

                var result = csvParser
                             .ReadFromFile(csvFilePath, Encoding.ASCII)
                             .ToList();

                if (result.Any(r => !r.IsValid))
                    throw new InvalidDataException();

                var order = 0;

                foreach (var r in result)
                    yield return ConvertRowToTransaction(r.Result, culture, order++);
            }
        }

        private Transaction ConvertRowToTransaction(BittrexCsvRow row, CultureInfo dateTimeCulture, int order)
        {
            var tr = new Transaction();
            tr.TransactionSource = TransactionSource.Bittrex;
            tr.CreatedUtc = DateTime.Parse(row.Closed, dateTimeCulture);
            tr.Order = order;

            var currencies = row.Exchange.Split("-");

            if (row.OrderType.Contains("BUY"))
            {
                tr.SellAmount = row.Price + row.Commission;
                tr.SellAsset = StringToCurrencyConverter.Convert(currencies[0]);

                tr.BuyAmount = row.Quantity - row.QuantityRemaining;
                tr.BuyAsset = StringToCurrencyConverter.Convert(currencies[1]);
            }
            else if (row.OrderType.Contains("SELL"))
            {
                tr.SellAmount = row.Quantity - row.QuantityRemaining;
                tr.SellAsset = StringToCurrencyConverter.Convert(currencies[1]);

                tr.BuyAmount = row.Price - row.Commission;
                tr.BuyAsset = StringToCurrencyConverter.Convert(currencies[0]);
            }

            tr.FeeAmount = row.Commission;

            if (tr.BuyAsset == Assets.EUR || tr.SellAsset == Assets.EUR)
                tr.FeeAsset = Assets.EUR;
            else if (tr.BuyAsset == Assets.BTC || tr.SellAsset == Assets.BTC)
                tr.FeeAsset = Assets.BTC;
            else if (tr.BuyAsset == Assets.ETH || tr.SellAsset == Assets.ETH)
                tr.FeeAsset = Assets.ETH;
            else
                throw new InvalidDataException("Cannot determine transaction fee asset");

            return tr;
        }
    }
}