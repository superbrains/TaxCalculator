using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using TaxCalculator.Web.Models;

namespace TaxCalculator.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, HttpClient httpClient, IConfiguration configuration)
        {
            _logger = logger;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CalculateTax(TaxCalculationRequest request)
        {
            try
            {
                string apiBaseUrl = _configuration.GetValue<string>("TaxApiBaseUrl")!;
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"{apiBaseUrl}/api/Tax", request);
                response.EnsureSuccessStatusCode();

                decimal calculatedTax = await response.Content.ReadFromJsonAsync<decimal>();
                ViewBag.CalculatedTax = calculatedTax;

                return View("Index");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Error calling tax calculation API: {ex.Message}");
                ViewBag.ErrorMessage = "Error calling tax calculation API.";
                return View("Index");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
