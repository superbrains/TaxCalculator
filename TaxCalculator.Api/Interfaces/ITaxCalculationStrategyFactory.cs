using TaxCalculator.Api.Enums;

namespace TaxCalculator.Api.Interfaces
{
    public interface ITaxCalculationStrategyFactory
    {
        ITaxCalculationStrategy CreateTaxCalculationStrategy(TaxCalculationType taxType);
    }

}
