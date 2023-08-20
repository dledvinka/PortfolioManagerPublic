namespace PortfolioManager.Core
{
    public class Transaction
    {
        public decimal BuyAmount { get; set; }
        public Asset BuyAsset { get; set; }
        public DateTime CreatedUtc { get; set; }
        public Guid Id { get; private set; } = Guid.NewGuid();
        public bool IsSwap => !BuyAsset.IsFiat && !SellAsset.IsFiat;
        public int Order { get; set; }
        public int? RowIndex { get; set; }
        public decimal SellAmount { get; set; }
        public Asset SellAsset { get; set; }
        public TransactionSource TransactionSource { get; set; }
        public decimal FeeAmount { get; set; }
        public Asset FeeAsset { get; set; }

        public override string ToString() => $"{SellAmount} {SellAsset.Code} => {BuyAmount} {BuyAsset.Code} : {CreatedUtc} @ {TransactionSource} ({RowIndex?.ToString() ?? string.Empty})";

        public Transaction Clone() =>
            new()
            {
                Id = Id,
                BuyAmount = BuyAmount,
                BuyAsset = BuyAsset,
                CreatedUtc = CreatedUtc,
                Order = Order,
                RowIndex = RowIndex,
                SellAmount = SellAmount,
                SellAsset = SellAsset,
                TransactionSource = TransactionSource
            };

        public Transaction ChipAwayBuyAmount(decimal buyAmountPart)
        {
            if (BuyAmount < buyAmountPart)
                throw new InvalidDataException("Cannot chip away more that entire current BuyAmount");

            var clone = Clone();
            clone.BuyAmount = buyAmountPart;
            clone.SellAmount = buyAmountPart * SellAmount / BuyAmount;

            BuyAmount -= buyAmountPart;
            SellAmount -= clone.SellAmount;

            return clone;
        }
    }
}