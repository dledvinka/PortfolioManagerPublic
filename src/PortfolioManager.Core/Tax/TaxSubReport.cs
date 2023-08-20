namespace PortfolioManager.Core.Tax;

public class TaxSubReport
{
    public Asset Asset { get; }
    public List<Transaction> BuyTransactionQueue { get; set; }
    public decimal RealizedProfit { get; set; }
    public List<Transaction> SellTransactionsForCurrentYear { get; set; }
    public decimal TaxObligation => RealizedProfit - TotalExpenditure;
    public decimal TotalExpenditure { get; set; }
    public decimal TotalSold { get; set; }
    public Dictionary<Guid, List<Transaction>> TransactionPairings { get; set; }
    public int Year { get; }

    public TaxSubReport(Asset asset, int year)
    {
        Asset = asset;
        Year = year;
        TransactionPairings = new Dictionary<Guid, List<Transaction>>();
    }

    public override string ToString() => $"SubReport for {Year}/{Asset}";
}