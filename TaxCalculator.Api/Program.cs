using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaxCalculator.Api.Database;
using TaxCalculator.Api.Interfaces;
using TaxCalculator.Api.Services;
using TaxCalculator.Api.Utilities;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json");

builder.Services.AddDbContext<TaxDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ITaxTypeResolver, TaxTypeResolver>();
builder.Services.AddScoped<ITaxCalculationStrategyFactory, TaxCalculationStrategyFactory>();
builder.Services.AddScoped<TaxCalculationService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Apply Migrations during startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TaxDbContext>();
    dbContext.Database.Migrate();
}

app.MapControllers();
app.Run();
