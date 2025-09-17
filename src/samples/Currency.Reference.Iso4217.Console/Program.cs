using Currency.Reference.Iso4217;
using Currency.Reference.Iso4217.Abstractions;
using Currency.Reference.Iso4217.Extensions;
using Currency.Reference.Iso4217.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddCurrencyService();
    })
    .Build();
try
{
    var container = host.Services;
    using var scope = container.CreateScope();
    var currencyService = scope.ServiceProvider.GetCurrencyService();
    
    //var currencyService = scope.ServiceProvider.GetRequiredService<ICurrencyServiceSafe>();

    //var allCurrencies = currencyService.GetAll();
    //var uniqueCurrencies = currencyService.GetUniqueCodesWithNames();
    //var isValidEuro = currencyService.IsValid("EUR", [Field.Code]);
    //var getEuro = currencyService.Get("EUR", Field.Code);
    
    //var fiatCurrencies = currencyService.GetFiatCurrencies();
    //var preciousMetalsCurrencies = currencyService.GetPreciousMetals();
    //var specialReservedCurrencies = currencyService.GetSpecialReserveCodes();
    //var specialUnitsCurrencies = currencyService.GetSpecialUnits();
    
    /*
    var currencies = currencyService.Query()
        .IncludesSpecialUnits()
        .Build();
        */
    
    var currencies = currencyService.Query()
        .Includes
        .Type(CurrencyType.Fiat)
        .Type(CurrencyType.SpecialUnit)
        //.Without(w => w.Codes("XUA", "USD"))
        //.Without(w => w.Codes("GBP", "EUR"))
        .Build();

    var result= currencies.Any(c => c.Code == "EUR");
    
    foreach (var currency in currencies)
        Console.WriteLine($"{currency.Code} - {currency.Name}");
    
    await host.RunAsync();
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}