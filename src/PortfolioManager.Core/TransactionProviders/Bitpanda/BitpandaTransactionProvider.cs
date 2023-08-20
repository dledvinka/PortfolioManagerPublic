namespace PortfolioManager.Core.TransactionProviders.Bitpanda
{
    using System.Globalization;
    using System.Text;
    using PortfolioManager.Core.Converters;
    using TinyCsvParser;
    using TinyCsvParser.Mapping;

    public class BitpandaTransactionProvider : ITransactionProvider
    {
        private readonly CoreSettings _settings;
        private int order = 0;

        public BitpandaTransactionProvider(CoreSettings settings) => _settings = settings;

        public IEnumerable<Transaction> GetAllTransactions()
        {
            var csvParserOptions = new CsvParserOptions(true, ',', 1, true);
            var dateTimeFormatCulture = CultureInfo.CurrentCulture;
            ;
            var numberFormatCulture = CultureInfo.CreateSpecificCulture("en-US");

            var files = Directory.EnumerateFiles(_settings.BitpandaProviderRootPath, "headerless*.csv").OrderBy(fileName => fileName);
            foreach (var file in files)
            {
                var csvMapper = new BitpandaCsvTransactionMapping();
                var csvParser = new CsvParser<BitpandaCsvRow>(csvParserOptions, csvMapper);
                var csvFilePath = Path.Combine(_settings.BitpandaProviderRootPath, file);

                var result = csvParser
                             .ReadFromFile(csvFilePath, Encoding.ASCII)
                             .ToList();

                if (result.Any(r => !r.IsValid))
                    throw new InvalidDataException();

                foreach (var r in result.Where(r =>
                                                   r.Result.TransactionType.Contains("BUY", StringComparison.InvariantCultureIgnoreCase) ||
                                                   r.Result.TransactionType.Contains("SELL", StringComparison.InvariantCultureIgnoreCase)))
                    yield return ConvertRowToTransaction(r, dateTimeFormatCulture, numberFormatCulture);
            }
        }

        private Transaction ConvertRowToTransaction(CsvMappingResult<BitpandaCsvRow> mappingResult, CultureInfo dateTimeFormatCulture, CultureInfo numberFormatCulture)
        {
            var row = mappingResult.Result;

            var tr = new Transaction();
            tr.TransactionSource = TransactionSource.Bitpanda;
            tr.CreatedUtc = DateTime.Parse(row.Timestamp, dateTimeFormatCulture);
            tr.RowIndex = mappingResult.RowIndex;
            tr.Order = order++;

            if (row.TransactionType.Contains("BUY", StringComparison.InvariantCultureIgnoreCase))
            {
                tr.BuyAmount = decimal.Parse(row.AmountAsset, numberFormatCulture);
                tr.BuyAsset = StringToCurrencyConverter.Convert(row.Asset);

                tr.SellAmount = decimal.Parse(row.AmountFiat, numberFormatCulture);
                tr.SellAsset = StringToCurrencyConverter.Convert(row.FiatCurrency);
            }
            else if (row.TransactionType.Contains("SELL", StringComparison.InvariantCultureIgnoreCase))
            {
                tr.BuyAmount = decimal.Parse(row.AmountFiat, numberFormatCulture);
                tr.BuyAsset = StringToCurrencyConverter.Convert(row.FiatCurrency);

                tr.SellAmount = decimal.Parse(row.AmountAsset, numberFormatCulture);
                tr.SellAsset = StringToCurrencyConverter.Convert(row.Asset);
            }

            return tr;
        }
    }
}