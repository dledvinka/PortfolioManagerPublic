namespace PortfolioManager.Core.Tests.TransactionProviders.Anycoin
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using FluentAssertions;
    using NUnit.Framework;
    using PortfolioManager.Core.TransactionProviders.Anycoin;

    public class AnycoinTransactionProviderTests
    {
        [Test]
        public void ParseAllFiles()
        {
            var currentDirectory = TestContext.CurrentContext.TestDirectory;
            var resourcesDirectory = Path.Combine(currentDirectory, "Resources", "Anycoin");

            var settings = new CoreSettings()
            {
                AnycoinProviderRootPath = resourcesDirectory
            };
            var btp = new AnycoinTransactionProvider(settings);

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

            var transactionsOf2023 = allTransactions
                                     .Where(tr => tr.CreatedUtc.Year == 2023)
                                     .OrderBy(tr => tr.CreatedUtc)
                                     .ThenBy(tr => tr.Order)
                                     .ToList();

			AssertTransactions_2020(transactionsOf2020);
            AssertTransactions_2021(transactionsOf2021);
            AssertTransactions_2022(transactionsOf2022);
            AssertTransactions_2023(transactionsOf2023);
        }

        private void AssertTransactions_2020(List<Transaction> transactions)
        {
            transactions.Should().BeEmpty();
        }

        private void AssertTransactions_2021(List<Transaction> transactions)
        {
            transactions.Should().BeEmpty();
        }

        private void AssertTransactions_2022(List<Transaction> transactions)
        {
            transactions.Should().HaveCount(51);

            AssertTransaction_2022_0(transactions[0]);
            AssertTransaction_2022_23(transactions[23]);
        }

        private void AssertTransactions_2023(List<Transaction> transactions)
        {
	        transactions.Should().HaveCount(4);

	        AssertTransaction_2023_0(transactions[0]);
	        AssertTransaction_2023_1(transactions[1]);
	        AssertTransaction_2023_2(transactions[2]);
	        AssertTransaction_2023_3(transactions[3]);
        }

		private void AssertTransaction_2022_0(Transaction tr)
        {
            tr.TransactionSource.Should().Be(TransactionSource.Anycoin);

            tr.CreatedUtc.Year.Should().Be(2022);
            tr.CreatedUtc.Month.Should().Be(1);
            tr.CreatedUtc.Day.Should().Be(24);
            tr.CreatedUtc.Hour.Should().Be(9);
            tr.CreatedUtc.Minute.Should().Be(7);
            tr.CreatedUtc.Second.Should().Be(43);

            tr.BuyAmount.Should().Be(0.0032814m);
            tr.BuyAsset.Should().Be(Assets.BTC);

            tr.SellAmount.Should().BeApproximately(tr.BuyAmount * 761869.9335649419m, 1m);
            tr.SellAsset.Should().Be(Assets.CZK);
        }

        private void AssertTransaction_2022_23(Transaction tr)
        {
            tr.TransactionSource.Should().Be(TransactionSource.Anycoin);

            tr.CreatedUtc.Year.Should().Be(2022);
            tr.CreatedUtc.Month.Should().Be(6);
            tr.CreatedUtc.Day.Should().Be(22);
            tr.CreatedUtc.Hour.Should().Be(9);
            tr.CreatedUtc.Minute.Should().Be(6);
            tr.CreatedUtc.Second.Should().Be(14);

            tr.BuyAmount.Should().Be(0.00520541m);
            tr.BuyAsset.Should().Be(Assets.BTC);

            tr.SellAmount.Should().BeApproximately(tr.BuyAmount * 480269.5657018371m, 1m);
            tr.SellAsset.Should().Be(Assets.CZK);
        }

        private void AssertTransaction_2023_0(Transaction tr)
        {
	        tr.TransactionSource.Should().Be(TransactionSource.Anycoin);

	        tr.BuyAmount.Should().Be(0.00129982m);
	        tr.BuyAsset.Should().Be(Assets.BTC);

	        tr.SellAmount.Should().BeApproximately(tr.BuyAmount * 384668.6464279669m, 1m);
	        tr.SellAsset.Should().Be(Assets.CZK);
        }

        private void AssertTransaction_2023_1(Transaction tr)
        {
	        tr.TransactionSource.Should().Be(TransactionSource.Anycoin);

	        tr.BuyAmount.Should().BeApproximately(tr.SellAmount * 455278.1428571428m, 1m);
	        tr.BuyAsset.Should().Be(Assets.CZK);

	        tr.SellAmount.Should().Be(0.07m);
			tr.SellAsset.Should().Be(Assets.BTC);
        }

        private void AssertTransaction_2023_2(Transaction tr)
        {
	        tr.TransactionSource.Should().Be(TransactionSource.Anycoin);

	        tr.BuyAmount.Should().BeApproximately(tr.SellAmount * 457864.189611442m, 1m);
	        tr.BuyAsset.Should().Be(Assets.CZK);

	        tr.SellAmount.Should().Be(0.10157868m);
	        tr.SellAsset.Should().Be(Assets.BTC);
        }

        private void AssertTransaction_2023_3(Transaction tr)
        {
	        tr.TransactionSource.Should().Be(TransactionSource.Anycoin);

	        tr.BuyAmount.Should().BeApproximately(tr.SellAmount * 493500m, 1m);
	        tr.BuyAsset.Should().Be(Assets.CZK);

	        tr.SellAmount.Should().Be(0.02m);
	        tr.SellAsset.Should().Be(Assets.BTC);
        }
	}
}