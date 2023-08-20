namespace PortfolioManager.Core.Tests.TransactionProviders.Binance
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using NUnit.Framework;
    using PortfolioManager.Core.TransactionProviders.Binance;

    public class BinanceTransactionProviderTests
    {
        [Test]
        public void ParseAllFiles()
        {
            var currentDirectory = TestContext.CurrentContext.TestDirectory;
            var resourcesDirectory = Path.Combine(currentDirectory, "Resources", "Binance");

            var settings = new CoreSettings()
            {
                BinanceProviderRootPath = resourcesDirectory
            };
            var btp = new BinanceTransactionProvider(settings);

            var allTransactions = btp.GetAllTransactions().ToList();
            var transactionsOf2017 = allTransactions.Where(tr => tr.CreatedUtc.Year == 2017).OrderBy(tr => tr.CreatedUtc).ThenBy(tr => tr.Order).ToList();
            var transactionsOf2021 = allTransactions.Where(tr => tr.CreatedUtc.Year == 2021).OrderBy(tr => tr.CreatedUtc).ThenBy(tr => tr.Order).ToList();

            AssertTransactions_2017(transactionsOf2017);
            AssertTransactions_2021(transactionsOf2021);
        }

        private void AssertTransactions_2017(List<Transaction> transactions)
        {
            AssertTransaction_2017_0(transactions[0]);
            AssertTransaction_2017_1(transactions[1]);
            AssertTransaction_2017_2(transactions[2]);
            AssertTransaction_2017_5(transactions[5]);
        }

        private void AssertTransactions_2021(List<Transaction> transactions)
        {
            AssertTransaction_2021_0(transactions[0]);
            AssertTransaction_2021_8(transactions[8]);
            AssertTransaction_2021_10(transactions[10]);
        }

        private void AssertTransaction_2017_0(Transaction tr)
        {
            Assert.AreEqual(TransactionSource.Binance, tr.TransactionSource);
            Assert.AreEqual(2017, tr.CreatedUtc.Year);
            Assert.AreEqual(12, tr.CreatedUtc.Month);
            Assert.AreEqual(25, tr.CreatedUtc.Day);
            Assert.AreEqual(18, tr.CreatedUtc.Hour);
            Assert.AreEqual(07, tr.CreatedUtc.Minute);
            Assert.AreEqual(56, tr.CreatedUtc.Second);
            Assert.AreEqual(9, tr.RowIndex);
            Assert.AreEqual(74m - 0.074m, tr.BuyAmount);
            Assert.AreEqual(Assets.SUB, tr.BuyAsset);
            Assert.AreEqual(0.00588226m, tr.SellAmount);
            Assert.AreEqual(Assets.BTC, tr.SellAsset);
        }

        private void AssertTransaction_2017_1(Transaction tr)
        {
            Assert.AreEqual(TransactionSource.Binance, tr.TransactionSource);
            Assert.AreEqual(2017, tr.CreatedUtc.Year);
            Assert.AreEqual(12, tr.CreatedUtc.Month);
            Assert.AreEqual(25, tr.CreatedUtc.Day);
            Assert.AreEqual(18, tr.CreatedUtc.Hour);
            Assert.AreEqual(07, tr.CreatedUtc.Minute);
            Assert.AreEqual(56, tr.CreatedUtc.Second);
            Assert.AreEqual(8, tr.RowIndex);
            Assert.AreEqual(29m - 0.029m, tr.BuyAmount);
            Assert.AreEqual(Assets.SUB, tr.BuyAsset);
            Assert.AreEqual(0.00230521m, tr.SellAmount);
            Assert.AreEqual(Assets.BTC, tr.SellAsset);
        }

        private void AssertTransaction_2017_2(Transaction tr)
        {
            Assert.AreEqual(TransactionSource.Binance, tr.TransactionSource);
            Assert.AreEqual(2017, tr.CreatedUtc.Year);
            Assert.AreEqual(12, tr.CreatedUtc.Month);
            Assert.AreEqual(25, tr.CreatedUtc.Day);
            Assert.AreEqual(18, tr.CreatedUtc.Hour);
            Assert.AreEqual(07, tr.CreatedUtc.Minute);
            Assert.AreEqual(56, tr.CreatedUtc.Second);
            Assert.AreEqual(7, tr.RowIndex);
            Assert.AreEqual(47m - 0.047m, tr.BuyAmount);
            Assert.AreEqual(Assets.SUB, tr.BuyAsset);
            Assert.AreEqual(0.00373603m, tr.SellAmount);
            Assert.AreEqual(Assets.BTC, tr.SellAsset);
        }

        private void AssertTransaction_2017_5(Transaction tr)
        {
            Assert.AreEqual(TransactionSource.Binance, tr.TransactionSource);
            Assert.AreEqual(2017, tr.CreatedUtc.Year);
            Assert.AreEqual(12, tr.CreatedUtc.Month);
            Assert.AreEqual(25, tr.CreatedUtc.Day);
            Assert.AreEqual(18, tr.CreatedUtc.Hour);
            Assert.AreEqual(08, tr.CreatedUtc.Minute);
            Assert.AreEqual(37, tr.CreatedUtc.Second);
            Assert.AreEqual(4, tr.RowIndex);
            Assert.AreEqual(63.35m - 0.06335m, tr.BuyAmount);
            Assert.AreEqual(Assets.ICX, tr.BuyAsset);
            Assert.AreEqual(0.03196008m, tr.SellAmount);
            Assert.AreEqual(Assets.BTC, tr.SellAsset);
        }

        private void AssertTransaction_2021_0(Transaction tr)
        {
            Assert.AreEqual(TransactionSource.Binance, tr.TransactionSource);
            Assert.AreEqual(2021, tr.CreatedUtc.Year);
            Assert.AreEqual(2, tr.CreatedUtc.Month);
            Assert.AreEqual(20, tr.CreatedUtc.Day);
            Assert.AreEqual(10, tr.CreatedUtc.Hour);
            Assert.AreEqual(44, tr.CreatedUtc.Minute);
            Assert.AreEqual(15, tr.CreatedUtc.Second);
            Assert.AreEqual(11, tr.RowIndex);
            Assert.AreEqual(0.06587280m - 0.0000658700m, tr.BuyAmount);
            Assert.AreEqual(Assets.ETH, tr.BuyAsset);
            Assert.AreEqual(252m, tr.SellAmount);
            Assert.AreEqual(Assets.XLM, tr.SellAsset);
        }

        private void AssertTransaction_2021_8(Transaction tr)
        {
            Assert.AreEqual(TransactionSource.Binance, tr.TransactionSource);
            Assert.AreEqual(2021, tr.CreatedUtc.Year);
            Assert.AreEqual(4, tr.CreatedUtc.Month);
            Assert.AreEqual(6, tr.CreatedUtc.Day);
            Assert.AreEqual(11, tr.CreatedUtc.Hour);
            Assert.AreEqual(23, tr.CreatedUtc.Minute);
            Assert.AreEqual(17, tr.CreatedUtc.Second);
            Assert.AreEqual(3, tr.RowIndex);
            Assert.AreEqual(171.03178400m - 0.1710317800m, tr.BuyAmount);
            Assert.AreEqual(Assets.USDT, tr.BuyAsset);
            Assert.AreEqual(2.749m, tr.SellAmount);
            Assert.AreEqual(Assets.NEO, tr.SellAsset);
        }

        private void AssertTransaction_2021_10(Transaction tr)
        {
            Assert.AreEqual(TransactionSource.Binance, tr.TransactionSource);
            Assert.AreEqual(2021, tr.CreatedUtc.Year);
            Assert.AreEqual(4, tr.CreatedUtc.Month);
            Assert.AreEqual(12, tr.CreatedUtc.Day);
            Assert.AreEqual(18, tr.CreatedUtc.Hour);
            Assert.AreEqual(1, tr.CreatedUtc.Minute);
            Assert.AreEqual(1, tr.CreatedUtc.Second);
            Assert.AreEqual(1, tr.RowIndex);
            Assert.AreEqual(0.23679700m - 0.0001776000m, tr.BuyAmount);
            Assert.AreEqual(Assets.BNB, tr.BuyAsset);
            Assert.AreEqual(209m, tr.SellAmount);
            Assert.AreEqual(Assets.WABI, tr.SellAsset);
        }
    }
}