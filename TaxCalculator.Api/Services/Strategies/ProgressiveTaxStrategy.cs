using TaxCalculator.Api.Interfaces;

namespace TaxCalculator.Api.Services.Strategies
{
    public class ProgressiveTaxStrategy : ITaxCalculationStrategy
    {
        public decimal CalculateTax(decimal annualIncome)
        {
            // Progressive tax rates and brackets
            var brackets = new[]
            {
            (0m, 8350m, 0.1m),
            (8351m, 33950m, 0.15m),
            (33951m, 82250m, 0.25m),
            (82251m, 171550m, 0.28m),
            (171551m, 372950m, 0.33m),
            (372951m, decimal.MaxValue, 0.35m)
        };

            decimal tax = 0;

            foreach (var (from, to, rate) in brackets)
            {
                if (annualIncome > from)
                {
                    decimal taxableAmountInBracket = Math.Min(to - from, annualIncome - from);
                    tax += taxableAmountInBracket * rate;
                }
                else
                {
                    break;
                }
            }

            return tax;
        }
    }
}
