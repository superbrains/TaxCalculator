using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TaxCalculator.Api.Models;

namespace TaxCalculator.Api.Database
{
    public class TaxDbContext : DbContext
    {
        public TaxDbContext(DbContextOptions<TaxDbContext> options) : base(options) { }

        public DbSet<TaxCalculation> TaxCalculations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Only configure SQLite if no other provider has been configured
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=taxcalculator.db");
            }
        }

    }

}
