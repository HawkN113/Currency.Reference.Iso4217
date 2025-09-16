using Currency.Reference.Iso4217.Builders;
using Currency.Reference.Iso4217.Models;

namespace Currency.Reference.Iso4217.Abstractions;

public interface ICurrencyService
{
    //IReadOnlyCollection<CurrencyInfo> GetAll();
    //IReadOnlyCollection<CurrencyInfo> GetAllExcept(params string[] excludedCodes);
    //IReadOnlyCollection<CurrencyInfo> GetFiatCurrencies();
    //IReadOnlyCollection<CurrencyInfo> GetPreciousMetals();
    //IReadOnlyCollection<CurrencyInfo> GetSpecialReserveCodes();
    //IReadOnlyCollection<CurrencyInfo> GetSpecialUnits();
    //IEnumerable<string> GetUniqueCodesWithNames();
    bool IsValid(string value, CriteriaField[] fields, CurrencyType? type = null);
    CurrencyInfo? Get(string value, CriteriaField field, CurrencyType? type = null);
    CurrencyQueryBuilder Query();
}