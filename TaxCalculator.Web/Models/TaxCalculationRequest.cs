namespace TaxCalculator.Web.Models
{
    public class TaxCalculationRequest
    {
        public decimal AnnualIncome { get; set; }
        public string PostalCode { get; set; }
    }
}
