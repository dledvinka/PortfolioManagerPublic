namespace PortfolioManager.ConsoleApp
{
    using CoinGecko.Interfaces;
    using ConsoleTables;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using PortfolioManager.Core;
    using PortfolioManager.Core.Services;
    using PortfolioManager.Core.Tax;
    using PortfolioManager.Core.TransactionProcessors;
    using PortfolioManager.Core.TransactionProviders;
    using PortfolioManager.Core.TransactionProviders.Anycoin;
    using PortfolioManager.Core.TransactionProviders.Binance;
    using PortfolioManager.Core.TransactionProviders.Bitcomat;
    using PortfolioManager.Core.TransactionProviders.Bitpanda;
    using PortfolioManager.Core.TransactionProviders.BitpandaPro;
    using PortfolioManager.Core.TransactionProviders.Bittrex;
    using PortfolioManager.Core.TransactionProviders.Coinmate;
    using PortfolioManager.Core.TransactionProviders.Kraken;
    using PortfolioManager.Core.TransactionProviders.LocalBitcoins;

    public class Worker : IHostedService
    {
        private readonly AppSettings _appSettings;
        private readonly IAssetConversionService _assetConversionService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<Worker> _logger;
        private readonly IPingClient _pingClient;

        public Worker(ILogger<Worker> logger, IConfiguration configuration, IPingClient pingClient, IAssetConversionService assetConversionService)
        {
            _logger = logger;
            _pingClient = pingClient;
            _assetConversionService = assetConversionService;
            _appSettings = configuration.GetRequiredSection("AppSettings").Get<AppSettings>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.Log(LogLevel.Information, "Worker started");

            //var ping = await _pingClient.GetPingAsync();

            //// https://github.com/tosunthex/CoinGecko/tree/master/CoinGecko/Clients
            //var cc = new CoinsClient(new HttpClient(), new JsonSerializerSettings());
            //var c = await cc.GetCoinList(false);
            //var btc = c.Where(coin => coin.Symbol == "btc");
            //var eur = c.Where(coin => coin.Symbol.Contains("eur", StringComparison.InvariantCultureIgnoreCase));
            //var usd = c.Where(coin => coin.Symbol.Contains("usd", StringComparison.InvariantCultureIgnoreCase));
            //var omg = c.Where(coin => coin.Symbol.StartsWith("omg", StringComparison.InvariantCultureIgnoreCase));
            //var firo = c.Where(coin => coin.Symbol.StartsWith("firo", StringComparison.InvariantCultureIgnoreCase));
            //var bnb = c.Where(coin => coin.Name.StartsWith("binance", StringComparison.InvariantCultureIgnoreCase));
            //var snx = c.Where(coin => coin.Name.StartsWith("synthetix", StringComparison.InvariantCultureIgnoreCase));
            //var bat = c.Where(coin => coin.Name.StartsWith("basic", StringComparison.InvariantCultureIgnoreCase));

            //var sc = new SimpleClient(new HttpClient(), new JsonSerializerSettings());
            //var vs = await sc.GetSupportedVsCurrencies();
            //var pp = await sc.GetSimplePrice(new[]
            //{
            //    "bitcoin"
            //}, new[]
            //{
            //    "czk", "eur", "usd"
            //});

            //var tt = await cc.GetHistoryByCoinId("seth2", "22-12-2018", "false");
            //var eurPrice = tt.MarketData.CurrentPrice["eur"];
            //var czkPrice = tt.MarketData.CurrentPrice["czk"];
            //var usdPrice = tt.MarketData.CurrentPrice["usd"];

            var transactionProviders = new List<ITransactionProvider>()
            {
                new BitcomatTransactionProvider(),
                new LocalBitcoinsTransactionProvider(_appSettings.CoreSettings),
                new BittrexTransactionProvider(_appSettings.CoreSettings),
                new KrakenTransactionProvider(_appSettings.CoreSettings),
                new BinanceTransactionProvider(_appSettings.CoreSettings),
                new CoinmateTransactionProvider(_appSettings.CoreSettings),
                new BitpandaTransactionProvider(_appSettings.CoreSettings),
                new BitpandaProTransactionProvider(_appSettings.CoreSettings),
                new AnycoinTransactionProvider(_appSettings.CoreSettings)
            };

            var allTransactions = new List<Transaction>();

            foreach (var transactionProvider in transactionProviders)
            {
                var providersTransactions = transactionProvider.GetAllTransactions().ToList();

                if (!providersTransactions.Any())
                    throw new InvalidDataException($"No transaction from provider {transactionProvider.GetType()}");
                
                allTransactions.AddRange(providersTransactions);
            }

            allTransactions = allTransactions.OrderBy(tr => tr.CreatedUtc).ToList();

            var trCount = allTransactions.Count();

            var sts = new SwapTransactionSplitter(_assetConversionService, Assets.EUR);
            allTransactions = (await sts.ProcessAsync(allTransactions)).ToList();

            var fcc = new FiatCurrencyConverter(_assetConversionService, Assets.CZK);
            allTransactions = (await fcc.ProcessAsync(allTransactions)).ToList();

            //var allSwapTransaction = allTransactions.Where(t => t.IsSwap);

            //var tw = new JsonTextWriter()
            //    _logger
            //var cto = new ConsoleTableOptions() { OutputTo = }

            var cts = ConsoleTable
                      .From(allTransactions)
                      .Configure(o => o.NumberAlignment = Alignment.Right)
                      .ToString();

            _logger.Log(LogLevel.Information, cts);


            var btcTotal = allTransactions.Where(tr => tr.BuyAsset == Assets.BTC)
                                          .Sum(tr => tr.BuyAmount);

            var czkTotal = allTransactions.Where(tr => tr.SellAsset == Assets.CZK)
                                          .Sum(tr => tr.SellAmount);

            var eurTotal = allTransactions.Where(tr => tr.SellAsset == Assets.EUR)
                                          .Sum(tr => tr.SellAmount);

            _logger.Log(LogLevel.Information, $"BTC = {btcTotal}");
            _logger.Log(LogLevel.Information, $"CZK = {czkTotal}");
            _logger.Log(LogLevel.Information, $"EUR = {eurTotal}");

            var portfolio = new Portfolio(allTransactions, _assetConversionService);

            var btcTotal2 = await portfolio.GetCurrentBalanceAsync(Assets.BTC);
            var czkTotal2 = await portfolio.GetCurrentBalanceAsync(Assets.CZK);
            var eurTotal2 = await portfolio.GetCurrentBalanceAsync(Assets.EUR, Assets.CZK);
            var balances = (await portfolio.GetCurrentBalancesAsync()).Where(b => b.Amount != 0.0m);
            var remainingBtcAverageBuyPriceInCzk = await portfolio.GetCurrentBalanceWeightedAverageBuyPriceAsync(Assets.BTC, Assets.CZK);

            _logger.Log(LogLevel.Information, Environment.NewLine);
            _logger.Log(LogLevel.Information, $"From portfolio class:");
            _logger.Log(LogLevel.Information, $"BTC = {btcTotal2}");
            _logger.Log(LogLevel.Information, $"CZK = {czkTotal2}");
            _logger.Log(LogLevel.Information, $"EUR = {eurTotal2}");
            _logger.Log(LogLevel.Information, Environment.NewLine);

            cts = ConsoleTable
                  .From(balances)
                  .Configure(o => o.NumberAlignment = Alignment.Right)
                  .ToString();

            _logger.Log(LogLevel.Information, cts);
            _logger.Log(LogLevel.Information, $"Remaining BTC buy average price = {remainingBtcAverageBuyPriceInCzk}{Assets.CZK}");

            var trg = new TaxReportGenerator(_assetConversionService, allTransactions);
            var tr = await trg.GetTaxReportAsync(Assets.CZK);

            var yearly2017 = tr.GetCombinedYearlyReport(2017);
            var yearly2018 = tr.GetCombinedYearlyReport(2018);
            var yearly2019 = tr.GetCombinedYearlyReport(2019);
            var yearly2020 = tr.GetCombinedYearlyReport(2020);
            var yearly2021 = tr.GetCombinedYearlyReport(2021);
            var yearly2022 = tr.GetCombinedYearlyReport(2022);
            var yearly2023 = tr.GetCombinedYearlyReport(2023);

            var tree = new TaxReportExcelExporter();
            var exportFileName = Path.Combine(@"g:\My Drive\Dokumenty\CryptoHistory\", $"{DateTime.Now:yyyyMMdd_HHmmss}_TaxReportExport.xlsx");
            tree.Export(tr, exportFileName, 2023);

            _logger.Log(LogLevel.Information, "Done.");

            // Application code which might rely on the config could start here.

            // This will output the following:
            //   KeyOne = 1
            //   KeyTwo = True
            //   KeyThree:Message = Oh, that's nice...
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}