namespace PortfolioManager.Core
{
	using PortfolioManager.Core.Services;
    using PortfolioManager.Core.Tax;

    public class Portfolio
    {
        private readonly IAssetConversionService _conversionService;
        private readonly IEnumerable<Transaction> _transactions;

        public Portfolio(IEnumerable<Transaction> transactions, IAssetConversionService conversionService)
        {
            _transactions = transactions;
            _conversionService = conversionService;
        }

        public async Task<IEnumerable<AssetAmount>> GetCurrentBalancesAsync()
        {
            var results = new List<AssetAmount>();

            foreach (var currency in Assets.All)
            {
                var balance = await GetCurrentBalanceAsync(currency);
                results.Add(balance);
            }

            return results;
        }

        public Task<AssetAmount> GetCurrentBalanceAsync(Asset asset) => GetCurrentBalanceAsync(asset, asset);

        public async Task<AssetAmount> GetCurrentBalanceAsync(Asset asset, Asset targetAsset)
        {
            var balance = 0.0m;

            balance += _transactions.Where(tr => tr.BuyAsset == asset).Sum(tr => tr.BuyAmount);
            balance += _transactions.Where(tr => tr.SellAsset == asset).Sum(tr => -tr.SellAmount);

            var balanceInTargetCurrency = await _conversionService.ConvertAsync(balance, asset, targetAsset, DateTime.UtcNow);

            return new AssetAmount(balanceInTargetCurrency, targetAsset);
        }

		public async Task<decimal> GetCurrentBalanceWeightedAverageBuyPriceAsync(Asset asset, Asset targetAsset)
		{
			var buyAssetTransactions = _transactions.Where(tr => tr.BuyAsset == asset);
			var buyTransactionQueue = new TransactionQueue(buyAssetTransactions.OrderBy(tr => tr.CreatedUtc));

			var sellAssetTransactions = _transactions.Where(tr => tr.SellAsset == asset);
			var sellTransactionQueue = new Queue<Transaction>(sellAssetTransactions.OrderBy(tr => tr.CreatedUtc));

			while (sellTransactionQueue.TryPeek(out var sellTransaction))
			{
				var transactionPairings = new List<Transaction>
				{
					sellTransactionQueue.Dequeue()
				};

				var remainingTransactionAmount = sellTransaction.SellAmount;

				while (remainingTransactionAmount > 0)
				{
					if (!buyTransactionQueue.TryPeek(out var buyTransaction))
						throw new InvalidDataException("SELL transaction is not covered with BUY transaction");

					if (buyTransaction.BuyAmount <= remainingTransactionAmount)
						buyTransaction = buyTransactionQueue.DequeueWhole();
					else
						buyTransaction = buyTransactionQueue.DequeuePart(remainingTransactionAmount);

					remainingTransactionAmount -= buyTransaction.BuyAmount;
					transactionPairings.Add(buyTransaction);
				}
			}

			var sellAmountSum = 0m;

			foreach (var buyTransaction in buyTransactionQueue)
			{
				sellAmountSum += await _conversionService.ConvertAsync(buyTransaction.SellAmount, buyTransaction.SellAsset, targetAsset, buyTransaction.CreatedUtc);
			}

			var buyAmountSum = buyTransactionQueue.Sum(tr => tr.BuyAmount);
			var weightedAverageBuyPrice = sellAmountSum / buyAmountSum;

			return weightedAverageBuyPrice;
		}
	}
}