namespace PortfolioManager.Core.Tests.Mocks;

using System;
using System.Threading.Tasks;
using PortfolioManager.Core.Services;

public class AssetConversionServiceMock : IAssetConversionService
{
    private readonly decimal _conversionRate;

    public AssetConversionServiceMock(decimal conversionRate) => _conversionRate = conversionRate;

    public Task<decimal> ConvertAsync(decimal sourceAmount, Asset sourceAsset, Asset targetAsset, DateTime pointInTimeUtc) => Task.FromResult(sourceAsset == targetAsset ? sourceAmount : sourceAmount * _conversionRate);
}