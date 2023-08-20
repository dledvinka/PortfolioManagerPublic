namespace PortfolioManager.Core.TransactionProviders.Coinmate
{
    using System.Globalization;
    using System.Text;
    using PortfolioManager.Core.Converters;
    using TinyCsvParser;
    using TinyCsvParser.Mapping;

    public class CoinmateTransactionProvider : ITransactionProvider
    {
        private readonly CoreSettings _settings;

        public CoinmateTransactionProvider(CoreSettings settings) => _settings = settings;

        public IEnumerable<Transaction> GetAllTransactions()
        {
            var csvParserOptions = new CsvParserOptions(true, ';', 1, true);
            var dateTimeFormatCulture = CultureInfo.CurrentCulture;
            ;
            var numberFormatCulture = CultureInfo.CreateSpecificCulture("en-US");

            var files = Directory.EnumerateFiles(_settings.CoinmateProviderRootPath, "*.csv").OrderBy(fileName => fileName);
            foreach (var file in files)
            {
                var isBefore2019Type = file.Contains("_before_2019");
                var csvMapper = new CoinmateCsvTransactionMapping(isBefore2019Type);
                var csvParser = new CsvParser<CoinmateCsvRow>(csvParserOptions, csvMapper);
                var csvFilePath = Path.Combine(_settings.CoinmateProviderRootPath, file);

                var result = csvParser
                             .ReadFromFile(csvFilePath, Encoding.ASCII)
                             .ToList();

                if (result.Any(r => !r.IsValid))
                    throw new InvalidDataException();

                var order = result.Count;

                foreach (var r in result.Where(r => r.Result.Type.Contains("BUY") || r.Result.Type.Contains("SELL")))
                    yield return ConvertRowToTransaction(r, dateTimeFormatCulture, numberFormatCulture, order--);
            }
        }

        private Transaction ConvertRowToTransaction(CsvMappingResult<CoinmateCsvRow> mappingResult, CultureInfo dateTimeFormatCulture, CultureInfo numberFormatCulture, int order)
        {
            var row = mappingResult.Result;

            var tr = new Transaction();
            tr.TransactionSource = TransactionSource.Coinmate;
            tr.CreatedUtc = DateTime.Parse(row.Date, dateTimeFormatCulture);
            tr.RowIndex = mappingResult.RowIndex;
            tr.Order = order;

            if (row.Type.Contains("BUY"))
            {
                tr.BuyAmount = row.Amount;
                tr.BuyAsset = StringToCurrencyConverter.Convert(row.AmountCurrency);

                tr.SellAmount = -row.Total;
                tr.SellAsset = StringToCurrencyConverter.Convert(row.TotalCurrency);
            }
            else if (row.Type.Contains("SELL"))
            {
                tr.BuyAmount = row.Total;
                tr.BuyAsset = StringToCurrencyConverter.Convert(row.TotalCurrency);

                tr.SellAmount = -row.Amount;
                tr.SellAsset = StringToCurrencyConverter.Convert(row.AmountCurrency);
            }

            return tr;
        }
    }
}