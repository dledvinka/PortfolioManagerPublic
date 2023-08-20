namespace PortfolioManager.Core.TransactionProcessors
{
    using PortfolioManager.Core.Services;

    public class SwapTransactionSplitter : ITransactionProcessor
    {
        private readonly IAssetConversionService _assetConversionService;
        private readonly Asset _swapFiatAsset;

        public SwapTransactionSplitter(IAssetConversionService assetConversionService, Asset swapFiatAsset)
        {
            _assetConversionService = assetConversionService;
            _swapFiatAsset = swapFiatAsset;
        }

        public async Task<IEnumerable<Transaction>> ProcessAsync(IEnumerable<Transaction> transactions)
        {
            var resultTransactions = new List<Transaction>();

            foreach (var sourceTransaction in transactions)
            {
                if (!sourceTransaction.IsSwap)
                {
                    resultTransactions.Add(sourceTransaction);
                    continue;
                }

                var toFiatTransaction = new Transaction
                {
                    CreatedUtc = sourceTransaction.CreatedUtc,
                    TransactionSource = sourceTransaction.TransactionSource,
                    Order = sourceTransaction.Order,
                    RowIndex = sourceTransaction.RowIndex,
                    SellAmount = sourceTransaction.SellAmount,
                    SellAsset = sourceTransaction.SellAsset,
                    BuyAmount = await _assetConversionService.ConvertAsync(sourceTransaction.SellAmount, sourceTransaction.SellAsset, _swapFiatAsset, sourceTransaction.CreatedUtc),
                    BuyAsset = _swapFiatAsset
                };

                resultTransactions.Add(toFiatTransaction);

                var toCryptoTransaction = new Transaction
                {
                    CreatedUtc = sourceTransaction.CreatedUtc,
                    TransactionSource = sourceTransaction.TransactionSource,
                    Order = sourceTransaction.Order + 1,
                    RowIndex = sourceTransaction.RowIndex,
                    SellAmount = toFiatTransaction.BuyAmount,
                    SellAsset = _swapFiatAsset,
                    BuyAmount = sourceTransaction.BuyAmount,
                    BuyAsset = sourceTransaction.BuyAsset
                };

                resultTransactions.Add(toCryptoTransaction);
            }

            return resultTransactions;
        }
    }
}