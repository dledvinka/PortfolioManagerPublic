namespace PortfolioManager.Core.Tests.TaxReport
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using NUnit.Framework;
    using PortfolioManager.Core.Services;
    using PortfolioManager.Core.Tax;

    public partial class TaxReportGeneratorTests
    {
        [Test]
        public async Task Case02a_FIFO()
        {
            var acs = new AssetConversionService(_coinsClient, _cache);
            var targetFiatAsset = Assets.CZK;

            var transactions = Enumerable.Range(1, 12).Select(i => new Transaction()
            {
                CreatedUtc = new DateTime(2018, i, 1).Date,
                TransactionSource = TransactionSource.BitpandaPro,
                BuyAmount = 1.0m / i / 10,
                BuyAsset = Assets.BTC,
                SellAmount = 4000 + i * 500,
                SellAsset = Assets.CZK
            }).ToList();

            transactions.AddRange(Enumerable.Range(1, 12).Select(i => new Transaction()
            {
                CreatedUtc = new DateTime(2018, i, 1).Date,
                TransactionSource = TransactionSource.Bitpanda,
                BuyAmount = 1.0m / i,
                BuyAsset = Assets.ETH,
                SellAmount = 4000 + i * 500,
                SellAsset = Assets.CZK
            }));

            transactions.Add(new Transaction()
            {
                CreatedUtc = new DateTime(2021, 6, 1).Date,
                TransactionSource = TransactionSource.BitpandaPro,
                BuyAmount = 100000,
                BuyAsset = Assets.CZK,
                SellAmount = 0.24m,
                SellAsset = Assets.BTC
            });

            transactions.Add(new Transaction()
            {
                CreatedUtc = new DateTime(2022, 8, 1).Date,
                TransactionSource = TransactionSource.BitpandaPro,
                BuyAmount = 400000,
                BuyAsset = Assets.CZK,
                SellAmount = 0.05m,
                SellAsset = Assets.BTC
            });

            var trg = new TaxReportGenerator(acs, transactions);
            var tr = await trg.GetTaxReportAsync(targetFiatAsset);

            // 2018
            //Assert.AreEqual(1, tr2018.SubReports.Count);

            //// 2019
            //var tr2019 = await trg.GetTaxReportAsync(2019, targetFiatAsset);
            //Assert.AreEqual(0, tr2019.SubReports.Count);

            //// 2020
            //var tr2020 = await trg.GetTaxReportAsync(2020, targetFiatAsset);
            //Assert.AreEqual(0, tr2020.SubReports.Count);

            // 2021
            //var tr2021 = await trg.GetTaxReportAsync(2021, targetFiatAsset);
            //Assert.AreEqual(1, tr2021.SubReports.Count);

            var btcSubReport2021 = tr.GetSubReport(Assets.BTC, 2021);
            btcSubReport2021.RealizedProfit.Should().Be(100000m);
            btcSubReport2021.TotalExpenditure.Should().BeApproximately(32400m, 2);
            btcSubReport2021.TaxObligation.Should().BeApproximately(67600m, 2);
            btcSubReport2021.TotalSold.Should().Be(0.24m);

            // 2022
            //var tr2022 = await trg.GetTaxReportAsync(2022, targetFiatAsset);
            //Assert.AreEqual(1, tr2022.SubReports.Count);

            var btcSubReport2022 = tr.GetSubReport(Assets.BTC, 2022);
            btcSubReport2022.RealizedProfit.Should().Be(400000m);
            btcSubReport2022.TotalExpenditure.Should().BeApproximately(32493m, 2);
            btcSubReport2022.TaxObligation.Should().BeApproximately(367507m, 2);
            btcSubReport2022.TotalSold.Should().Be(0.05m);
        }
    }
}