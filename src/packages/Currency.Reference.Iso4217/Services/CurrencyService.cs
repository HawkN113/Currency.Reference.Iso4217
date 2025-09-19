using Currency.Reference.Iso4217.Abstractions;
using Currency.Reference.Iso4217.Builders;
using Currency.Reference.Iso4217.Builders.Abstractions;
using Currency.Reference.Iso4217.Common.Models;
namespace Currency.Reference.Iso4217.Services;

internal sealed class CurrencyService : ICurrencyService
{
    private readonly IReadOnlyList<CurrencyInfo> _currencies = LocalDatabase.Currencies;

    public bool IsValid(string value, Criteria[] criteria, CurrencyType? type = null)
    {
        if (criteria.Length == 0)
            throw new ArgumentException($"{nameof(criteria)} must not be empty.", nameof(criteria));
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{nameof(value)} must not be empty.", nameof(value));

        var result = false;
        var trimmed = value.Trim();

        foreach (var field in criteria)
        {
            result = field switch
            {
                Criteria.Code => _currencies.Any(c =>
                    c.Code.Equals(trimmed, StringComparison.OrdinalIgnoreCase)),
                Criteria.Name => _currencies.Any(c =>
                    c.Name.Equals(trimmed, StringComparison.OrdinalIgnoreCase)),
                Criteria.NumericCode => _currencies.Any(c => c.NumericCode == trimmed),
                Criteria.CurrencyType => type.HasValue && _currencies.Any(c =>
                    c.CurrencyType == type.Value && c.Code.Equals(trimmed, StringComparison.OrdinalIgnoreCase)),
                _ => result
            };
            if (result)
                return true;
        }
        return result;
    }

    public CurrencyInfo? Get(string value, Criteria criteria, CurrencyType? type = null)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{nameof(value)} must not be empty.", nameof(value));

        CurrencyInfo? result = null;
        var trimmed = value.Trim();
        return criteria switch
        {
            Criteria.Code => _currencies.FirstOrDefault(c =>
                c.Code.Equals(trimmed, StringComparison.OrdinalIgnoreCase)),
            Criteria.Name => _currencies.FirstOrDefault(c =>
                c.Name.Equals(trimmed, StringComparison.OrdinalIgnoreCase)),
            Criteria.NumericCode => _currencies.FirstOrDefault(c => c.NumericCode == trimmed),
            Criteria.CurrencyType => type.HasValue
                ? _currencies.FirstOrDefault(c => c.CurrencyType == type.Value && c.Code.Equals(trimmed, StringComparison.OrdinalIgnoreCase))
                : null,
            _ => result
        };
    }
    public ICurrencyQueryStart Query()
    {
        return new CurrencyQueryBuilder(_currencies);
    }
}