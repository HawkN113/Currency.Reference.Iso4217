namespace Currency.Reference.Iso4217.Builders.Abstractions;

/// <summary>
/// Builder interface for excluding specific currencies by codes, names, or numeric codes.
/// </summary>
public interface IExcludeFilterBuilder
{
    IExcludeFilterBuilder Codes(params string[] codes);
    IExcludeFilterBuilder Names(params string[] names);
    IExcludeFilterBuilder NumericCodes(params int[] numericCodes);
}