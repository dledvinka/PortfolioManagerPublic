namespace PortfolioManager.Core.Tax
{
    using PortfolioManager.Core.Services;

    public class TaxReportGenerator : ITaxReportGenerator
    {
        private readonly IAssetConversionService _assetConversionService;
        private readonly List<Transaction> _transactions;
        private readonly int startYear = 2017;

        public TaxReportGenerator(IAssetConversionService assetConversionService, IEnumerable<Transaction> transactions)
        {
            _assetConversionService = assetConversionService;
            _transactions = transactions.Select(tr => tr.Clone()).ToList();
        }

        public async Task<TaxReport> GetTaxReportAsync(Asset targetFiatAsset)
        {
            var taxReport = new TaxReport
            {
                AllTransactions = _transactions.Select(tr => tr.Clone()).ToList()
            };

            var years = Enumerable.Range(startYear, DateTime.Now.Year - startYear + 1);
            var excluded = new List<Asset>()
            {
                Assets.XLM,
                Assets.EOS,
                //Assets.ADA,
                //Assets.XRP
            };

            foreach (var asset in Assets.All.Where(asset => !asset.IsFiat && !excluded.Contains(asset)))
            {
                var assetTransactions = _transactions.Where(tr => tr.BuyAsset == asset || tr.SellAsset == asset);
                var buyTransactionQueue = new TransactionQueue(assetTransactions.Where(tr => tr.BuyAsset == asset).OrderBy(tr => tr.CreatedUtc));

                foreach (var currentYear in years)
                {
                    var sellTransactionsForCurrentYear = assetTransactions.Where(tr => tr.SellAsset == asset && tr.CreatedUtc.Year == currentYear).OrderBy(tr => tr.CreatedUtc).ToList();

                    if (!sellTransactionsForCurrentYear.Any())
                        continue;

                    var sellTransactionQueue = new Queue<Transaction>(sellTransactionsForCurrentYear);
                    var subReport = new TaxSubReport(asset, currentYear);
                    subReport.BuyTransactionQueue = buyTransactionQueue.Select(tr => tr.Clone()).ToList();
                    subReport.SellTransactionsForCurrentYear = sellTransactionsForCurrentYear;
                    taxReport.SubReports.Add(new AssetYearKey(asset, currentYear), subReport);

                    while (sellTransactionQueue.TryPeek(out var sellTransaction))
                    {
                        var transactionPairings = new List<Transaction>
                        {
                            sellTransactionQueue.Dequeue()
                        };

                        subReport.TotalSold += sellTransaction.SellAmount;
                        subReport.RealizedProfit += await _assetConversionService.ConvertAsync(sellTransaction.BuyAmount, sellTransaction.BuyAsset, targetFiatAsset, new DateTime(currentYear, 1, 1).Date);

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
                            subReport.TotalExpenditure += buyTransaction.SellAmount;
                            transactionPairings.Add(buyTransaction);
                        }

                        subReport.TransactionPairings.Add(sellTransaction.Id, transactionPairings);
                    }
                }
            }

            return taxReport;
        }
    }
}