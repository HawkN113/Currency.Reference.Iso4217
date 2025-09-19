using Currency.Reference.Iso4217.Abstractions;
using Currency.Reference.Iso4217.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
namespace Currency.Reference.Iso4217.Extensions;

/// <summary>
/// 
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddCurrencyService(this IServiceCollection services)
    {
        services.TryAddSingleton<ICurrencyService, CurrencyService>();
        return services;
    }
}