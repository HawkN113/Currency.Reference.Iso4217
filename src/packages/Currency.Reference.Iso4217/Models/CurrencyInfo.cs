namespace Currency.Reference.Iso4217.Models;

/// <summary>
/// 
/// </summary>
/// <param name="Code"></param>
/// <param name="Name"></param>
/// <param name="Country"></param>
/// <param name="NumericCode"></param>
public sealed record CurrencyInfo(
    string Code, 
    string Name, 
    string? Country, 
    string? NumericCode,
    CurrencyType CurrencyType = CurrencyType.Fiat
    );
