namespace Currency.Reference.Iso4217.Domain.Entities;

/// <summary>
/// 
/// </summary>
/// <param name="Code"></param>
/// <param name="Name"></param>
/// <param name="Country"></param>
/// <param name="NumericCode"></param>
public sealed record Currency(
    string Code,
    string Name,
    string? CountryName,
    string? NumericCode,
    bool IsHistoric,
    DateOnly? WithdrawalDate,
    CurrencyType CurrencyType = CurrencyType.Fiat);