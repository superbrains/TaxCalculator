using TaxCalculator.Api.Enums;
using TaxCalculator.Api.Interfaces;
using TaxCalculator.Api.Services.Strategies;

namespace TaxCalculator.Api.Services
{
    public class TaxCalculationStrategyFactory : ITaxCalculationStrategyFactory
    {
        public ITaxCalculationStrategy CreateTaxCalculationStrategy(TaxCalculationType taxType)
        {
            switch (taxType)
            {
                case TaxCalculationType.Progressive:
                    return new ProgressiveTaxStrategy();

                case TaxCalculationType.FlatValue:
                    return new FlatValueTaxStrategy();

                case TaxCalculationType.FlatRate:
                    return new FlatRateTaxStrategy();

                default:
                    throw new InvalidOperationException("Invalid tax calculation type");
            }
        }
    }

}
