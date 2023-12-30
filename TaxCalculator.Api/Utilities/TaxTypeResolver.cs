using TaxCalculator.Api.Enums;
using TaxCalculator.Api.Interfaces;

namespace TaxCalculator.Api.Utilities
{
    public class TaxTypeResolver : ITaxTypeResolver
    {
        public TaxCalculationType ResolveTaxType(string postalCode)
        {
            switch (postalCode)
            {
                case "7441":
                case "1000":
                    return TaxCalculationType.Progressive;

                case "A100":
                    return TaxCalculationType.FlatValue;

                case "7000":
                    return TaxCalculationType.FlatRate;

                default:
                    // Default to progressive tax if postal code is not recognized
                    return TaxCalculationType.Progressive;
            }
        }
    }
}
