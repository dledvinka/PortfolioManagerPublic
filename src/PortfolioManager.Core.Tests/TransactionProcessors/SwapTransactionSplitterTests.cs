namespace PortfolioManager.Core.Tests.TransactionProcessors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using PortfolioManager.Core.Tests.Mocks;
    using PortfolioManager.Core.TransactionProcessors;

    public class SwapTransactionSplitterTests
    {
        [Test]
        public async Task SplitsSwapTransaction()
        {
            var conversionRate = 2.0m;
            var acs = new AssetConversionServiceMock(conversionRate);
            var swatFiatAsset = Assets.EUR;

            var swapTransaction = new Transaction()
            {
                CreatedUtc = new DateTime(2020, 1, 1).Date,
                TransactionSource = TransactionSource.BitpandaPro,
                Order = 5,
                RowIndex = 10,
                BuyAmount = 10,
                BuyAsset = Assets.ETH,
                SellAmount = 0.1m,
                SellAsset = Assets.BTC
            };

            var expectedFiatAmount = swapTransaction.SellAmount * conversionRate;

            var transactions = new List<Transaction>()
            {
                swapTransaction
            };

            var sps = new SwapTransactionSplitter(acs, Assets.EUR);

            var processedTransactions = (await sps.ProcessAsync(transactions)).ToList();

            Assert.IsTrue(processedTransactions.Count == 2);

            var tr0 = processedTransactions[0];
            Assert.AreEqual(swapTransaction.CreatedUtc, tr0.CreatedUtc);
            Assert.AreEqual(swapTransaction.TransactionSource, tr0.TransactionSource);
            Assert.AreEqual(swapTransaction.Order, tr0.Order);
            Assert.AreEqual(swapTransaction.RowIndex, tr0.RowIndex);
            Assert.AreEqual(swapTransaction.SellAmount, tr0.SellAmount);
            Assert.AreEqual(swapTransaction.SellAsset, tr0.SellAsset);
            Assert.AreEqual(expectedFiatAmount, tr0.BuyAmount);
            Assert.AreEqual(swatFiatAsset, tr0.BuyAsset);

            var tr1 = processedTransactions[1];
            Assert.AreEqual(swapTransaction.CreatedUtc, tr1.CreatedUtc);
            Assert.AreEqual(swapTransaction.TransactionSource, tr1.TransactionSource);
            Assert.AreEqual(swapTransaction.Order + 1, tr1.Order);
            Assert.AreEqual(swapTransaction.RowIndex, tr1.RowIndex);
            Assert.AreEqual(expectedFiatAmount, tr1.SellAmount);
            Assert.AreEqual(swatFiatAsset, tr1.SellAsset);
            Assert.AreEqual(swapTransaction.BuyAmount, tr1.BuyAmount);
            Assert.AreEqual(swapTransaction.BuyAsset, tr1.BuyAsset);
        }

        [Test]
        public async Task DoesNotSplitFiatTransaction()
        {
            var conversionRate = 2.0m;
            var acs = new AssetConversionServiceMock(conversionRate);
            var swatFiatAsset = Assets.EUR;

            var nonSwapTransaction = new Transaction()
            {
                CreatedUtc = new DateTime(2020, 1, 1).Date,
                TransactionSource = TransactionSource.BitpandaPro,
                Order = 5,
                RowIndex = 10,
                BuyAmount = 10,
                BuyAsset = Assets.ETH,
                SellAmount = 1000m,
                SellAsset = Assets.EUR
            };

            var transactions = new List<Transaction>()
            {
                nonSwapTransaction
            };

            var sps = new SwapTransactionSplitter(acs, Assets.EUR);

            var processedTransactions = (await sps.ProcessAsync(transactions)).ToList();

            Assert.IsTrue(processedTransactions.Count == 1);

            var tr0 = processedTransactions[0];
            Assert.AreEqual(nonSwapTransaction.CreatedUtc, tr0.CreatedUtc);
            Assert.AreEqual(nonSwapTransaction.TransactionSource, tr0.TransactionSource);
            Assert.AreEqual(nonSwapTransaction.Order, tr0.Order);
            Assert.AreEqual(nonSwapTransaction.RowIndex, tr0.RowIndex);
            Assert.AreEqual(nonSwapTransaction.SellAmount, tr0.SellAmount);
            Assert.AreEqual(nonSwapTransaction.SellAsset, tr0.SellAsset);
            Assert.AreEqual(nonSwapTransaction.BuyAmount, tr0.BuyAmount);
            Assert.AreEqual(nonSwapTransaction.BuyAsset, tr0.BuyAsset);
        }
    }
}