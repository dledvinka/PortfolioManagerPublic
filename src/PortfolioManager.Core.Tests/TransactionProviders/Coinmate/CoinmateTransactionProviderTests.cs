namespace PortfolioManager.Core.Tests.TransactionProviders.Coinmate
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using NUnit.Framework;
    using PortfolioManager.Core.TransactionProviders.Coinmate;

    public class CoinmateTransactionProviderTests
    {
        [Test]
        public void ParseAllFiles()
        {
            var currentDirectory = TestContext.CurrentContext.TestDirectory;
            var resourcesDirectory = Path.Combine(currentDirectory, "Resources", "Coinmate");

            var settings = new CoreSettings()
            {
                CoinmateProviderRootPath = resourcesDirectory
            };
            var btp = new CoinmateTransactionProvider(settings);

            var allTransactions = btp.GetAllTransactions().ToList();
            var transactionsOf2018 = allTransactions.Where(tr => tr.CreatedUtc.Year == 2018).OrderBy(tr => tr.CreatedUtc).ThenBy(tr => tr.Order).ToList();
            var otherTransactions = allTransactions.Where(tr => tr.CreatedUtc.Year >= 2019).OrderBy(tr => tr.CreatedUtc).ThenBy(tr => tr.Order).ToList();

            AssertTransactions_2018(transactionsOf2018);
            AssertTransactions_Other(otherTransactions);
        }

        private void AssertTransactions_2018(List<Transaction> transactions)
        {
            AssertTransaction_2018_0(transactions[0]);
            AssertTransaction_2018_4(transactions[4]);
            AssertTransaction_2018_10(transactions[10]);
            AssertTransaction_2018_20(transactions[20]);
        }

        private void AssertTransactions_Other(List<Transaction> transactions)
        {
            AssertTransaction_Other_0(transactions[0]);
            AssertTransaction_Other_8(transactions[8]);
            AssertTransaction_Other_9(transactions[9]);
            AssertTransaction_Other_10(transactions[10]);
        }

        private void AssertTransaction_2018_0(Transaction tr)
        {
            Assert.AreEqual(TransactionSource.Coinmate, tr.TransactionSource);
            Assert.AreEqual(2018, tr.CreatedUtc.Year);
            Assert.AreEqual(3, tr.CreatedUtc.Month);
            Assert.AreEqual(3, tr.CreatedUtc.Day);
            Assert.AreEqual(8, tr.CreatedUtc.Hour);
            Assert.AreEqual(39, tr.CreatedUtc.Minute);
            Assert.AreEqual(0, tr.CreatedUtc.Second);
            Assert.AreEqual(47, tr.RowIndex);
            Assert.AreEqual(0.15131422m, tr.BuyAmount);
            Assert.AreEqual(Assets.BTC, tr.BuyAsset);
            Assert.AreEqual(34999.99893853m, tr.SellAmount);
            Assert.AreEqual(Assets.CZK, tr.SellAsset);
        }

        private void AssertTransaction_2018_4(Transaction tr)
        {
            Assert.AreEqual(TransactionSource.Coinmate, tr.TransactionSource);
            Assert.AreEqual(2018, tr.CreatedUtc.Year);
            Assert.AreEqual(6, tr.CreatedUtc.Month);
            Assert.AreEqual(12, tr.CreatedUtc.Day);
            Assert.AreEqual(17, tr.CreatedUtc.Hour);
            Assert.AreEqual(1, tr.CreatedUtc.Minute);
            Assert.AreEqual(0, tr.CreatedUtc.Second);
            Assert.AreEqual(35, tr.RowIndex);
            Assert.AreEqual(0.43335381m, tr.BuyAmount);
            Assert.AreEqual(Assets.LTC, tr.BuyAsset);
            Assert.AreEqual(999.9999781m, tr.SellAmount);
            Assert.AreEqual(Assets.CZK, tr.SellAsset);
        }

        private void AssertTransaction_2018_10(Transaction tr)
        {
            Assert.AreEqual(TransactionSource.Coinmate, tr.TransactionSource);
            Assert.AreEqual(2018, tr.CreatedUtc.Year);
            Assert.AreEqual(7, tr.CreatedUtc.Month);
            Assert.AreEqual(11, tr.CreatedUtc.Day);
            Assert.AreEqual(15, tr.CreatedUtc.Hour);
            Assert.AreEqual(1, tr.CreatedUtc.Minute);
            Assert.AreEqual(0, tr.CreatedUtc.Second);
            Assert.AreEqual(25, tr.RowIndex);
            Assert.AreEqual(0.1m, tr.BuyAmount);
            Assert.AreEqual(Assets.LTC, tr.BuyAsset);
            Assert.AreEqual(173.182548m, tr.SellAmount);
            Assert.AreEqual(Assets.CZK, tr.SellAsset);
        }

        private void AssertTransaction_2018_20(Transaction tr)
        {
            Assert.AreEqual(TransactionSource.Coinmate, tr.TransactionSource);
            Assert.AreEqual(2018, tr.CreatedUtc.Year);
            Assert.AreEqual(12, tr.CreatedUtc.Month);
            Assert.AreEqual(15, tr.CreatedUtc.Day);
            Assert.AreEqual(10, tr.CreatedUtc.Hour);
            Assert.AreEqual(37, tr.CreatedUtc.Minute);
            Assert.AreEqual(0, tr.CreatedUtc.Second);
            Assert.AreEqual(3, tr.RowIndex);
            Assert.AreEqual(0.06892244m, tr.BuyAmount);
            Assert.AreEqual(Assets.BTC, tr.BuyAsset);
            Assert.AreEqual(4999.99940702m, tr.SellAmount);
            Assert.AreEqual(Assets.CZK, tr.SellAsset);
        }

        private void AssertTransaction_Other_0(Transaction tr)
        {
            Assert.AreEqual(TransactionSource.Coinmate, tr.TransactionSource);
            Assert.AreEqual(2019, tr.CreatedUtc.Year);
            Assert.AreEqual(3, tr.CreatedUtc.Month);
            Assert.AreEqual(15, tr.CreatedUtc.Day);
            Assert.AreEqual(10, tr.CreatedUtc.Hour);
            Assert.AreEqual(35, tr.CreatedUtc.Minute);
            Assert.AreEqual(2, tr.CreatedUtc.Second);
            Assert.AreEqual(22, tr.RowIndex);
            Assert.AreEqual(140.87439938m, tr.BuyAmount);
            Assert.AreEqual(Assets.XRP, tr.BuyAsset);
            Assert.AreEqual(999.99999998m, tr.SellAmount);
            Assert.AreEqual(Assets.CZK, tr.SellAsset);
        }

        private void AssertTransaction_Other_8(Transaction tr)
        {
            Assert.AreEqual(TransactionSource.Coinmate, tr.TransactionSource);
            Assert.AreEqual(2021, tr.CreatedUtc.Year);
            Assert.AreEqual(2, tr.CreatedUtc.Month);
            Assert.AreEqual(14, tr.CreatedUtc.Day);
            Assert.AreEqual(15, tr.CreatedUtc.Hour);
            Assert.AreEqual(36, tr.CreatedUtc.Minute);
            Assert.AreEqual(40, tr.CreatedUtc.Second);
            Assert.AreEqual(4, tr.RowIndex);
            Assert.AreEqual(9879.66363354m, tr.BuyAmount);
            Assert.AreEqual(Assets.CZK, tr.BuyAsset);
            Assert.AreEqual(0.00964467m, tr.SellAmount);
            Assert.AreEqual(Assets.BTC, tr.SellAsset);
        }

        private void AssertTransaction_Other_9(Transaction tr)
        {
            Assert.AreEqual(TransactionSource.Coinmate, tr.TransactionSource);
            Assert.AreEqual(2021, tr.CreatedUtc.Year);
            Assert.AreEqual(2, tr.CreatedUtc.Month);
            Assert.AreEqual(14, tr.CreatedUtc.Day);
            Assert.AreEqual(15, tr.CreatedUtc.Hour);
            Assert.AreEqual(38, tr.CreatedUtc.Minute);
            Assert.AreEqual(52, tr.CreatedUtc.Second);
            Assert.AreEqual(3, tr.RowIndex);
            Assert.AreEqual(0.00954268m, tr.BuyAmount);
            Assert.AreEqual(Assets.BTC, tr.BuyAsset);
            Assert.AreEqual(9879.66982458m, tr.SellAmount);
            Assert.AreEqual(Assets.CZK, tr.SellAsset);
        }

        private void AssertTransaction_Other_10(Transaction tr)
        {
            Assert.AreEqual(TransactionSource.Coinmate, tr.TransactionSource);
            Assert.AreEqual(2021, tr.CreatedUtc.Year);
            Assert.AreEqual(2, tr.CreatedUtc.Month);
            Assert.AreEqual(14, tr.CreatedUtc.Day);
            Assert.AreEqual(15, tr.CreatedUtc.Hour);
            Assert.AreEqual(39, tr.CreatedUtc.Minute);
            Assert.AreEqual(12, tr.CreatedUtc.Second);
            Assert.AreEqual(2, tr.RowIndex);
            Assert.AreEqual(379.75787621m, tr.BuyAmount);
            Assert.AreEqual(Assets.EUR, tr.BuyAsset);
            Assert.AreEqual(0.00954268m, tr.SellAmount);
            Assert.AreEqual(Assets.BTC, tr.SellAsset);
        }
    }
}