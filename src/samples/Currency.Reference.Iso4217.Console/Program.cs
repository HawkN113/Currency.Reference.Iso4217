using Currency.Reference.Iso4217.Abstractions;
using Currency.Reference.Iso4217.Extensions;
using Currency.Reference.Iso4217.Models;
using Currency.Reference.Iso4217.Services;
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

    var allCurrencies = currencyService.GetAll();
    var uniqueCurrencies = currencyService.GetUniqueCodesWithNames();
    var isValidEuro = currencyService.IsValid("EUR", [Field.Code]);
    var getEuro = currencyService.Get("EUR", Field.Code);
    
    foreach (var currency in uniqueCurrencies)
        Console.WriteLine(currency);
    
    /*
    foreach (var kv in CurrencyCodeExtensions.Dictionary)
    {
        Console.WriteLine($"{kv.Key} - {kv.Value.Name} ({kv.Value.Country})");
    }
    */
    
    //var eur = Currency.Reference.Iso4217.CurrencyCode.EUR;
    //var info = CurrencyCodeExtensions.Dictionary[eur];
    
    await host.RunAsync();
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}