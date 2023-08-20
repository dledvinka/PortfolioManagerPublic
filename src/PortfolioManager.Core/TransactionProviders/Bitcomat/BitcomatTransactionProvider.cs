namespace PortfolioManager.Core.TransactionProviders.Bitcomat
{
    public class BitcomatTransactionProvider : ITransactionProvider
    {
        public IEnumerable<Transaction> GetAllTransactions() =>
            new List<Transaction>()
            {
                new Transaction()
                {
                    BuyAmount = 0.01m,
                    BuyAsset = Assets.BTC,
                    CreatedUtc = new DateTime(2017, 10, 10).Date,
                    SellAsset = Assets.CZK,
                    SellAmount = 2000.0m,
                    TransactionSource = TransactionSource.Bitcomat
                },
                new Transaction()
                {
                    BuyAmount = 1.95144097M + 0.11764M,
                    BuyAsset = Assets.LTC,
                    CreatedUtc = new DateTime(2017, 10, 10).Date,
                    SellAsset = Assets.CZK,
                    SellAmount = 2600.0m,
                    TransactionSource = TransactionSource.Bitcomat
                }
               ,
                new Transaction()
                {
                    BuyAmount = 2.71849M,
                    BuyAsset = Assets.LTC,
                    CreatedUtc = new DateTime(2017, 10, 10).Date,
                    SellAsset = Assets.CZK,
                    SellAmount = 3500.0m,
                    TransactionSource = TransactionSource.Bitcomat
                },
                new Transaction()
                {
                    BuyAmount = 209M,
                    BuyAsset = Assets.WABI,
                    CreatedUtc = new DateTime(2017, 10, 10).Date,
                    SellAsset = Assets.CZK,
                    SellAmount = 100M, // TODO DL fictional transaction
                    TransactionSource = TransactionSource.Bitcomat
                }
            };
    }
}