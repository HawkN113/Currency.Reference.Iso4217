namespace Currency.Reference.Iso4217.Common.Models;

public abstract class Criteria
{
    public Func<CurrencyInfo, bool> Predicate { get; set; } = _ => true;
}