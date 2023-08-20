namespace PortfolioManager.Core.Tax;

public interface ITaxReportExcelExporter
{
    void Export(TaxReport taxReport, string targetFileName, int year);
}