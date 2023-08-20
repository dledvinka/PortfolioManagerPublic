namespace PortfolioManager.ConsoleApp;

using CoinGecko.Clients;
using CoinGecko.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using PortfolioManager.Core.Cache;
using PortfolioManager.Core.Services;

public class Program
{
    // https://docs.microsoft.com/en-us/dotnet/core/extensions/logging?tabs=command-line
    // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-6.0

    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                var jsonSerializerSettings = new JsonSerializerSettings();

                services.AddSingleton(jsonSerializerSettings);
                services.AddHttpClient<IPingClient, PingClient>();
                services.AddHttpClient<ICoinsClient, CoinsClient>();
                services.AddSingleton<ICoinGeckoCache, CoinGeckoCache>();
                services.AddSingleton<IAssetConversionService, AssetConversionService>();
                services.AddHostedService<Worker>();
            });
}