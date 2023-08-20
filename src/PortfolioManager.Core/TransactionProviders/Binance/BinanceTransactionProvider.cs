namespace PortfolioManager.Core.TransactionProviders.Binance
{
    using System.Globalization;
    using System.Text;
    using PortfolioManager.Core.Converters;
    using TinyCsvParser;
    using TinyCsvParser.Mapping;

    /// <summary>
    /// https://www.binance.com/en-NZ/support/faq/how-to-download-spot-trading-transaction-history-statement-e4ff64f2533f4d23a0b3f8f17f510eab
    /// </summary>
    public class BinanceTransactionProvider : ITransactionProvider
    {
        private readonly CoreSettings _settings;

        public BinanceTransactionProvider(CoreSettings settings) => _settings = settings;

        public IEnumerable<Transaction> GetAllTransactions()
        {
            var csvParserOptions = new CsvParserOptions(true, ',', 1, true);
            var csvMapper = new BinanceCsvTransactionMapping();
            var csvParser = new CsvParser<BinanceCsvRow>(csvParserOptions, csvMapper);
            var culture = CultureInfo.CreateSpecificCulture("en-US");

            var files = Directory.EnumerateFiles(_settings.BinanceProviderRootPath, "*.csv", SearchOption.AllDirectories).OrderBy(fileName => fileName);
            foreach (var file in files)
            {
                var csvFilePath = Path.Combine(_settings.BinanceProviderRootPath, file);

                var result = csvParser
                             .ReadFromFile(csvFilePath, Encoding.ASCII)
                             .ToList();

                if (result.Any(r => !r.IsValid))
                    throw new InvalidDataException();

                var order = result.Count;

                foreach (var r in result)
                    yield return ConvertRowToTransaction(r, culture, order--);
            }
        }

        private Transaction ConvertRowToTransaction(CsvMappingResult<BinanceCsvRow> mappingResult, CultureInfo dateTimeCulture, int order)
        {
            var row = mappingResult.Result;

            var tr = new Transaction();
            tr.TransactionSource = TransactionSource.Binance;
            tr.CreatedUtc = DateTime.Parse(row.Date, dateTimeCulture);
            tr.RowIndex = mappingResult.RowIndex;
            tr.Order = order;

            var executed = SeparateAmountAndCurrency(row.Executed, dateTimeCulture);
            var amount = SeparateAmountAndCurrency(row.Amount, dateTimeCulture);
            var fee = SeparateAmountAndCurrency(row.Fee, dateTimeCulture);

            if (row.Side == "BUY")
            {
                tr.BuyAmount = executed.Amount - fee.Amount;
                tr.BuyAsset = StringToCurrencyConverter.Convert(executed.CurrencyCode);

                tr.SellAmount = amount.Amount;
                tr.SellAsset = StringToCurrencyConverter.Convert(amount.CurrencyCode);
            }
            else if (row.Side == "SELL")
            {
                tr.BuyAmount = amount.Amount - fee.Amount;
                tr.BuyAsset = StringToCurrencyConverter.Convert(amount.CurrencyCode);

                tr.SellAmount = executed.Amount;
                tr.SellAsset = StringToCurrencyConverter.Convert(executed.CurrencyCode);
            }

            return tr;
        }

        private (decimal Amount, string CurrencyCode) SeparateAmountAndCurrency(string value, CultureInfo culture)
        {
            var currencyCode = new string(value.Reverse().TakeWhile(char.IsLetter).Reverse().ToArray());
            var amountString = value.Substring(0, value.Length - currencyCode.Length);
            var amount = decimal.Parse(amountString, culture);

            return (amount, currencyCode);
        }
    }
}