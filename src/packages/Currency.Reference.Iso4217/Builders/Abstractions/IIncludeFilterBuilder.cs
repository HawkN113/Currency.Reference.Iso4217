namespace Currency.Reference.Iso4217.Builders.Abstractions;

/// <summary>
/// Builder interface for including specific currencies by codes, names, or numeric codes.
/// </summary>
public interface IIncludeFilterBuilder
{
    IIncludeFilterBuilder Codes(params string[] codes);
    IIncludeFilterBuilder Names(params string[] names);
    IIncludeFilterBuilder NumericCodes(params int[] numericCodes);
}