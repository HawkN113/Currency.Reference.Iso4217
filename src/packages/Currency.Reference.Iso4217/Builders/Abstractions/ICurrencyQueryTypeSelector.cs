using Currency.Reference.Iso4217.Models;

namespace Currency.Reference.Iso4217.Builders.Abstractions;

public interface ICurrencyQueryTypeSelector
{
    ICurrencyQueryFilter Types(params CurrencyType[] types);
}