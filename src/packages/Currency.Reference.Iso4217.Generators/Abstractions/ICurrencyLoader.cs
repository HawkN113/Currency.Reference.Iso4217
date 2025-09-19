using System.Collections.Generic;
using Currency.Reference.Iso4217.Generators.Models;
namespace Currency.Reference.Iso4217.Generators.Abstractions;

internal interface ICurrencyLoader
{
    List<CurrencyInfo> Currencies { get; }
    CurrencyType GetCurrencyType(string code);
}