using Currency.Reference.Iso4217.Models;

namespace Currency.Reference.Iso4217.Builders.Abstractions;

public interface ICurrencyQueryFinal
{
    IReadOnlyCollection<CurrencyInfo> Build();
}