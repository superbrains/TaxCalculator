namespace TaxCalculator.Test
{
    using NUnit.Framework;
    using Moq;
    using TaxCalculator.Api.Interfaces;
    using TaxCalculator.Api.Services;
    using TaxCalculator.Api.Enums;
    using Microsoft.EntityFrameworkCore;
    using TaxCalculator.Api.Database;
    using Microsoft.Extensions.Logging;
    using System;

    [TestFixture]
    public class TaxCalculationServiceTests
    {
        private Mock<ITaxTypeResolver> taxTypeResolverMock;
        private Mock<ITaxCalculationStrategyFactory> strategyFactoryMock;
        private Mock<ILogger<TaxCalculationService>> loggerMock;
        private DbContextOptions<TaxDbContext> dbContextOptions;

        [SetUp]
        public void Setup()
        {
            taxTypeResolverMock = new Mock<ITaxTypeResolver>();
            strategyFactoryMock = new Mock<ITaxCalculationStrategyFactory>();
            loggerMock = new Mock<ILogger<TaxCalculationService>>();
            dbContextOptions = new DbContextOptionsBuilder<TaxDbContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;
        }

        [Test]
        public void CalculateTax_ShouldCallStrategyFactoryWithCorrectType()
        {
            // Arrange
            var progressiveTaxStrategyMock = new Mock<ITaxCalculationStrategy>();
            strategyFactoryMock.Setup(x => x.CreateTaxCalculationStrategy(TaxCalculationType.Progressive)).Returns(progressiveTaxStrategyMock.Object);

            using var dbContext = new TaxDbContext(dbContextOptions);

            var taxService = new TaxCalculationService(taxTypeResolverMock.Object, strategyFactoryMock.Object, dbContext, loggerMock.Object);

            // Act
            taxService.CalculateTax("7441", 50000);

            // Assert
            taxTypeResolverMock.Verify(x => x.ResolveTaxType("7441"), Times.Once);
            strategyFactoryMock.Verify(x => x.CreateTaxCalculationStrategy(It.IsAny<TaxCalculationType>()), Times.Once);
        }

        [Test]
        public void CalculateTax_ShouldCallCalculateTaxOnStrategy()
        {
            // Arrange
            var strategyMock = new Mock<ITaxCalculationStrategy>();
            strategyFactoryMock.Setup(x => x.CreateTaxCalculationStrategy(It.IsAny<TaxCalculationType>())).Returns(strategyMock.Object);

            using var dbContext = new TaxDbContext(dbContextOptions);

            var taxService = new TaxCalculationService(taxTypeResolverMock.Object, strategyFactoryMock.Object, dbContext, loggerMock.Object);

            // Act
            taxService.CalculateTax("7441", 50000);

            // Assert
            strategyMock.Verify(x => x.CalculateTax(50000), Times.Once);
        }

        [TestCase("7441", 50000, TaxCalculationType.Progressive, 7500)]
        [TestCase("A100", 150000, TaxCalculationType.FlatValue, 7500)]
        [TestCase("7000", 50000, TaxCalculationType.FlatRate, 8750)]
        public void CalculateTax_ShouldReturnCorrectTax(string postalCode, decimal annualIncome, TaxCalculationType taxType, decimal expectedResult)
        {
            // Arrange
            taxTypeResolverMock.Setup(x => x.ResolveTaxType(postalCode)).Returns(taxType);

            var strategyMock = new Mock<ITaxCalculationStrategy>();
            strategyMock.Setup(x => x.CalculateTax(annualIncome)).Returns(expectedResult);
            strategyFactoryMock.Setup(x => x.CreateTaxCalculationStrategy(taxType)).Returns(strategyMock.Object);

            using var dbContext = new TaxDbContext(dbContextOptions);

            var taxService = new TaxCalculationService(taxTypeResolverMock.Object, strategyFactoryMock.Object, dbContext, loggerMock.Object);

            // Act
            decimal result = taxService.CalculateTax(postalCode, annualIncome);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void CalculateTax_ShouldSaveToDatabase()
        {
            // Arrange
            taxTypeResolverMock.Setup(x => x.ResolveTaxType("7441")).Returns(TaxCalculationType.Progressive);

            var strategyMock = new Mock<ITaxCalculationStrategy>();
            strategyFactoryMock.Setup(x => x.CreateTaxCalculationStrategy(TaxCalculationType.Progressive)).Returns(strategyMock.Object);

            using var dbContext = new TaxDbContext(dbContextOptions);

            var taxService = new TaxCalculationService(taxTypeResolverMock.Object, strategyFactoryMock.Object, dbContext, loggerMock.Object);

            // Act
            taxService.CalculateTax("7441", 50000);

            // Assert
            var savedRecord = dbContext.TaxCalculations.FirstOrDefault();
            Assert.IsNotNull(savedRecord);
            Assert.That(savedRecord.PostalCode, Is.EqualTo("7441"));
            Assert.That(savedRecord.AnnualIncome, Is.EqualTo(50000));
            Assert.That(savedRecord.CalculatedTax, Is.EqualTo(0));
            Assert.IsTrue(DateTime.UtcNow - savedRecord.CalculationDate < TimeSpan.FromSeconds(1));
        }
    }
}
