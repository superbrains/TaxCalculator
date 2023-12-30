using TaxCalculator.Api.Enums;

namespace TaxCalculator.Api.Interfaces
{
    public interface ITaxTypeResolver
    {
        TaxCalculationType ResolveTaxType(string postalCode);
    }
}
