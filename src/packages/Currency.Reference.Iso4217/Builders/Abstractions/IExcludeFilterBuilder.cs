namespace Currency.Reference.Iso4217.Builders.Abstractions;

public interface IExcludeFilterBuilder
{
    IExcludeFilterBuilder Codes(params string[] codes);
    IExcludeFilterBuilder Names(params string[] names);
    IExcludeFilterBuilder NumericCodes(params int[] numericCodes);
}