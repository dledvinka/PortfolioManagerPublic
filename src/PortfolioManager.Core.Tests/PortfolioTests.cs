namespace PortfolioManager.Core.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using NUnit.Framework;
    using PortfolioManager.Core.Services;
    using PortfolioManager.Core.Tests.Mocks;

    public class PortfolioTests
    {
        private static readonly decimal _defaultConversionRate = 1.0m;
        private readonly IAssetConversionService _defaultConversionService = new AssetConversionServiceMock(_defaultConversionRate);

        [Test]
        public async Task Portfolio_ZeroBalanceWithoutTransaction()
        {
            var transactions = new List<Transaction>();

            var portfolio = new Portfolio(transactions, _defaultConversionService);
            var balances = (await portfolio.GetCurrentBalancesAsync()).ToList();

            Assert.AreEqual(Assets.All.Count, balances.Count());
            Assert.IsTrue(balances.All(b => b.Amount == 0.0m));
        }

        [Test]
        public async Task Portfolio_ZeroBalanceWithNegatingTransactions()
        {
            var transactions = new List<Transaction>();

            transactions.Add(new Transaction()
            {
                BuyAmount = 0.1m,
                BuyAsset = Assets.BTC,
                SellAmount = 1000.0m,
                SellAsset = Assets.CZK
            });

            transactions.Add(new Transaction()
            {
                BuyAmount = 1000.0m,
                BuyAsset = Assets.CZK,
                SellAmount = 0.1m,
                SellAsset = Assets.BTC
            });

            var portfolio = new Portfolio(transactions, _defaultConversionService);
            var balances = (await portfolio.GetCurrentBalancesAsync()).ToList();

            Assert.AreEqual(Assets.All.Count, balances.Count());
            Assert.IsTrue(balances.All(b => b.Amount == 0.0m));
        }

        [Test]
        public async Task Portfolio_UsesCurrencyConversionService()
        {
            var transactions = new List<Transaction>();
            var conversionRate = 2.0m;
            var ccs = new AssetConversionServiceMock(conversionRate);

            transactions.Add(new Transaction()
            {
                BuyAmount = 0.1m,
                BuyAsset = Assets.BTC,
                SellAmount = 1000.0m,
                SellAsset = Assets.CZK
            });

            transactions.Add(new Transaction()
            {
                BuyAmount = 2m,
                BuyAsset = Assets.ETH,
                SellAmount = 400.0m,
                SellAsset = Assets.EUR
            });

            var portfolio = new Portfolio(transactions, ccs);

            var czkBalance = await portfolio.GetCurrentBalanceAsync(Assets.CZK);
            Assert.AreEqual(-1000, czkBalance.Amount);
            Assert.AreEqual(Assets.CZK, czkBalance.Asset);

            var czkBalanceInEur = await portfolio.GetCurrentBalanceAsync(Assets.CZK, Assets.EUR);
            Assert.AreEqual(-1000 * conversionRate, czkBalanceInEur.Amount);
            Assert.AreEqual(Assets.EUR, czkBalanceInEur.Asset);

            var eurBalance = await portfolio.GetCurrentBalanceAsync(Assets.EUR);
            Assert.AreEqual(-400, eurBalance.Amount);
            Assert.AreEqual(Assets.EUR, eurBalance.Asset);

            var eurBalanceInCzk = await portfolio.GetCurrentBalanceAsync(Assets.EUR, Assets.CZK);
            Assert.AreEqual(-400 * conversionRate, eurBalanceInCzk.Amount);
            Assert.AreEqual(Assets.CZK, eurBalanceInCzk.Asset);
        }

		[Test]
		public async Task Portfolio_CurrentBalanceAveragePrice()
		{
			var transactions = new List<Transaction>();

			transactions.Add(new Transaction()
			{
				BuyAmount = 0.1m,
				BuyAsset = Assets.BTC,
				SellAmount = 1000.0m,
				SellAsset = Assets.CZK
			});

			transactions.Add(new Transaction()
			{
				BuyAmount = 0.1m,
				BuyAsset = Assets.BTC,
				SellAmount = 2000.0m,
				SellAsset = Assets.CZK
			});

			transactions.Add(new Transaction()
			{
				BuyAmount = 0.25m,
				BuyAsset = Assets.BTC,
				SellAmount = 3000.0m,
				SellAsset = Assets.CZK
			});

			transactions.Add(new Transaction()
			{
				BuyAmount = 2m,
				BuyAsset = Assets.ETH,
				SellAmount = 0.15m,
				SellAsset = Assets.BTC
			});

			transactions.Add(new Transaction()
			{
				BuyAmount = 30000.0m,
				BuyAsset = Assets.CZK,
				SellAmount = 0.04m,
				SellAsset = Assets.BTC
			});

			var portfolio = new Portfolio(transactions, _defaultConversionService);
            
			var btcBalance = await portfolio.GetCurrentBalanceAsync(Assets.BTC);
			Assert.AreEqual(0.26m, btcBalance.Amount);
			Assert.AreEqual(Assets.BTC, btcBalance.Asset);

			var btcBuyAveragePriceInCzk = await portfolio.GetCurrentBalanceWeightedAverageBuyPriceAsync(Assets.BTC, Assets.CZK);

			btcBuyAveragePriceInCzk.Should().BeApproximately(12307.69m, 0.01m);
		}

		[Test]
        public async Task Portfolio_SomeTransactions()
        {
            var transactions = new List<Transaction>();

            transactions.Add(new Transaction()
            {
                BuyAmount = 0.1m,
                BuyAsset = Assets.BTC,
                SellAmount = 1000.0m,
                SellAsset = Assets.CZK
            });

            transactions.Add(new Transaction()
            {
                BuyAmount = 0.1m,
                BuyAsset = Assets.BTC,
                SellAmount = 1000.0m,
                SellAsset = Assets.CZK
            });

            transactions.Add(new Transaction()
            {
                BuyAmount = 2m,
                BuyAsset = Assets.ETH,
                SellAmount = 400.0m,
                SellAsset = Assets.EUR
            });

            transactions.Add(new Transaction()
            {
                BuyAmount = 2m,
                BuyAsset = Assets.ETH,
                SellAmount = 0.15m,
                SellAsset = Assets.BTC
            });

            transactions.Add(new Transaction()
            {
                BuyAmount = 30000.0m,
                BuyAsset = Assets.CZK,
                SellAmount = 3.0m,
                SellAsset = Assets.ETH
            });

            var portfolio = new Portfolio(transactions, _defaultConversionService);
            var balances = (await portfolio.GetCurrentBalancesAsync()).ToList();

            Assert.AreEqual(Assets.All.Count, balances.Count());

            var czkBalance = await portfolio.GetCurrentBalanceAsync(Assets.CZK);
            Assert.AreEqual(28000.0, czkBalance.Amount);
            Assert.AreEqual(Assets.CZK, czkBalance.Asset);

            var eurBalance = await portfolio.GetCurrentBalanceAsync(Assets.EUR);
            Assert.AreEqual(-400.0, eurBalance.Amount);
            Assert.AreEqual(Assets.EUR, eurBalance.Asset);

            var btcBalance = await portfolio.GetCurrentBalanceAsync(Assets.BTC);
            Assert.AreEqual(0.05m, btcBalance.Amount);
            Assert.AreEqual(Assets.BTC, btcBalance.Asset);

            var ethBalance = await portfolio.GetCurrentBalanceAsync(Assets.ETH);
            Assert.AreEqual(1.0m, ethBalance.Amount);
            Assert.AreEqual(Assets.ETH, ethBalance.Asset);

            var nonZeroBalances = new[]
            {
                Assets.CZK, Assets.EUR, Assets.BTC, Assets.ETH
            };

            Assert.IsTrue(balances.Where(b => !nonZeroBalances.Contains(b.Asset)).All(b => b.Amount == 0.0m));
        }
    }
}