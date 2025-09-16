namespace Currency.Reference.Iso4217.Builders.Abstractions;

public interface ICurrencyQueryStart
{
    ICurrencyQueryFilter Includes { get; }
}