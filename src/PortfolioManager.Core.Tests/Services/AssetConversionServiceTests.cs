namespace PortfolioManager.Core.Tests.Services
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using CoinGecko.Clients;
    using Newtonsoft.Json;
    using NUnit.Framework;
    using PortfolioManager.Core.Cache;
    using PortfolioManager.Core.Services;
    using PortfolioManager.Core.Tests.Mocks;

    public class AssetConversionServiceTests
    {
        private readonly ICoinGeckoCache _cache = new CoinGeckoCacheMock();
        private readonly CoinsClient _coinsClient = new CoinsClient(new HttpClient(), new JsonSerializerSettings());

        [Test]
        public async Task EurToCzkFiatConversion()
        {
            var acs = new AssetConversionService(_coinsClient, _cache);
            var czk = await acs.ConvertAsync(100.0m, Assets.EUR, Assets.CZK, new DateTime(2019, 1, 1));

            Assert.AreEqual(2566.0m, czk);
        }

        [Test]
        public async Task CzkToEurFiatConversion()
        {
            var acs = new AssetConversionService(_coinsClient, _cache);
            var eur = await acs.ConvertAsync(2566.0m, Assets.CZK, Assets.EUR, new DateTime(2019, 1, 1));

            Assert.AreEqual(100.0m, Math.Round(eur, 5));
        }

        [Test]
        public async Task BtcToFiatConversion()
        {
            var date = new DateTime(2017, 12, 22).Date;
            var acs = new AssetConversionService(_coinsClient, _cache);
            var eur = await acs.ConvertAsync(1.0m, Assets.BTC, Assets.EUR, date);
            var usd = await acs.ConvertAsync(1.0m, Assets.BTC, Assets.USD, date);
            var czk = await acs.ConvertAsync(1.0m, Assets.BTC, Assets.CZK, date);

            Assert.AreEqual(11877.9159634748m, eur);
            Assert.AreEqual(14084.8249267767M, usd);
            Assert.AreEqual(305797.0424677416m, czk);
        }

        [Test]
        public async Task AllCryptoAssetsToFiatConversion_DoesntThrow()
        {
            var date = new DateTime(2021, 12, 22).Date;
            var acs = new AssetConversionService(_coinsClient, _cache);

            foreach (var cryptoAsset in Assets.All.Where(asset => !asset.IsFiat))
            {
                var value = await acs.ConvertAsync(1.0m, cryptoAsset, Assets.EUR, date);
            }
        }

        [Test]
        public async Task FiatToEthConversion()
        {
            var date = new DateTime(2018, 12, 22).Date;
            var acs = new AssetConversionService(_coinsClient, _cache);
            var sourceAssetAmount = 10000.0m;
            var eur = await acs.ConvertAsync(sourceAssetAmount, Assets.EUR, Assets.ETH, date);
            var usd = await acs.ConvertAsync(sourceAssetAmount, Assets.USD, Assets.ETH, date);
            var czk = await acs.ConvertAsync(sourceAssetAmount, Assets.CZK, Assets.ETH, date);

            Assert.AreEqual(sourceAssetAmount / 94.8535293333884M, eur);
            Assert.AreEqual(sourceAssetAmount / 107.85318110517642M, usd);
            Assert.AreEqual(sourceAssetAmount / 2452.3009200608385M, czk);
        }
    }
}