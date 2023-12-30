using TaxCalculator.Api.Interfaces;

namespace TaxCalculator.Api.Services.Strategies
{
    public class FlatRateTaxStrategy : ITaxCalculationStrategy
    {
        public decimal CalculateTax(decimal annualIncome)
        {
            return 0.175m * annualIncome;
        }
    }
}
