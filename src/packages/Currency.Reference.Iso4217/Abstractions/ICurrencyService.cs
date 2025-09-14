namespace Currency.Reference.Iso4217.Abstractions;

public interface ICurrencyService
{
    IReadOnlyCollection<CurrencyInfo> GetAll();
    IEnumerable<CurrencyInfo> GetAllExcept(params string[] excludedCodes);
    IEnumerable<string> GetUniqueCodesWithNames();

    bool IsValidCode(string code);
    bool IsValidName(string name);
    bool IsValidNumeric(string numericCode);

    CurrencyInfo? GetByCode(string code);
    CurrencyInfo? GetByName(string name);
    CurrencyInfo? GetByNumeric(string numericCode);
}