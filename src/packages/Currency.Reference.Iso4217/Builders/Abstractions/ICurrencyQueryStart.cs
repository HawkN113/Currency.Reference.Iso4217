namespace Currency.Reference.Iso4217.Builders.Abstractions;

public interface ICurrencyQueryStart
{
    ICurrencyQueryTypeSelector Includes { get; }
}