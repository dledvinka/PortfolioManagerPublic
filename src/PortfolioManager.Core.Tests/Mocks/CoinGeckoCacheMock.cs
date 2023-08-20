namespace PortfolioManager.Core.Tests.Mocks
{
    using PortfolioManager.Core.Cache;

    public class CoinGeckoCacheMock : ICoinGeckoCache
    {
        public void Add(string cacheKey, decimal fiatPrice)
        {
        }

        public bool TryGetValue(string cacheKey, out decimal fiatPrice)
        {
            fiatPrice = 0;
            return false;
        }
    }
}