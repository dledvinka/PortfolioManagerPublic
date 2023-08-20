namespace PortfolioManager.Core.Tests.TransactionProviders.Bittrex
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using NUnit.Framework;
    using PortfolioManager.Core.TransactionProviders.Bittrex;

    public class BittrexTransactionProviderTests
    {
        [Test]
        public void ParseAllFiles()
        {
            var currentDirectory = TestContext.CurrentContext.TestDirectory;
            var resourcesDirectory = Path.Combine(currentDirectory, "Resources", "Bittrex");
            var settings = new CoreSettings()
            {
                BittrexProviderRootPath = resourcesDirectory
            };
            var btp = new BittrexTransactionProvider(settings);

            var allTransactions = btp.GetAllTransactions().ToList();
            var transactions = allTransactions.Where(tr => tr.CreatedUtc.Year == 2018).OrderBy(tr => tr.CreatedUtc).ToList();

            AssertTransactions_2018(transactions);
        }

        private void AssertTransactions_2018(List<Transaction> transactions)
        {
            AssertTransaction_0(transactions[0]);
            AssertTransaction_1(transactions[1]);
            AssertTransaction_2(transactions[2]);
            AssertTransaction_3(transactions[3]);
            AssertTransaction_4(transactions[4]);
            AssertTransaction_5(transactions[5]);
        }

        private void AssertTransaction_0(Transaction tr)
        {
            Assert.AreEqual(TransactionSource.Bittrex, tr.TransactionSource);
            Assert.AreEqual(2018, tr.CreatedUtc.Year);
            Assert.AreEqual(1, tr.CreatedUtc.Month);
            Assert.AreEqual(2, tr.CreatedUtc.Day);
            Assert.AreEqual(19, tr.CreatedUtc.Hour);
            Assert.AreEqual(18, tr.CreatedUtc.Minute);
            Assert.AreEqual(08, tr.CreatedUtc.Second);
            Assert.AreEqual(1000.0m, tr.BuyAmount);
            Assert.AreEqual(Assets.XVG, tr.BuyAsset);
            Assert.AreEqual(0.00920295m, tr.SellAmount);
            Assert.AreEqual(Assets.BTC, tr.SellAsset);
            Assert.AreEqual(0.00002295m, tr.FeeAmount);
            Assert.AreEqual(Assets.BTC, tr.FeeAsset);
        }

        private void AssertTransaction_1(Transaction tr)
        {
            Assert.AreEqual(2018, tr.CreatedUtc.Year);
            Assert.AreEqual(1, tr.CreatedUtc.Month);
            Assert.AreEqual(21, tr.CreatedUtc.Day);
            Assert.AreEqual(11, tr.CreatedUtc.Hour);
            Assert.AreEqual(7, tr.CreatedUtc.Minute);
            Assert.AreEqual(37, tr.CreatedUtc.Second);
            Assert.AreEqual(518.37790434m, tr.BuyAmount);
            Assert.AreEqual(Assets.ADA, tr.BuyAsset);
            Assert.AreEqual(0.02756868m, tr.SellAmount);
            Assert.AreEqual(Assets.BTC, tr.SellAsset);
            Assert.AreEqual(0.00006874m, tr.FeeAmount);
            Assert.AreEqual(Assets.BTC, tr.FeeAsset);
        }

        private void AssertTransaction_2(Transaction tr)
        {
            Assert.AreEqual(2018, tr.CreatedUtc.Year);
            Assert.AreEqual(2, tr.CreatedUtc.Month);
            Assert.AreEqual(3, tr.CreatedUtc.Day);
            Assert.AreEqual(19, tr.CreatedUtc.Hour);
            Assert.AreEqual(20, tr.CreatedUtc.Minute);
            Assert.AreEqual(45, tr.CreatedUtc.Second);
            Assert.AreEqual(478.41908417m, tr.BuyAmount);
            Assert.AreEqual(Assets.XRP, tr.BuyAsset);
            Assert.AreEqual(0.04999986m, tr.SellAmount);
            Assert.AreEqual(Assets.BTC, tr.SellAsset);
            Assert.AreEqual(0.00012468m, tr.FeeAmount);
            Assert.AreEqual(Assets.BTC, tr.FeeAsset);
        }

        private void AssertTransaction_3(Transaction tr)
        {
            Assert.AreEqual(2018, tr.CreatedUtc.Year);
            Assert.AreEqual(3, tr.CreatedUtc.Month);
            Assert.AreEqual(3, tr.CreatedUtc.Day);
            Assert.AreEqual(9, tr.CreatedUtc.Hour);
            Assert.AreEqual(9, tr.CreatedUtc.Minute);
            Assert.AreEqual(47, tr.CreatedUtc.Second);
            Assert.AreEqual(0.08171765m, tr.BuyAmount);
            Assert.AreEqual(Assets.BTC, tr.BuyAsset);
            Assert.AreEqual(7.50686757m, tr.SellAmount);
            Assert.AreEqual(Assets.NEO, tr.SellAsset);
            Assert.AreEqual(0.00020476m, tr.FeeAmount);
            Assert.AreEqual(Assets.BTC, tr.FeeAsset);
        }

        private void AssertTransaction_4(Transaction tr)
        {
            Assert.AreEqual(2018, tr.CreatedUtc.Year);
            Assert.AreEqual(6, tr.CreatedUtc.Month);
            Assert.AreEqual(13, tr.CreatedUtc.Day);
            Assert.AreEqual(19, tr.CreatedUtc.Hour);
            Assert.AreEqual(37, tr.CreatedUtc.Minute);
            Assert.AreEqual(32, tr.CreatedUtc.Second);
            Assert.AreEqual(2.73693969m, tr.BuyAmount);
            Assert.AreEqual(Assets.LTC, tr.BuyAsset);
            Assert.AreEqual(0.04071769m, tr.SellAmount);
            Assert.AreEqual(Assets.BTC, tr.SellAsset);
            Assert.AreEqual(0.00010153m, tr.FeeAmount);
            Assert.AreEqual(Assets.BTC, tr.FeeAsset);
        }

        private void AssertTransaction_5(Transaction tr)
        {
            Assert.AreEqual(2018, tr.CreatedUtc.Year);
            Assert.AreEqual(8, tr.CreatedUtc.Month);
            Assert.AreEqual(2, tr.CreatedUtc.Day);
            Assert.AreEqual(20, tr.CreatedUtc.Hour);
            Assert.AreEqual(18, tr.CreatedUtc.Minute);
            Assert.AreEqual(58, tr.CreatedUtc.Second);
            Assert.AreEqual(0.12750120m, tr.BuyAmount);
            Assert.AreEqual(Assets.ETH, tr.BuyAsset);
            Assert.AreEqual(0.00693782m, tr.SellAmount);
            Assert.AreEqual(Assets.BTC, tr.SellAsset);
            Assert.AreEqual(0.0000173m, tr.FeeAmount);
            Assert.AreEqual(Assets.BTC, tr.FeeAsset);
        }
    }
}