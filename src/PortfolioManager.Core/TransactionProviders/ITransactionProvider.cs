namespace PortfolioManager.Core.TransactionProviders
{
    public interface ITransactionProvider
    {
        public IEnumerable<Transaction> GetAllTransactions();
    }
}