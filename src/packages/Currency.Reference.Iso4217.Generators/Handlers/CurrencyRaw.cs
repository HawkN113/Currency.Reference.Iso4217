namespace Currency.Reference.Iso4217.Generators.Handlers;

internal sealed class CurrencyRaw
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string NumericCode { get; set; } = string.Empty;
    public string WithdrawalDate { get; set; } = string.Empty;
}