namespace Currency.Reference.Iso4217.Builders.Abstractions;

public interface IIncludeFilterBuilder
{
    IIncludeFilterBuilder Codes(params string[] codes);
    IIncludeFilterBuilder Names(params string[] names);
    IIncludeFilterBuilder NumericCodes(params int[] numericCodes);
}