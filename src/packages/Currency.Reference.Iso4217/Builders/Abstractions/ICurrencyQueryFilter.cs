namespace Currency.Reference.Iso4217.Builders.Abstractions;

public interface ICurrencyQueryFilter : ICurrencyQueryFinal
{
    ICurrencyQueryFilter With(Action<IIncludeFilterBuilder> configure);
    ICurrencyQueryFilter Without(Action<IExcludeFilterBuilder> configure);
    
}