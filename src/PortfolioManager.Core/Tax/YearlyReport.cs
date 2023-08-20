namespace PortfolioManager.Core.Tax
{
    public class YearlyReport
    {
        private readonly IEnumerable<TaxSubReport> _allSubReportsForGivenYear;

        public decimal RealizedProfitLoss => Math.Ceiling(_allSubReportsForGivenYear.Sum(sr => sr.RealizedProfit));
        public decimal TaxObligation => RealizedProfitLoss - TotalExpenditure;
        public decimal TotalExpenditure => Math.Floor(_allSubReportsForGivenYear.Sum(sr => sr.TotalExpenditure));

        public YearlyReport(IEnumerable<TaxSubReport> allSubReportsForGivenYear) => _allSubReportsForGivenYear = allSubReportsForGivenYear;
    }
}