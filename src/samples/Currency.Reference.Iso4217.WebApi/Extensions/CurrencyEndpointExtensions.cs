using Currency.Reference.Iso4217.WebApi.Handlers;
namespace Currency.Reference.Iso4217.WebApi.Extensions;

/// <summary>
/// Extension for currency-related endpoints.
/// </summary>
static class CurrencyEndpointExtensions
{
    public static IEndpointRouteBuilder MapCurrencyEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/fiatCurrencies", CurrencyHandler.GetFiatCurrencies)
            .WithName("GetFiatCurrencies")
            .WithOpenApi(o =>
            {
                o.Summary = "Get actual fiat currencies (EUR, USD, etc.)";
                o.Description = "Returns a short code and names.";
                return o;
            });

        app.MapGet("/fiatCurrency", CurrencyHandler.GetFiatCurrency)
            .WithName("GetFiatCurrency")
            .WithOpenApi(o =>
            {
                o.Summary = "Get fiat currency by code (EUR, USD, etc.)";
                o.Description = "Returns detailed currency info.";
                return o;
            });

        app.MapGet("/preciousMetals", CurrencyHandler.GetPreciousMetals)
            .WithName("GetPreciousMetals")
            .WithOpenApi(o =>
            {
                o.Summary = "Get precious metals (XAG, XAU, XPD, XPT)";
                o.Description = "Returns a short code and names.";
                return o;
            });

        app.MapGet("/specialCurrencies", CurrencyHandler.GetSpecialCurrencies)
            .WithName("GetSpecialCurrencies")
            .WithOpenApi(o =>
            {
                o.Summary = "Get special currencies (XBA, XSU, XXX, etc.)";
                o.Description = "Returns a short code, names and type.";
                return o;
            });

        app.MapGet("/historicalCurrencies", CurrencyHandler.GetHistoricalCurrencies)
            .WithName("GetHistoricalCurrencies")
            .WithOpenApi(o =>
            {
                o.Summary = "Get historical currencies";
                o.Description = "Returns a short code, name and withdrawal date.";
                return o;
            });

        return app;
    }
}
