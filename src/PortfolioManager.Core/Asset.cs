namespace PortfolioManager.Core;

public record Asset(string Code, bool IsFiat, string? CoinGeckoId)
{
    public override string ToString() => Code;
}