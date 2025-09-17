using Currency.Reference.Iso4217.Abstractions;
using Currency.Reference.Iso4217.Builders.Abstractions;
namespace Currency.Reference.Iso4217.Services;

internal sealed class CurrencyServiceSafe(ICurrencyService? inner) : ICurrencyServiceSafe
{
    public ICurrencyQueryStart Query()
    {
        if (inner is null)
            throw new InvalidOperationException(
                "CurrencyService is not registered. Call 'services.AddCurrencyService()' in your DI configuration.");
        return inner.Query();
    }
}