namespace PortfolioManager.Core.Tests.TaxReport
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using PortfolioManager.Core.Services;
    using PortfolioManager.Core.Tax;

    public partial class TaxReportGeneratorTests
    {
        [Test]
        public async Task Case03_FIFO()
        {
            var acs = new AssetConversionService(_coinsClient, _cache);
            var targetFiatAsset = Assets.CZK;

            var transactions = new List<Transaction>()
            {
                new()
                {
                    CreatedUtc = new DateTime(2017, 1, 1).Date,
                    TransactionSource = TransactionSource.BitpandaPro,
                    BuyAmount = 1m,
                    BuyAsset = Assets.BTC,
                    SellAmount = 100000,
                    SellAsset = Assets.CZK
                },
                new()
                {
                    CreatedUtc = new DateTime(2017, 12, 31).Date,
                    TransactionSource = TransactionSource.BitpandaPro,
                    BuyAmount = 10000m,
                    BuyAsset = Assets.CZK,
                    SellAmount = 0.1m,
                    SellAsset = Assets.BTC
                },
                new()
                {
                    CreatedUtc = new DateTime(2018, 12, 31).Date,
                    TransactionSource = TransactionSource.BitpandaPro,
                    BuyAmount = 20000m,
                    BuyAsset = Assets.CZK,
                    SellAmount = 0.15m,
                    SellAsset = Assets.BTC
                },
                new()
                {
                    CreatedUtc = new DateTime(2019, 12, 31).Date,
                    TransactionSource = TransactionSource.BitpandaPro,
                    BuyAmount = 30000m,
                    BuyAsset = Assets.CZK,
                    SellAmount = 0.2m,
                    SellAsset = Assets.BTC
                },
                new()
                {
                    CreatedUtc = new DateTime(2020, 12, 31).Date,
                    TransactionSource = TransactionSource.BitpandaPro,
                    BuyAmount = 40000m,
                    BuyAsset = Assets.CZK,
                    SellAmount = 0.25m,
                    SellAsset = Assets.BTC
                },
                new()
                {
                    CreatedUtc = new DateTime(2021, 12, 31).Date,
                    TransactionSource = TransactionSource.BitpandaPro,
                    BuyAmount = 50000m,
                    BuyAsset = Assets.CZK,
                    SellAmount = 0.3m,
                    SellAsset = Assets.BTC
                }
            };


            var trg = new TaxReportGenerator(acs, transactions);
            var tr = await trg.GetTaxReportAsync(targetFiatAsset);

            // 2017
            var btcSubReport2017 = tr.GetSubReport(Assets.BTC, 2017);
            Assert.AreEqual(10000m, btcSubReport2017.RealizedProfit);
            Assert.AreEqual(10000m, btcSubReport2017.TotalExpenditure);
            Assert.AreEqual(0m, btcSubReport2017.TaxObligation);
            Assert.AreEqual(0.1m, btcSubReport2017.TotalSold);

            // 2018
            var btcSubReport2018 = tr.GetSubReport(Assets.BTC, 2018);
            Assert.AreEqual(20000m, btcSubReport2018.RealizedProfit);
            Assert.AreEqual(15000m, btcSubReport2018.TotalExpenditure);
            Assert.AreEqual(5000m, btcSubReport2018.TaxObligation);
            Assert.AreEqual(0.15m, btcSubReport2018.TotalSold);

            // 2019
            var btcSubReport2019 = tr.GetSubReport(Assets.BTC, 2019);
            Assert.AreEqual(30000m, btcSubReport2019.RealizedProfit);
            Assert.AreEqual(20000m, btcSubReport2019.TotalExpenditure);
            Assert.AreEqual(10000m, btcSubReport2019.TaxObligation);
            Assert.AreEqual(0.2m, btcSubReport2019.TotalSold);

            // 2020
            var btcSubReport2020 = tr.GetSubReport(Assets.BTC, 2020);
            Assert.AreEqual(40000m, btcSubReport2020.RealizedProfit);
            Assert.AreEqual(25000m, btcSubReport2020.TotalExpenditure);
            Assert.AreEqual(15000m, btcSubReport2020.TaxObligation);
            Assert.AreEqual(0.25m, btcSubReport2020.TotalSold);

            // 2021
            var btcSubReport2021 = tr.GetSubReport(Assets.BTC, 2021);
            Assert.AreEqual(50000m, btcSubReport2021.RealizedProfit);
            Assert.AreEqual(30000m, btcSubReport2021.TotalExpenditure);
            Assert.AreEqual(20000m, btcSubReport2021.TaxObligation);
            Assert.AreEqual(0.3m, btcSubReport2021.TotalSold);
        }
    }
}