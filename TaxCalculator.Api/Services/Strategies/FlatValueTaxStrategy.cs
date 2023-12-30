using TaxCalculator.Api.Interfaces;

namespace TaxCalculator.Api.Services.Strategies
{
    public class FlatValueTaxStrategy : ITaxCalculationStrategy
    {
        public decimal CalculateTax(decimal annualIncome)
        {
            return annualIncome < 200000m ? 0.05m * annualIncome : 10000m;
        }
    }

}
