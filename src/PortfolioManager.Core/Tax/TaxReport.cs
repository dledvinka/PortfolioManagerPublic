namespace PortfolioManager.Core.Tax;

public class TaxReport
{
    public List<Transaction> AllTransactions { get; set; }
    public Dictionary<AssetYearKey, TaxSubReport> SubReports { get; } = new();

    public TaxSubReport GetSubReport(Asset asset, int year) => SubReports[new AssetYearKey(asset, year)];

    public YearlyReport GetCombinedYearlyReport(int year)
    {
        var allForSpecifiedYear = SubReports.Where(sr => sr.Key.Year == year).Select(sr => sr.Value);
        return new YearlyReport(allForSpecifiedYear);
    }
}