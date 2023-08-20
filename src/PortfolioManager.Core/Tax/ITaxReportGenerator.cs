namespace PortfolioManager.Core.Tax;

public interface ITaxReportGenerator
{
    Task<TaxReport> GetTaxReportAsync(Asset targetFiatAsset);
}