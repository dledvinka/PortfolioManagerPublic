namespace PortfolioManager.Core.Services
{
    public interface IAssetConversionService
    {
        public Task<decimal> ConvertAsync(decimal sourceAmount, Asset sourceAsset, Asset targetAsset, DateTime pointInTimeUtc);
    }
}