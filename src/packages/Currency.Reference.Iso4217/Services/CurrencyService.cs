using Currency.Reference.Iso4217.Abstractions;
using Currency.Reference.Iso4217.Builders;
using Currency.Reference.Iso4217.Builders.Abstractions;
using Currency.Reference.Iso4217.Domain.Entities;
using Currency.Reference.Iso4217.Data;
namespace Currency.Reference.Iso4217.Services;

internal sealed class CurrencyService : ICurrencyService
{
    private readonly IReadOnlyList<Domain.Entities.Currency> _currencies = LocalDatabase.ActualCurrencies;
    private readonly IReadOnlyList<Domain.Entities.Currency> _historicalCurrencies = LocalDatabase.HistoricalCurrencies;

    public bool IsValid(string value, params Criteria[] criteria)
    {
        var currency = _currencies.FirstOrDefault(c =>
            string.Equals(c.Code.ToString(), value, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(c.Name, value, StringComparison.OrdinalIgnoreCase));

        return currency != null && criteria.All(c => c.Predicate(currency));
    }

    public bool IsValid(CurrencyCode code, params Criteria[] criteria)
    {
        var currency = _currencies.FirstOrDefault(c => c.Code == code.ToString());
        return currency != null && criteria.All(c => c.Predicate(currency));
    }
    
    public Domain.Entities.Currency? Get(string value, params Criteria[] criteria)
    {
        var currency = _currencies.FirstOrDefault(c =>
            string.Equals(c.Code.ToString(), value, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(c.Name, value, StringComparison.OrdinalIgnoreCase));

        if (currency == null) return null;
        return criteria.All(c => c.Predicate(currency)) ? currency : null;
    }

    public Domain.Entities.Currency? Get(CurrencyCode code, params Criteria[] criteria)
    {
        var currency = _currencies.FirstOrDefault(c => c.Code == code.ToString());
        if (currency == null) return null;
        return criteria.All(c => c.Predicate(currency)) ? currency : null;
    }
    
    public ICurrencyQueryStart Query()
    {
        return new CurrencyQueryBuilder(_currencies, _historicalCurrencies);
    }
}