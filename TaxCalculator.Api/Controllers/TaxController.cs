using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaxCalculator.Api.Dtos;
using TaxCalculator.Api.Services;

namespace TaxCalculator.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxController : ControllerBase
    {
        private readonly TaxCalculationService _taxCalculatorService;

        public TaxController(TaxCalculationService taxCalculatorService)
        {
            _taxCalculatorService = taxCalculatorService;
        }

        [HttpPost]
        public ActionResult<decimal> Calculate(TaxCalculationRequest request)
        {
            decimal calculatedTax = _taxCalculatorService.CalculateTax(request.PostalCode, request.AnnualIncome);
            return Ok(calculatedTax);
        }
    }
}
