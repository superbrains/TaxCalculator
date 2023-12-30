namespace TaxCalculator.Api.Interfaces
{
    public interface ITaxCalculationStrategy
    {
        decimal CalculateTax(decimal annualIncome);
    }
}
