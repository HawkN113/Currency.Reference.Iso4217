using Currency.Reference.Iso4217.Abstractions;

namespace Currency.Reference.Iso4217.Services;

public sealed class CurrencyService : ICurrencyService
{
    public IReadOnlyCollection<CurrencyInfo> GetAll() => CurrencyIso3.All;

    public IEnumerable<CurrencyInfo> GetAllExcept(params string[] excludedCodes) =>
        CurrencyIso3.GetAllExcept(excludedCodes);

    public IEnumerable<string> GetUniqueCodesWithNames() =>
        CurrencyIso3.UniqueCodesWithNames();

    public bool IsValidCode(string code) => CurrencyIso3.IsValidCode(code);
    public bool IsValidName(string name) => CurrencyIso3.IsValidName(name);
    public bool IsValidNumeric(string numericCode) => CurrencyIso3.IsValidNumeric(numericCode);

    public CurrencyInfo? GetByCode(string code) => CurrencyIso3.GetByCode(code);
    public CurrencyInfo? GetByName(string name) => CurrencyIso3.GetByName(name);
    public CurrencyInfo? GetByNumeric(string numericCode) => CurrencyIso3.GetByNumeric(numericCode);
}