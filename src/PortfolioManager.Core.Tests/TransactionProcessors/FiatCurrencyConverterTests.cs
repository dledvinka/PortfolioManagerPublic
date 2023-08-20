namespace PortfolioManager.Core.Tests.TransactionProcessors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using CoinGecko.Clients;
    using Newtonsoft.Json;
    using NUnit.Framework;
    using PortfolioManager.Core.Cache;
    using PortfolioManager.Core.Services;
    using PortfolioManager.Core.Tests.Mocks;
    using PortfolioManager.Core.TransactionProcessors;

    public class FiatCurrencyConverterTests
    {
        private readonly ICoinGeckoCache _cache = new CoinGeckoCacheMock();
        private readonly CoinsClient _coinsClient = new CoinsClient(new HttpClient(), new JsonSerializerSettings());

        [Test]
        public async Task ConvertsEurToCzk()
        {
            var acs = new AssetConversionService(_coinsClient, _cache);
            var targetFiatAsset = Assets.CZK;

            var transactions = new List<Transaction>()
            {
                new()
                {
                    CreatedUtc = new DateTime(2017, 1, 1).Date,
                    TransactionSource = TransactionSource.BitpandaPro,
                    Order = 5,
                    RowIndex = 10,
                    BuyAmount = 1000,
                    BuyAsset = Assets.EUR,
                    SellAmount = 0.1m,
                    SellAsset = Assets.BTC
                },
                new()
                {
                    CreatedUtc = new DateTime(2017, 1, 1).Date,
                    TransactionSource = TransactionSource.BitpandaPro,
                    Order = 5,
                    RowIndex = 10,
                    BuyAmount = 0.1m,
                    BuyAsset = Assets.BTC,
                    SellAmount = 1000m,
                    SellAsset = Assets.EUR
                }
            };

            transactions[0].BuyAmount = 1000m;

            var fcc = new FiatCurrencyConverter(acs, Assets.CZK);

            var processedTransactions = (await fcc.ProcessAsync(transactions)).ToList();

            Assert.IsTrue(processedTransactions.Count == 2);

            var tr0 = processedTransactions[0];
            Assert.AreEqual(1000m * 26.29m, tr0.BuyAmount);
            Assert.AreEqual(targetFiatAsset, tr0.BuyAsset);

            var tr1 = processedTransactions[1];
            Assert.AreEqual(1000m * 26.29m, tr1.SellAmount);
            Assert.AreEqual(targetFiatAsset, tr1.SellAsset);
        }

        [Test]
        public async Task ConvertsEurToCzk_DifferentYears()
        {
            var acs = new AssetConversionService(_coinsClient, _cache);
            var targetFiatAsset = Assets.CZK;

            var transactions = new List<Transaction>()
            {
                new()
                {
                    CreatedUtc = new DateTime(2017, 1, 1).Date,
                    TransactionSource = TransactionSource.BitpandaPro,
                    Order = 5,
                    RowIndex = 10,
                    BuyAmount = 1000,
                    BuyAsset = Assets.EUR,
                    SellAmount = 0.1m,
                    SellAsset = Assets.BTC
                },
                new()
                {
                    CreatedUtc = new DateTime(2018, 1, 1).Date,
                    TransactionSource = TransactionSource.BitpandaPro,
                    Order = 5,
                    RowIndex = 10,
                    BuyAmount = 1000,
                    BuyAsset = Assets.EUR,
                    SellAmount = 0.1m,
                    SellAsset = Assets.BTC
                },
                new()
                {
                    CreatedUtc = new DateTime(2019, 1, 1).Date,
                    TransactionSource = TransactionSource.BitpandaPro,
                    Order = 5,
                    RowIndex = 10,
                    BuyAmount = 1000,
                    BuyAsset = Assets.EUR,
                    SellAmount = 0.1m,
                    SellAsset = Assets.BTC
                },
                new()
                {
                    CreatedUtc = new DateTime(2020, 1, 1).Date,
                    TransactionSource = TransactionSource.BitpandaPro,
                    Order = 5,
                    RowIndex = 10,
                    BuyAmount = 1000,
                    BuyAsset = Assets.EUR,
                    SellAmount = 0.1m,
                    SellAsset = Assets.BTC
                },
                new()
                {
                    CreatedUtc = new DateTime(2021, 1, 1).Date,
                    TransactionSource = TransactionSource.BitpandaPro,
                    Order = 5,
                    RowIndex = 10,
                    BuyAmount = 1000,
                    BuyAsset = Assets.EUR,
                    SellAmount = 0.1m,
                    SellAsset = Assets.BTC
                },
                new()
                {
                    CreatedUtc = new DateTime(2022, 1, 1).Date,
                    TransactionSource = TransactionSource.BitpandaPro,
                    Order = 5,
                    RowIndex = 10,
                    BuyAmount = 1000,
                    BuyAsset = Assets.EUR,
                    SellAmount = 0.1m,
                    SellAsset = Assets.BTC
                }
            };

            var fcc = new FiatCurrencyConverter(acs, targetFiatAsset);

            var processedTransactions = (await fcc.ProcessAsync(transactions)).ToList();

            Assert.IsTrue(processedTransactions.Count == 6);

            var tr = processedTransactions[0];
            Assert.AreEqual(1000m * 26.29m, tr.BuyAmount);
            Assert.AreEqual(targetFiatAsset, tr.BuyAsset);

            tr = processedTransactions[1];
            Assert.AreEqual(1000m * 25.68m, tr.BuyAmount);
            Assert.AreEqual(targetFiatAsset, tr.BuyAsset);

            tr = processedTransactions[2];
            Assert.AreEqual(1000m * 25.66m, tr.BuyAmount);
            Assert.AreEqual(targetFiatAsset, tr.BuyAsset);

            tr = processedTransactions[3];
            Assert.AreEqual(1000m * 26.50m, tr.BuyAmount);
            Assert.AreEqual(targetFiatAsset, tr.BuyAsset);

            tr = processedTransactions[4];
            Assert.AreEqual(1000m * 25.65m, tr.BuyAmount);
            Assert.AreEqual(targetFiatAsset, tr.BuyAsset);

            tr = processedTransactions[5];
            Assert.AreEqual(1000m * 25.65m, tr.BuyAmount);
            Assert.AreEqual(targetFiatAsset, tr.BuyAsset);
        }

        [Test]
        public async Task ConvertsCzkToEur()
        {
            var acs = new AssetConversionService(_coinsClient, _cache);
            var targetFiatAsset = Assets.EUR;

            var transactions = new List<Transaction>()
            {
                new()
                {
                    CreatedUtc = new DateTime(2017, 1, 1).Date,
                    TransactionSource = TransactionSource.BitpandaPro,
                    Order = 5,
                    RowIndex = 10,
                    BuyAmount = 1000,
                    BuyAsset = Assets.CZK,
                    SellAmount = 0.1m,
                    SellAsset = Assets.BTC
                },
                new()
                {
                    CreatedUtc = new DateTime(2017, 1, 1).Date,
                    TransactionSource = TransactionSource.BitpandaPro,
                    Order = 5,
                    RowIndex = 10,
                    BuyAmount = 0.1m,
                    BuyAsset = Assets.BTC,
                    SellAmount = 1000m,
                    SellAsset = Assets.CZK
                }
            };

            var fcc = new FiatCurrencyConverter(acs, Assets.EUR);

            var processedTransactions = (await fcc.ProcessAsync(transactions)).ToList();

            Assert.IsTrue(processedTransactions.Count == 2);

            var tr0 = processedTransactions[0];
            Assert.AreEqual(Math.Round(1000m / 26.29m, 5), Math.Round(tr0.BuyAmount, 5));
            Assert.AreEqual(targetFiatAsset, tr0.BuyAsset);

            var tr1 = processedTransactions[1];
            Assert.AreEqual(Math.Round(1000m / 26.29m, 5), Math.Round(tr1.SellAmount, 5));
            Assert.AreEqual(targetFiatAsset, tr1.SellAsset);
        }

        [Test]
        public async Task LeavesCzk()
        {
            var acs = new AssetConversionService(_coinsClient, _cache);
            var targetFiatAsset = Assets.CZK;

            var transactions = new List<Transaction>()
            {
                new()
                {
                    CreatedUtc = new DateTime(2017, 1, 1).Date,
                    TransactionSource = TransactionSource.BitpandaPro,
                    Order = 5,
                    RowIndex = 10,
                    BuyAmount = 1000,
                    BuyAsset = Assets.CZK,
                    SellAmount = 0.1m,
                    SellAsset = Assets.BTC
                },
                new()
                {
                    CreatedUtc = new DateTime(2017, 1, 1).Date,
                    TransactionSource = TransactionSource.BitpandaPro,
                    Order = 5,
                    RowIndex = 10,
                    BuyAmount = 0.1m,
                    BuyAsset = Assets.BTC,
                    SellAmount = 1000m,
                    SellAsset = Assets.CZK
                }
            };

            var fcc = new FiatCurrencyConverter(acs, Assets.CZK);

            var processedTransactions = (await fcc.ProcessAsync(transactions)).ToList();

            Assert.IsTrue(processedTransactions.Count == 2);

            var tr0 = processedTransactions[0];
            Assert.AreEqual(tr0.BuyAmount, tr0.BuyAmount);
            Assert.AreEqual(targetFiatAsset, tr0.BuyAsset);

            var tr1 = processedTransactions[1];
            Assert.AreEqual(tr1.SellAmount, tr1.SellAmount);
            Assert.AreEqual(targetFiatAsset, tr1.SellAsset);
        }
    }
}