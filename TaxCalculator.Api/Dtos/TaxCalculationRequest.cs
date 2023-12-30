namespace TaxCalculator.Api.Dtos
{
    public class TaxCalculationRequest
    {
        public decimal AnnualIncome { get; set; }
        public string PostalCode { get; set; }
    }
}
