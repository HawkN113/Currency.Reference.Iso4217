namespace Currency.Reference.Iso4217.Domain.Entities;

public abstract class Criteria
{
    public Func<Currency, bool> Predicate { get; set; } = _ => true;
}