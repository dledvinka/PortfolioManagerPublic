namespace PortfolioManager.Core.Tests.TaxReport
{
    using System.Net.Http;
    using CoinGecko.Clients;
    using Newtonsoft.Json;
    using PortfolioManager.Core.Cache;
    using PortfolioManager.Core.Tests.Mocks;

    public partial class TaxReportGeneratorTests
    {
        private readonly ICoinGeckoCache _cache = new CoinGeckoCacheMock();
        private readonly CoinsClient _coinsClient = new CoinsClient(new HttpClient(), new JsonSerializerSettings());
    }
}