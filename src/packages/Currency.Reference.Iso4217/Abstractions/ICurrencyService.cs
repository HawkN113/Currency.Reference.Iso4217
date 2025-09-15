using Currency.Reference.Iso4217.Models;

namespace Currency.Reference.Iso4217.Abstractions;

public interface ICurrencyService
{
    IReadOnlyCollection<CurrencyInfo> GetAll();
    IReadOnlyCollection<CurrencyInfo> GetAllExcept(params string[] excludedCodes);
    IEnumerable<string> GetUniqueCodesWithNames();
    bool IsValid(string value, Field[] fields);
    CurrencyInfo? Get(string value, Field field);
}