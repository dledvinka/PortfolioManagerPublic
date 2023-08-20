namespace PortfolioManager.Core.Cache
{
    using System.Text.Json;

    public class CoinGeckoCache : ICoinGeckoCache
    {
        private const string CacheFileName = "CoinGeckoCache.json";
        private Dictionary<string, decimal>? _cacheContent;
        private bool _cacheLoaded = false;

        public void Add(string cacheKey, decimal fiatPrice)
        {
            _cacheContent.Add(cacheKey, fiatPrice);
            var serializedCache = JsonSerializer.Serialize(_cacheContent);
            File.WriteAllText(CacheFileName, serializedCache);
        }

        public bool TryGetValue(string cacheKey, out decimal fiatPrice)
        {
            if (!_cacheLoaded)
            {
                if (!File.Exists(CacheFileName))
                    _cacheContent = new Dictionary<string, decimal>();
                else
                {
                    var fileContent = File.ReadAllText(CacheFileName);
                    _cacheContent = JsonSerializer.Deserialize<Dictionary<string, decimal>>(fileContent);
                }

                _cacheLoaded = true;
            }

            return _cacheContent.TryGetValue(cacheKey, out fiatPrice);
        }
    }
}