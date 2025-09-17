using Currency.Reference.Iso4217.Common.Models;
namespace Currency.Reference.Iso4217.Abstractions;

internal interface ICurrencyLoader
{
    List<CurrencyInfo> Currencies { get; }
    CurrencyType GetCurrencyType(string code);
}