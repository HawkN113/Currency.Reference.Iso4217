using Currency.Reference.Iso4217.Models;

namespace Currency.Reference.Iso4217.Builders.Abstractions;

public interface ICurrencyQueryFilter : IBuildQuery
{
    ICurrencyQueryFilter Types(params CurrencyType[] types);
    ICurrencyQueryFilter Without(Action<IExcludeFilterBuilder> configure);
    ICurrencyQueryFilter With(Action<IIncludeFilterBuilder> configure);
}