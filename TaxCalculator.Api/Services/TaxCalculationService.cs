using System;
using Microsoft.EntityFrameworkCore;
using TaxCalculator.Api.Database;
using TaxCalculator.Api.Interfaces;
using TaxCalculator.Api.Models;
using Microsoft.Extensions.Logging;

namespace TaxCalculator.Api.Services
{
    public class TaxCalculationService
    {
        private readonly ITaxTypeResolver _taxTypeResolver;
        private readonly ITaxCalculationStrategyFactory _strategyFactory;
        private readonly TaxDbContext _dbContext;
        private readonly ILogger<TaxCalculationService> _logger;

        public TaxCalculationService(
            ITaxTypeResolver taxTypeResolver,
            ITaxCalculationStrategyFactory strategyFactory,
            TaxDbContext dbContext,
            ILogger<TaxCalculationService> logger)
        {
            _taxTypeResolver = taxTypeResolver;
            _strategyFactory = strategyFactory;
            _dbContext = dbContext;
            _logger = logger;
        }

        public decimal CalculateTax(string postalCode, decimal annualIncome)
        {
            try
            {
                var taxType = _taxTypeResolver.ResolveTaxType(postalCode);
                ITaxCalculationStrategy taxCalculationStrategy = _strategyFactory.CreateTaxCalculationStrategy(taxType);
                decimal calculatedTax = taxCalculationStrategy.CalculateTax(annualIncome);

                // Save to database
                var taxRecord = new TaxCalculation
                {
                    PostalCode = postalCode,
                    AnnualIncome = annualIncome,
                    CalculatedTax = calculatedTax,
                    CalculationDate = DateTime.Now
                };

                _dbContext.TaxCalculations.Add(taxRecord);
                _dbContext.SaveChanges();

                return calculatedTax;
            }
            catch (DbUpdateException ex)
            {
                // Log the database update exception
                _logger.LogError(ex, "An error occurred during tax calculation - Database update issue");
                throw; // Rethrow the exception
            }
            catch (Exception ex)
            {
                // Log other exceptions
                _logger.LogError(ex, "An error occurred during tax calculation");
                throw; // Rethrow the exception
            }
        }
    }
}
