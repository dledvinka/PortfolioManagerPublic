namespace PortfolioManager.Core.Tax
{
    using System.Collections;

    public class TransactionQueue : IEnumerable<Transaction>
    {
        private readonly Queue<Transaction> _queue;

        public TransactionQueue(IOrderedEnumerable<Transaction> transactions) => _queue = new Queue<Transaction>(transactions);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<Transaction> GetEnumerator() => _queue.GetEnumerator();

        public bool TryPeek(out Transaction transaction) => _queue.TryPeek(out transaction);

        public Transaction DequeueWhole() => _queue.Dequeue();

        public Transaction DequeuePart(decimal buyAmountPart)
        {
            var currentTransaction = _queue.Peek();
            return currentTransaction.ChipAwayBuyAmount(buyAmountPart);
        }
    }
}