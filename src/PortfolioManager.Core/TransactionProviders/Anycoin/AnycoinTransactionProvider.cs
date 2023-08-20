namespace PortfolioManager.Core.TransactionProviders.Anycoin
{
    using System.Globalization;
    using System.Text;
    using PortfolioManager.Core.Converters;
    using TinyCsvParser;
    using TinyCsvParser.Mapping;

    public class AnycoinTransactionProvider : ITransactionProvider
    {
        private readonly CoreSettings _settings;

        public AnycoinTransactionProvider(CoreSettings settings) => _settings = settings;

        public IEnumerable<Transaction> GetAllTransactions()
        {
            var csvParserOptions = new CsvParserOptions(true, ',', 1, true);
            var dateTimeFormatCulture = CultureInfo.CurrentCulture;
            var numberFormatCulture = CultureInfo.CreateSpecificCulture("en-US");
            var results = new List<Transaction>();

            var files = Directory.EnumerateFiles(_settings.AnycoinProviderRootPath, "*.csv").OrderBy(fileName => fileName);
            foreach (var file in files)
            {
                var csvMapper = new AnycoinCsvTransactionMapping();
                var csvParser = new CsvParser<AnycoinCsvRow>(csvParserOptions, csvMapper);
                var csvFilePath = Path.Combine(_settings.AnycoinProviderRootPath, file);

                var result = csvParser
                             .ReadFromFile(csvFilePath, Encoding.ASCII)
                             .ToList();

                if (result.Any(r => !r.IsValid))
                    throw new InvalidDataException();

                var alreadyExecutedTransactions = result.Where(r => !string.IsNullOrWhiteSpace(r.Result.Date));

                results.AddRange(alreadyExecutedTransactions.Select(r => ConvertRowToTransaction(r, dateTimeFormatCulture, numberFormatCulture)));
            }

            var order = 0;
            results = results.OrderBy(r => r.CreatedUtc).ToList();
            results.ForEach(r => r.Order = order++);

            return results;
        }

        private Transaction ConvertRowToTransaction(CsvMappingResult<AnycoinCsvRow> mappingResult, CultureInfo dateTimeFormatCulture, CultureInfo numberFormatCulture)
        {
            var row = mappingResult.Result;

            var tr = new Transaction();
            tr.TransactionSource = TransactionSource.Anycoin;
            tr.CreatedUtc = DateTime.Parse(row.Date, dateTimeFormatCulture, DateTimeStyles.AssumeLocal);
            tr.RowIndex = mappingResult.RowIndex;

            var currencies = row.Symbol.Split("/");

            if (row.Action.Contains("BUY", StringComparison.InvariantCultureIgnoreCase))
            {
                tr.BuyAmount = row.Quantity;
                tr.BuyAsset = StringToCurrencyConverter.Convert(currencies[0]);

                tr.SellAmount = row.Quantity * row.Price;
                tr.SellAsset = StringToCurrencyConverter.Convert(currencies[1]);
            }
			else if(row.Action.Contains("SELL", StringComparison.InvariantCultureIgnoreCase))
            {
				tr.BuyAmount = row.Quantity * row.Price;
				tr.BuyAsset = StringToCurrencyConverter.Convert(currencies[1]);

				tr.SellAmount = row.Quantity;
				tr.SellAsset = StringToCurrencyConverter.Convert(currencies[0]);
			}

            return tr;
        }
    }
}