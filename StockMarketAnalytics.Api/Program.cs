using Microsoft.EntityFrameworkCore;
using StockMarketAnalytics.Api.Data;
using StockMarketAnalytics.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddScoped<IInstrumentService, InstrumentService>();
builder.Services.AddScoped<IHistoricalPriceService, HistoricalPriceService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("ReactFrontend");

app.MapGet("/", () => Results.Ok(new
{
    message = "Stock Market Analytics API is running",
    status = "Healthy"
}));

app.MapControllers();

app.Run();