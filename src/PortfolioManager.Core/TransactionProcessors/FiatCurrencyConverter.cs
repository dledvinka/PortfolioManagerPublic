namespace PortfolioManager.Core.TransactionProcessors
{
    using PortfolioManager.Core.Services;

    public class FiatCurrencyConverter : ITransactionProcessor
    {
        private readonly IAssetConversionService _assetConversionService;
        private readonly Asset _targetFiatAsset;

        public FiatCurrencyConverter(IAssetConversionService assetConversionService, Asset targetFiatAsset)
        {
            _assetConversionService = assetConversionService;
            _targetFiatAsset = targetFiatAsset;
        }

        public async Task<IEnumerable<Transaction>> ProcessAsync(IEnumerable<Transaction> transactions)
        {
            var processedTransactions = new List<Transaction>();

            foreach (var transaction in transactions)
            {
                if (transaction.BuyAsset.IsFiat)
                {
                    transaction.BuyAmount = await _assetConversionService.ConvertAsync(transaction.BuyAmount, transaction.BuyAsset, _targetFiatAsset, transaction.CreatedUtc);
                    transaction.BuyAsset = _targetFiatAsset;
                }
                else if (transaction.SellAsset.IsFiat)
                {
                    transaction.SellAmount = await _assetConversionService.ConvertAsync(transaction.SellAmount, transaction.SellAsset, _targetFiatAsset, transaction.CreatedUtc);
                    transaction.SellAsset = _targetFiatAsset;
                }

                processedTransactions.Add(transaction);
            }

            return processedTransactions;
        }
    }
}