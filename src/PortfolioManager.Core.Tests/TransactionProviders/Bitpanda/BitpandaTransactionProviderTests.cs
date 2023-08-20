namespace PortfolioManager.Core.Tests.TransactionProviders.Bitpanda
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using NUnit.Framework;
    using PortfolioManager.Core.TransactionProviders.Bitpanda;

    public class BitpandaTransactionProviderTests
    {
        [Test]
        public void ParseAllFiles()
        {
            var currentDirectory = TestContext.CurrentContext.TestDirectory;
            var resourcesDirectory = Path.Combine(currentDirectory, "Resources", "Bitpanda");

            var settings = new CoreSettings()
            {
                BitpandaProviderRootPath = resourcesDirectory
            };
            var btp = new BitpandaTransactionProvider(settings);

            var allTransactions = btp.GetAllTransactions().ToList();
            var transactionsOf2021_5_18 = allTransactions
                                          .Where(tr =>
                                                     tr.CreatedUtc.Year == 2021 &&
                                                     tr.CreatedUtc.Month == 5 &&
                                                     tr.CreatedUtc.Day == 18)
                                          .OrderBy(tr => tr.CreatedUtc)
                                          .ThenBy(tr => tr.Order)
                                          .ToList();
            var otherTransactions = allTransactions.Where(tr => tr.CreatedUtc.Year >= 2019).OrderBy(tr => tr.CreatedUtc).ThenBy(tr => tr.Order).ToList();

            AssertTransactions_2021_5_18(transactionsOf2021_5_18);
        }

        private void AssertTransactions_2021_5_18(List<Transaction> transactions)
        {
            AssertTransaction_2021_5_18_0(transactions[0]);
            AssertTransaction_2021_5_18_1(transactions[1]);
            AssertTransaction_2021_5_18_2(transactions[2]);
            AssertTransaction_2021_5_18_3(transactions[3]);
        }

        private void AssertTransaction_2021_5_18_0(Transaction tr)
        {
            Assert.AreEqual(TransactionSource.Bitpanda, tr.TransactionSource);
            Assert.AreEqual(2021, tr.CreatedUtc.Year);
            Assert.AreEqual(5, tr.CreatedUtc.Month);
            Assert.AreEqual(18, tr.CreatedUtc.Day);
            Assert.AreEqual(19, tr.CreatedUtc.Hour);
            Assert.AreEqual(56, tr.CreatedUtc.Minute);
            Assert.AreEqual(31, tr.CreatedUtc.Second);
            Assert.AreEqual(72, tr.RowIndex);
            Assert.AreEqual(199.37m, tr.BuyAmount);
            Assert.AreEqual(Assets.EUR, tr.BuyAsset);
            Assert.AreEqual(7m, tr.SellAmount);
            Assert.AreEqual(Assets.UNI, tr.SellAsset);
        }

        private void AssertTransaction_2021_5_18_1(Transaction tr)
        {
            Assert.AreEqual(TransactionSource.Bitpanda, tr.TransactionSource);
            Assert.AreEqual(2021, tr.CreatedUtc.Year);
            Assert.AreEqual(5, tr.CreatedUtc.Month);
            Assert.AreEqual(18, tr.CreatedUtc.Day);
            Assert.AreEqual(19, tr.CreatedUtc.Hour);
            Assert.AreEqual(56, tr.CreatedUtc.Minute);
            Assert.AreEqual(31, tr.CreatedUtc.Second);
            Assert.AreEqual(73, tr.RowIndex);
            Assert.AreEqual(0.04920833m, tr.BuyAmount);
            Assert.AreEqual(Assets.MKR, tr.BuyAsset);
            Assert.AreEqual(199.37m, tr.SellAmount);
            Assert.AreEqual(Assets.EUR, tr.SellAsset);
        }

        private void AssertTransaction_2021_5_18_2(Transaction tr)
        {
            Assert.AreEqual(TransactionSource.Bitpanda, tr.TransactionSource);
            Assert.AreEqual(2021, tr.CreatedUtc.Year);
            Assert.AreEqual(5, tr.CreatedUtc.Month);
            Assert.AreEqual(18, tr.CreatedUtc.Day);
            Assert.AreEqual(19, tr.CreatedUtc.Hour);
            Assert.AreEqual(56, tr.CreatedUtc.Minute);
            Assert.AreEqual(57, tr.CreatedUtc.Second);
            Assert.AreEqual(74, tr.RowIndex);
            Assert.AreEqual(199.37m, tr.BuyAmount);
            Assert.AreEqual(Assets.EUR, tr.BuyAsset);
            Assert.AreEqual(7m, tr.SellAmount);
            Assert.AreEqual(Assets.UNI, tr.SellAsset);
        }

        private void AssertTransaction_2021_5_18_3(Transaction tr)
        {
            Assert.AreEqual(TransactionSource.Bitpanda, tr.TransactionSource);
            Assert.AreEqual(2021, tr.CreatedUtc.Year);
            Assert.AreEqual(5, tr.CreatedUtc.Month);
            Assert.AreEqual(18, tr.CreatedUtc.Day);
            Assert.AreEqual(19, tr.CreatedUtc.Hour);
            Assert.AreEqual(56, tr.CreatedUtc.Minute);
            Assert.AreEqual(57, tr.CreatedUtc.Second);
            Assert.AreEqual(75, tr.RowIndex);
            Assert.AreEqual(0.00328980m, tr.BuyAmount);
            Assert.AreEqual(Assets.YFI, tr.BuyAsset);
            Assert.AreEqual(199.37m, tr.SellAmount);
            Assert.AreEqual(Assets.EUR, tr.SellAsset);
        }
    }
}