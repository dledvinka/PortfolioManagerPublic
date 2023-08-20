namespace PortfolioManager.Core.Tests.TransactionProviders.BitpandaPro
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using NUnit.Framework;
    using PortfolioManager.Core.TransactionProviders.BitpandaPro;

    public class BitpandaProTransactionProviderTests
    {
        [Test]
        public void ParseAllFiles()
        {
            var currentDirectory = TestContext.CurrentContext.TestDirectory;
            var resourcesDirectory = Path.Combine(currentDirectory, "Resources", "BitpandaPro");

            var settings = new CoreSettings()
            {
                BitpandaProProviderRootPath = resourcesDirectory
            };
            var btp = new BitpandaProTransactionProvider(settings);

            var allTransactions = btp.GetAllTransactions().ToList();

            var transactionsOf2020 = allTransactions
                                     .Where(tr => tr.CreatedUtc.Year == 2020)
                                     .OrderBy(tr => tr.CreatedUtc)
                                     .ThenBy(tr => tr.Order)
                                     .ToList();

            var transactionsOf2021 = allTransactions
                                     .Where(tr => tr.CreatedUtc.Year == 2021)
                                     .OrderBy(tr => tr.CreatedUtc)
                                     .ThenBy(tr => tr.Order)
                                     .ToList();

            var transactionsOf2022 = allTransactions
                                     .Where(tr => tr.CreatedUtc.Year == 2022)
                                     .OrderBy(tr => tr.CreatedUtc)
                                     .ThenBy(tr => tr.Order)
                                     .ToList();


            AssertTransactions_2020(transactionsOf2020);
            AssertTransactions_2021(transactionsOf2021);
            AssertTransactions_2022(transactionsOf2022);
        }

        private void AssertTransactions_2020(List<Transaction> transactions)
        {
            Assert.IsTrue(transactions.Count == 1);
            AssertTransaction_2020_0(transactions[0]);
        }

        private void AssertTransactions_2021(List<Transaction> transactions)
        {
            Assert.IsTrue(transactions.Count == 48);
            AssertTransaction_2021_0(transactions[0]);
            AssertTransaction_2021_4(transactions[4]);
            AssertTransaction_2021_47(transactions[47]);
        }

        private void AssertTransactions_2022(List<Transaction> transactions)
        {
            Assert.IsTrue(transactions.Count == 10);
            AssertTransaction_2022_9(transactions[9]);
        }

        private void AssertTransaction_2020_0(Transaction tr)
        {
            Assert.AreEqual(TransactionSource.BitpandaPro, tr.TransactionSource);
            Assert.AreEqual(2020, tr.CreatedUtc.Year);
            Assert.AreEqual(12, tr.CreatedUtc.Month);
            Assert.AreEqual(11, tr.CreatedUtc.Day);
            Assert.AreEqual(2, tr.CreatedUtc.Hour);
            Assert.AreEqual(31, tr.CreatedUtc.Minute);
            Assert.AreEqual(42, tr.CreatedUtc.Second);
            Assert.AreEqual(0.01361m, tr.BuyAmount);
            Assert.AreEqual(Assets.BTC, tr.BuyAsset);
            Assert.AreEqual(0.01361m * 14700.0m, tr.SellAmount);
            Assert.AreEqual(Assets.EUR, tr.SellAsset);
        }

        private void AssertTransaction_2021_0(Transaction tr)
        {
            Assert.AreEqual(TransactionSource.BitpandaPro, tr.TransactionSource);
            Assert.AreEqual(2021, tr.CreatedUtc.Year);
            Assert.AreEqual(2, tr.CreatedUtc.Month);
            Assert.AreEqual(14, tr.CreatedUtc.Day);
            Assert.AreEqual(19, tr.CreatedUtc.Hour);
            Assert.AreEqual(20, tr.CreatedUtc.Minute);
            Assert.AreEqual(07, tr.CreatedUtc.Second);
            Assert.AreEqual(0.01362m * 40400.0m, tr.BuyAmount);
            Assert.AreEqual(Assets.EUR, tr.BuyAsset);
            Assert.AreEqual(0.01362m, tr.SellAmount);
            Assert.AreEqual(Assets.BTC, tr.SellAsset);
        }

        private void AssertTransaction_2021_4(Transaction tr)
        {
            Assert.AreEqual(TransactionSource.BitpandaPro, tr.TransactionSource);
            Assert.AreEqual(2021, tr.CreatedUtc.Year);
            Assert.AreEqual(2, tr.CreatedUtc.Month);
            Assert.AreEqual(22, tr.CreatedUtc.Day);
            Assert.AreEqual(12, tr.CreatedUtc.Hour);
            Assert.AreEqual(1, tr.CreatedUtc.Minute);
            Assert.AreEqual(32, tr.CreatedUtc.Second);
            Assert.AreEqual(1.002m, tr.BuyAmount);
            Assert.AreEqual(Assets.ETH, tr.BuyAsset);
            Assert.AreEqual(1.002m * 1503.06m, tr.SellAmount);
            Assert.AreEqual(Assets.EUR, tr.SellAsset);
        }

        private void AssertTransaction_2021_47(Transaction tr)
        {
            Assert.AreEqual(TransactionSource.BitpandaPro, tr.TransactionSource);
            Assert.AreEqual(2021, tr.CreatedUtc.Year);
            Assert.AreEqual(12, tr.CreatedUtc.Month);
            Assert.AreEqual(20, tr.CreatedUtc.Day);
            Assert.AreEqual(16, tr.CreatedUtc.Hour);
            Assert.AreEqual(29, tr.CreatedUtc.Minute);
            Assert.AreEqual(54, tr.CreatedUtc.Second);
            Assert.AreEqual(0.0147m, tr.BuyAmount);
            Assert.AreEqual(Assets.ETH, tr.BuyAsset);
            Assert.AreEqual(0.0147m * 3389.88m, tr.SellAmount);
            Assert.AreEqual(Assets.EUR, tr.SellAsset);
        }

        private void AssertTransaction_2022_9(Transaction tr)
        {
            Assert.AreEqual(TransactionSource.BitpandaPro, tr.TransactionSource);
            Assert.AreEqual(2022, tr.CreatedUtc.Year);
            Assert.AreEqual(01, tr.CreatedUtc.Month);
            Assert.AreEqual(24, tr.CreatedUtc.Day);
            Assert.AreEqual(18, tr.CreatedUtc.Hour);
            Assert.AreEqual(24, tr.CreatedUtc.Minute);
            Assert.AreEqual(57, tr.CreatedUtc.Second);
            Assert.AreEqual(0.7m * 1984.41m, tr.BuyAmount);
            Assert.AreEqual(Assets.EUR, tr.BuyAsset);
            Assert.AreEqual(0.7m, tr.SellAmount);
            Assert.AreEqual(Assets.ETH, tr.SellAsset);
        }
    }
}