using System;

namespace Currency.Reference.Iso4217.Generators.Models;

internal sealed class CurrencyInfo
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Country { get; set; } 
    public string? NumericCode { get; set; }
    public CurrencyType CurrencyType { get; set; } = CurrencyType.Fiat;
}
