using Currency.Reference.Iso4217.Abstractions;
using Currency.Reference.Iso4217.Builders;
using Currency.Reference.Iso4217.Builders.Abstractions;
using Currency.Reference.Iso4217.Data;
using Currency.Reference.Iso4217.Domain.Models;

namespace Currency.Reference.Iso4217.Services;

internal sealed class CurrencyService : ICurrencyService
{
    private readonly IReadOnlyDictionary<string, Domain.Models.Currency> _actualCurrencies =
        LocalDatabase.ActualCurrencies.ToDictionary(c => c.Code, StringComparer.OrdinalIgnoreCase);

    private readonly IReadOnlyDictionary<string, Domain.Models.Currency> _historicalCurrencies =
        LocalDatabase.HistoricalCurrencies.ToDictionary(c => c.Code, StringComparer.OrdinalIgnoreCase);

    public bool TryValidate(string value, out ValidationResult result)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            result = ValidationResult.Invalid("Value is null or empty.", ValidationType.Value);
            return false;
        }

        var isExistCurrency = Exists(value);
        result = isExistCurrency
            ? ValidationResult.Success()
            : ValidationResult.Invalid($"The currency code with value '{value}' does not exist");
        return isExistCurrency;
    }

    public bool TryValidate(CurrencyCode code, out ValidationResult result)
    {
        if (code == CurrencyCode.None)
        {
            result = ValidationResult.Invalid("Code should not be 'None'.", ValidationType.Code);
            return false;
        }

        var isExistCurrency = Exists(code);
        result = isExistCurrency
            ? ValidationResult.Success()
            : ValidationResult.Invalid($"The currency code '{code.ToString()}' does not exist");
        return isExistCurrency;
    }

    public bool Exists(string value) =>
        !string.IsNullOrWhiteSpace(value) && _actualCurrencies.ContainsKey(value.Trim());

    public bool Exists(CurrencyCode code) =>
        code != CurrencyCode.None && _actualCurrencies.ContainsKey(code.ToString());

    public Domain.Models.Currency? Get(string value)
    {
        if (!Exists(value)) return null;
        _actualCurrencies.TryGetValue(value, out var currency);
        return currency;
    }

    public Domain.Models.Currency? Get(CurrencyCode code)
    {
        if (!Exists(code)) return null;
        _actualCurrencies.TryGetValue(code.ToString(), out var currency);
        return currency;
    }

    public Domain.Models.Currency? GetHistorical(string value)
    {
        var isExist = !string.IsNullOrWhiteSpace(value) && _historicalCurrencies.ContainsKey(value.Trim());
        if (!isExist) return null;
        _historicalCurrencies.TryGetValue(value.Trim(), out var currency);
        return currency;
    }

    public Domain.Models.Currency[] GetAllHistorical()
    {
        return _historicalCurrencies.Values.ToArray();
    }

    public ICurrencyQueryStart Query()
    {
        return new CurrencyQueryBuilder(LocalDatabase.ActualCurrencies);
    }
}