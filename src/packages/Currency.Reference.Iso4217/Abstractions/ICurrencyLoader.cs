using Currency.Reference.Iso4217.Models;

namespace Currency.Reference.Iso4217.Abstractions;

internal interface ICurrencyLoader
{
    List<CurrencyInfo> Currencies { get; }
    CurrencyType GetCurrencyType(string code);
}