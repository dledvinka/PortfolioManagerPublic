namespace PortfolioManager.Core.Tax
{
    public record AssetYearKey(Asset Asset, int Year)
    {
        public override string ToString() => $"{Asset}_{Year}";
    }
}