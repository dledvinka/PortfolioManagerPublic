namespace PortfolioManager.Core;

public record AssetAmount(decimal Amount, Asset Asset)
{
    public override string ToString()
    {
        if (Asset.IsFiat)
            return $"{Amount:F2} {Asset}";
        else
            return $"{Amount:F8} {Asset}";
    }
}