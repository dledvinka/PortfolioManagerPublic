namespace PortfolioManager.Core.TransactionProviders.Kraken
{
    public class KrakenTransactionProvider : ITransactionProvider
    {
        public KrakenTransactionProvider(CoreSettings _)
        {
        }

        public IEnumerable<Transaction> GetAllTransactions() =>
            new List<Transaction>()
            {
                new ()
                {
                    BuyAmount = 0.0305568240m,
                    BuyAsset = Assets.BTC,
                    CreatedUtc = new DateTime(2022, 5, 10).Date,
                    SellAsset = Assets.ADA,
                    SellAmount = 1435.940971m,
                    FeeAsset = Assets.BTC,
                    FeeAmount = 0.00002128m,
                    TransactionSource = TransactionSource.Kraken
                },
                //new ()
                //{
                //    BuyAmount = 0.9918206800m,
                //    BuyAsset = Assets.ETH,
                //    CreatedUtc = new DateTime(2022, 5, 28).Date,
                //    SellAsset = Assets.ETH2S,
                //    SellAmount = 0.9960m,
                //    TransactionSource = TransactionSource.Kraken
                //},
                new ()
                {
                    BuyAmount = 0.0606910000m,
                    BuyAsset = Assets.BTC,
                    CreatedUtc = new DateTime(2022, 5, 28).Date,
                    SellAsset = Assets.ETH,
                    SellAmount = 0.98925m,
                    TransactionSource = TransactionSource.Kraken
                },
            };
    }
}