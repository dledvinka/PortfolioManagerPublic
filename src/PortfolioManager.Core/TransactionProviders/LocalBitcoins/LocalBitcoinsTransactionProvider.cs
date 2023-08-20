namespace PortfolioManager.Core.TransactionProviders.LocalBitcoins
{
    using System.Text;
    using TinyCsvParser;

    public class LocalBitcoinsTransactionProvider : ITransactionProvider
    {
        private readonly CoreSettings _settings;

        public LocalBitcoinsTransactionProvider(CoreSettings settings) => _settings = settings;

        public IEnumerable<Transaction> GetAllTransactions()
        {
            var csvParserOptions = new CsvParserOptions(true, ';');
            var csvMapper = new CsvTransactionMapping();
            var csvParser = new CsvParser<Transaction>(csvParserOptions, csvMapper);

            var csvFilePath = Path.Combine(_settings.LocalBitcoinsProviderRootPath, "contacts.csv");

            var result = csvParser
                         .ReadFromFile(csvFilePath, Encoding.ASCII)
                         .ToList();

            if (result.Any(r => !r.IsValid))
                throw new InvalidDataException();

            var results = result.Select(r => r.Result).ToList();

            foreach (var r in results)
            {
                r.BuyAsset = Assets.BTC;
                r.TransactionSource = TransactionSource.LocalBitcoins;
            }

            return results;
        }
    }
}