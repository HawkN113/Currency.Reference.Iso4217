using Currency.Reference.Iso4217.Abstractions;
using Currency.Reference.Iso4217.Handlers;
using Currency.Reference.Iso4217.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Currency.Reference.Iso4217.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCurrencyService(this IServiceCollection services)
    {
        if (services.All(sd => sd.ServiceType != typeof(ICurrencyLoader)))
            services.AddSingleton<ICurrencyLoader, CurrencyLoader>();
        if (services.All(sd => sd.ServiceType != typeof(ICurrencyService)))
            services.AddSingleton<ICurrencyService, CurrencyService>();
        return services;
    }
}