using Currency.Reference.Iso4217.Abstractions;
using Currency.Reference.Iso4217.Models;
using Currency.Reference.Iso4217.Extensions;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// ---- Register Currency service ----
builder.Services.AddCurrencyService();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/fiatCurrencies", ([FromServices] ICurrencyService currencyService) =>
    {
        var currencies = currencyService!
            .Query()
            .Includes
            .Type(CurrencyType.Fiat)
            .Build()
            .Select(c => $"{c.Code} - {c.Name}")
            .ToArray();
        return currencies;
    })
    .WithName("GetFiatCurrencies")
    .WithOpenApi(operation =>
    {
        operation.Summary = "Get fiat currencies (EUR, USD and others)";
        operation.Description = "Returns a short code and names.";
        return operation;
    });

app.MapGet("/preciousMetal", ([FromServices] ICurrencyService currencyService) =>
    {
        var currencies = currencyService!
            .Query()
            .Includes
            .Type(CurrencyType.PreciousMetal)
            .Build()
            .Select(c => $"{c.Code} - {c.Name}")
            .ToArray();
        return currencies;
    })
    .WithName("GetPreciousMetal")
    .WithOpenApi(operation =>
    {
        operation.Summary = "Get precious metals currencies (\"XAG\", \"XAU\", \"XPD\", \"XPT\")";
        operation.Description = "Returns a short code and names.";
        return operation;
    });

app.MapGet("/specialCurrencies", ([FromServices] ICurrencyService currencyService) =>
    {
        var currencies = currencyService!
            .Query()
            .Includes
            .Type(CurrencyType.SpecialReserve)
            .Type(CurrencyType.SpecialUnit)
            .Build()
            .Select(c => $"{c.Code} - {c.Name} ({c.CurrencyType.ToString()})")
            .ToArray();
        return currencies;
    })
    .WithName("GetSpecialCurrencies")
    .WithOpenApi(operation =>
    {
        operation.Summary = "Get special currencies (\"XBA\", \"XBB\", \"XBC\", \"XBD\", \"XSU\", \"XUA\", \"XXX\", \"XTS\")";
        operation.Description = "Returns a short code,names and type.";
        return operation;
    });

app.MapGet("/historicalCurrencies", ([FromServices] ICurrencyService currencyService) =>
    {
        var currencies = currencyService!.GetAllHistorical()
            .Select(c => new CurrencyInfo(c.Code, c.Name, c.WithdrawalDate))
            .ToArray();
        return currencies;
    })
    .WithName("GetHistoricalCurrencies")
    .WithOpenApi(operation =>
    {
        operation.Summary = "Get historical currencies";
        operation.Description = "Returns a short code, names and withdrawal date.";
        return operation;
    });

app.Run();

record CurrencyInfo(string Code, string Name, DateOnly? WithdrawalDate);