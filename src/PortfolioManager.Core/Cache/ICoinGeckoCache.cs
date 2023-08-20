namespace PortfolioManager.Core.Cache
{
    public interface ICoinGeckoCache
    {
        bool TryGetValue(string cacheKey, out decimal fiatPrice);
        void Add(string cacheKey, decimal fiatPrice);
    }
}