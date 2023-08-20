namespace PortfolioManager.Core.TransactionProcessors
{
    public interface ITransactionProcessor
    {
        Task<IEnumerable<Transaction>> ProcessAsync(IEnumerable<Transaction> transactions);
    }
}