namespace PortfolioManager.Core.Services
{
    using CoinGecko.Interfaces;
    using PortfolioManager.Core.Cache;

    public class AssetConversionService : IAssetConversionService
    {
        private readonly ICoinGeckoCache _cache;
        private readonly ICoinsClient _coinsClient;

        public AssetConversionService(ICoinsClient coinsClient, ICoinGeckoCache cache)
        {
            _coinsClient = coinsClient;
            _cache = cache;
        }

        public async Task<decimal> ConvertAsync(decimal sourceAmount, Asset sourceAsset, Asset targetAsset, DateTime pointInTimeUtc)
        {
            if (sourceAsset == targetAsset)
                return sourceAmount;

            if (sourceAsset.IsFiat && targetAsset.IsFiat)
                return await ConvertFiatAsync(sourceAmount, sourceAsset, targetAsset, pointInTimeUtc);

            return await ConvertCryptoToFiatAsync(sourceAmount, sourceAsset, targetAsset, pointInTimeUtc);
        }

        private async Task<decimal> ConvertCryptoToFiatAsync(decimal sourceAmount, Asset sourceAsset, Asset targetAsset, DateTime pointInTimeUtc)
        {
            if ((sourceAsset.IsFiat && targetAsset.IsFiat) || (!sourceAsset.IsFiat && !targetAsset.IsFiat))
                throw new InvalidOperationException("One of the assets has to be fiat and the other has to be crypto!");

            var cryptoAsset = sourceAsset.IsFiat ? targetAsset : sourceAsset;
            var fiatAsset = sourceAsset.IsFiat ? sourceAsset : targetAsset;

            var cryptoPriceInFiat = await GetCryptoPriceInFiat(cryptoAsset, fiatAsset, pointInTimeUtc);

            if (sourceAsset.IsFiat)
                return sourceAmount / cryptoPriceInFiat;
            else // if (targetAsset.IsFiat)
                return sourceAmount * cryptoPriceInFiat;
        }

        private async Task<decimal> GetCryptoPriceInFiat(Asset cryptoAsset, Asset fiatAsset, DateTime pointInTimeUtc)
        {
            var dayString = pointInTimeUtc.Date.ToString("dd-MM-yyyy");

            var cacheKey = $"{dayString}-{cryptoAsset.CoinGeckoId}-{fiatAsset.Code}";

            if (_cache.TryGetValue(cacheKey, out var cachedFiatPrice))
                return cachedFiatPrice;

            var coinSnapshot = await _coinsClient.GetHistoryByCoinId(cryptoAsset.CoinGeckoId, dayString, "true");

            if (!coinSnapshot.MarketData.CurrentPrice.ContainsKey(fiatAsset.Code.ToLower()))
                throw new InvalidDataException($"Coin price in fiat currency with code '{fiatAsset.Code}' is unknown to CoinGecko!");

            var fiatPrice = coinSnapshot.MarketData.CurrentPrice[fiatAsset.Code.ToLower()].Value;
            _cache.Add(cacheKey, fiatPrice);

            return fiatPrice;
        }

        private Task<decimal> ConvertFiatAsync(decimal sourceAmount, Asset sourceAsset, Asset targetAsset, DateTime pointInTimeUtc)
        {
            // https://www.kodap.cz/cs/pro-vas/prehledy/jednotny-kurz/jednotne-kurzy-men-stanovene-ministerstvem-financi-prehled.html

            var conversionRation = (pointInTimeUtc.Year, sourceAsset.Code, targetAsset.Code) switch
            {
                (2017, "EUR", "CZK") => 26.29m,
                (2018, "EUR", "CZK") => 25.68m,
                (2019, "EUR", "CZK") => 25.66m,
                (2020, "EUR", "CZK") => 26.50m,
                (2021, "EUR", "CZK") => 25.65m,
                (2022, "EUR", "CZK") => 25.65m, // Update when available
                (2023, "EUR", "CZK") => 25.65m, // Update when available
                (2017, "CZK", "EUR") => 1 / 26.29m,
                (2018, "CZK", "EUR") => 1 / 25.68m,
                (2019, "CZK", "EUR") => 1 / 25.66m,
                (2020, "CZK", "EUR") => 1 / 26.50m,
                (2021, "CZK", "EUR") => 1 / 25.65m,
                (2022, "CZK", "EUR") => 1 / 25.65m, // Update when available
                (2023, "CZK", "EUR") => 1 / 25.65m, // Update when available
                _ => throw new Exception()
            };

            return Task.FromResult(sourceAmount * conversionRation);
        }
    }
}