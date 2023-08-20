namespace PortfolioManager.Core.Converters
{
    using TinyCsvParser.TypeConverter;

    public class StringToCurrencyConverter : ITypeConverter<Asset>
    {
        public Type TargetType => typeof(Asset);

        public bool TryConvert(string value, out Asset result)
        {
            result = Assets.All.SingleOrDefault(c => c.Code == value) ?? Assets.Undefined;
            return result != Assets.Undefined;
        }

        public static Asset Convert(string value)
        {
            var currency = Assets.All.SingleOrDefault(c => c.Code == value);

            if (currency is null)
                throw new InvalidDataException($"Undefined asset: '{value}'");

            return currency;
        }
    }
}