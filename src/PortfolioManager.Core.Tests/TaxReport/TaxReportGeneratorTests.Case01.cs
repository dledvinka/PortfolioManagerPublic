namespace PortfolioManager.Core.Tests.TaxReport;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using PortfolioManager.Core.Services;
using PortfolioManager.Core.Tax;

public partial class TaxReportGeneratorTests
{
    [Test]
    public async Task Case01_FIFO()
    {
        var acs = new AssetConversionService(_coinsClient, _cache);
        var targetFiatAsset = Assets.CZK;

        var transactions = new List<Transaction>()
        {
            new()
            {
                CreatedUtc = new DateTime(2018, 1, 1).Date,
                TransactionSource = TransactionSource.BitpandaPro,
                BuyAmount = 0.1m,
                BuyAsset = Assets.BTC,
                SellAmount = 10000,
                SellAsset = Assets.CZK
            },
            new()
            {
                CreatedUtc = new DateTime(2018, 7, 1).Date,
                TransactionSource = TransactionSource.BitpandaPro,
                BuyAmount = 20000m,
                BuyAsset = Assets.CZK,
                SellAmount = 0.05m,
                SellAsset = Assets.BTC
            },
            new()
            {
                CreatedUtc = new DateTime(2019, 1, 1).Date,
                TransactionSource = TransactionSource.BitpandaPro,
                BuyAmount = 0.1m,
                BuyAsset = Assets.BTC,
                SellAmount = 15000,
                SellAsset = Assets.CZK
            },
            new()
            {
                CreatedUtc = new DateTime(2019, 7, 1).Date,
                TransactionSource = TransactionSource.BitpandaPro,
                BuyAmount = 3000m,
                BuyAsset = Assets.CZK,
                SellAmount = 0.04m,
                SellAsset = Assets.BTC
            },
            new()
            {
                CreatedUtc = new DateTime(2020, 1, 1).Date,
                TransactionSource = TransactionSource.BitpandaPro,
                BuyAmount = 0.1m,
                BuyAsset = Assets.BTC,
                SellAmount = 10000,
                SellAsset = Assets.CZK
            },
            new()
            {
                CreatedUtc = new DateTime(2020, 2, 1).Date,
                TransactionSource = TransactionSource.BitpandaPro,
                BuyAmount = 0.1m,
                BuyAsset = Assets.BTC,
                SellAmount = 12000,
                SellAsset = Assets.CZK
            },
            new()
            {
                CreatedUtc = new DateTime(2020, 3, 1).Date,
                TransactionSource = TransactionSource.BitpandaPro,
                BuyAmount = 0.1m,
                BuyAsset = Assets.BTC,
                SellAmount = 12000,
                SellAsset = Assets.CZK
            },
            new()
            {
                CreatedUtc = new DateTime(2020, 8, 1).Date,
                TransactionSource = TransactionSource.BitpandaPro,
                BuyAmount = 100000m,
                BuyAsset = Assets.CZK,
                SellAmount = 0.36m,
                SellAsset = Assets.BTC
            },
            new()
            {
                CreatedUtc = new DateTime(2021, 1, 1).Date,
                TransactionSource = TransactionSource.BitpandaPro,
                BuyAmount = 0.15m,
                BuyAsset = Assets.BTC,
                SellAmount = 15000,
                SellAsset = Assets.CZK
            },
            new()
            {
                CreatedUtc = new DateTime(2021, 7, 1).Date,
                TransactionSource = TransactionSource.BitpandaPro,
                BuyAmount = 70000m,
                BuyAsset = Assets.CZK,
                SellAmount = 0.2m,
                SellAsset = Assets.BTC
            },
        };


        var trg = new TaxReportGenerator(acs, transactions);
        var tr = await trg.GetTaxReportAsync(targetFiatAsset);

        // 2018
        var btcSubReport2018 = tr.GetSubReport(Assets.BTC, 2018);
        Assert.AreEqual(20000m, btcSubReport2018.RealizedProfit);
        Assert.AreEqual(5000m, btcSubReport2018.TotalExpenditure);
        Assert.AreEqual(15000m, btcSubReport2018.TaxObligation);
        Assert.AreEqual(0.05m, btcSubReport2018.TotalSold);

        // 2019
        var btcSubReport2019 = tr.GetSubReport(Assets.BTC, 2019);
        Assert.AreEqual(3000m, btcSubReport2019.RealizedProfit);
        Assert.AreEqual(4000m, btcSubReport2019.TotalExpenditure);
        Assert.AreEqual(-1000m, btcSubReport2019.TaxObligation);
        Assert.AreEqual(0.04m, btcSubReport2019.TotalSold);

        // 2020
        var btcSubReport2020 = tr.GetSubReport(Assets.BTC, 2020);
        Assert.AreEqual(100000m, btcSubReport2020.RealizedProfit);
        Assert.AreEqual(44000m, btcSubReport2020.TotalExpenditure);
        Assert.AreEqual(56000m, btcSubReport2020.TaxObligation);
        Assert.AreEqual(0.36m, btcSubReport2020.TotalSold);

        // 2021
        var btcSubReport2021 = tr.GetSubReport(Assets.BTC, 2021);
        Assert.AreEqual(70000m, btcSubReport2021.RealizedProfit);
        Assert.AreEqual(21000m, btcSubReport2021.TotalExpenditure);
        Assert.AreEqual(49000m, btcSubReport2021.TaxObligation);
        Assert.AreEqual(0.2m, btcSubReport2021.TotalSold);
    }
}