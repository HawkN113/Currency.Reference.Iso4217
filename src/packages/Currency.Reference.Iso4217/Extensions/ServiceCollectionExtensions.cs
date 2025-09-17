using Currency.Reference.Iso4217.Abstractions;
using Currency.Reference.Iso4217.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Currency.Reference.Iso4217.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCurrencyService(this IServiceCollection services)
    {
        services.TryAddSingleton<ICurrencyService, CurrencyService>();
        services.TryAddSingleton<ICurrencyServiceSafe>(sp =>
        {
            var inner = sp.GetRequiredService<ICurrencyService>();
            return new CurrencyServiceSafe(inner);
        });
        
        return services;
    }

    public static ICurrencyServiceSafe? GetCurrencyService(this IServiceProvider sp, bool required = true)
    {
        var service = sp.GetService<ICurrencyServiceSafe>();
        if (service is null && required)
        {
            throw new InvalidOperationException(
                "CurrencyService is not registered. Call 'services.AddCurrencyServices()' in your DI configuration.");
        }
        return service;
    }
}