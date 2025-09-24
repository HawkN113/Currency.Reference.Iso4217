using Currency.Reference.Iso4217;
using Currency.Reference.Iso4217.Abstractions;
using Currency.Reference.Iso4217.Domain.Models;
using Currency.Reference.Iso4217.Extensions;
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
    var currencyService = scope.ServiceProvider.GetRequiredService<ICurrencyService>();
    
    currencyService.Exists(CurrencyCode.AED);
    currencyService.Exists("AED");
    var valudationResult1 = currencyService.Exists(CurrencyCode.None);
    var valudationResult2 = currencyService.Exists(string.Empty);
    var result0 = currencyService.Get("AOA");
    var result1 = currencyService.Get(CurrencyCode.None);
    var result2 = currencyService.Get(CurrencyCode.AFN);
    var result3 = currencyService.GetHistorical("ESP");
    var validResult = currencyService.TryValidate("ESP", out var validateResult);
    var getAllhistorical = currencyService.GetAllHistorical();

    var currencies = currencyService?.Query()
        .Includes
        .Type(CurrencyType.Fiat)
        .Type(CurrencyType.SpecialUnit)
        .Type(CurrencyType.SpecialReserve)
        .Type(CurrencyType.PreciousMetal)
        //.Without(w => w.Codes(CurrencyCode.GBP, CurrencyCode.EUR, CurrencyCode.XUA, CurrencyCode.USD))
        .Build();
    var result = currencies!.Any(c => c.Code == "EUR");
    
    foreach (var currency in currencies!)
        Console.WriteLine($"{currency.Code} - {currency.Name}");
    
    await host.RunAsync();
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}