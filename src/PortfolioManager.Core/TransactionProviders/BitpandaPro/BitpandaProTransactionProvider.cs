namespace PortfolioManager.Core.TransactionProviders.BitpandaPro
{
    using System.Globalization;
    using System.Text;
    using PortfolioManager.Core.Converters;
    using TinyCsvParser;
    using TinyCsvParser.Mapping;

    public class BitpandaProTransactionProvider : ITransactionProvider
    {
        private readonly CoreSettings _settings;

        public BitpandaProTransactionProvider(CoreSettings settings) => _settings = settings;

        public IEnumerable<Transaction> GetAllTransactions()
        {
            var csvParserOptions = new CsvParserOptions(true, ',', 1, true);
            var dateTimeFormatCulture = CultureInfo.CurrentCulture;
            var numberFormatCulture = CultureInfo.CreateSpecificCulture("en-US");
            var results = new List<Transaction>();

            var files = Directory.EnumerateFiles(_settings.BitpandaProProviderRootPath, "headerless*.csv").OrderBy(fileName => fileName);
            foreach (var file in files)
            {
                var csvMapper = new BitpandaProCsvTransactionMapping();
                var csvParser = new CsvParser<BitpandaProCsvRow>(csvParserOptions, csvMapper);
                var csvFilePath = Path.Combine(_settings.BitpandaProProviderRootPath, file);

                var result = csvParser
                             .ReadFromFile(csvFilePath, Encoding.ASCII)
                             .ToList();


                if (result.Any(r => !r.IsValid))
                    throw new InvalidDataException();

                results.AddRange(result.Select(r => ConvertRowToTransaction(r, dateTimeFormatCulture, numberFormatCulture)));
            }

            var order = 0;
            results = results.OrderBy(r => r.CreatedUtc).ToList();
            results.ForEach(r => r.Order = order++);

            return results;
        }

        private Transaction ConvertRowToTransaction(CsvMappingResult<BitpandaProCsvRow> mappingResult, CultureInfo dateTimeFormatCulture, CultureInfo numberFormatCulture)
        {
            var row = mappingResult.Result;

            var tr = new Transaction();
            tr.TransactionSource = TransactionSource.BitpandaPro;
            tr.CreatedUtc = DateTime.Parse(row.Time, dateTimeFormatCulture, DateTimeStyles.AssumeLocal);
            tr.RowIndex = mappingResult.RowIndex;

            if (row.Type.Contains("BUY", StringComparison.InvariantCultureIgnoreCase))
            {
                tr.BuyAmount = row.Amount;
                tr.BuyAsset = StringToCurrencyConverter.Convert(row.AmountCurrency);

                tr.SellAmount = row.Amount * row.Price;
                tr.SellAsset = StringToCurrencyConverter.Convert(row.PriceCurrency);
            }
            else if (row.Type.Contains("SELL", StringComparison.InvariantCultureIgnoreCase))
            {
                tr.BuyAmount = row.Amount * row.Price;
                tr.BuyAsset = StringToCurrencyConverter.Convert(row.PriceCurrency);

                tr.SellAmount = row.Amount;
                tr.SellAsset = StringToCurrencyConverter.Convert(row.AmountCurrency);
            }

            return tr;
        }
    }
}