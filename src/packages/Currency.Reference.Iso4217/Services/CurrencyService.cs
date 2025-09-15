using Currency.Reference.Iso4217.Abstractions;
using Currency.Reference.Iso4217.Handlers;
using Currency.Reference.Iso4217.Models;
namespace Currency.Reference.Iso4217.Services;

internal sealed class CurrencyService : ICurrencyService
{
    private readonly List<CurrencyInfo> _currencies;
    public CurrencyService()
    {
        var loadedCurrencies = JsonCurrencyHandler.LoadCurrencies();
        _currencies = new List<CurrencyInfo>(loadedCurrencies.Count);
        foreach (var item in loadedCurrencies)
            _currencies.Add(new CurrencyInfo(
                Code: item.Code,
                Name: item.Name,
                Country: item.Country,
                NumericCode: item.NumericCode)
            );
        _currencies = _currencies.OrderBy(c => c.Code).ToList();
    }
    public IReadOnlyCollection<CurrencyInfo> GetAll() => _currencies.OrderBy(c => c.Code).ToList().AsReadOnly();
    public IReadOnlyCollection<CurrencyInfo> GetAllExcept(params string[] excludedCodes)
    {
        var excluded = new HashSet<string>(excludedCodes, StringComparer.OrdinalIgnoreCase);
        return _currencies
            .Where(c => !excluded.Contains(c.Code)).ToArray().AsReadOnly();
    }
    public IEnumerable<string> GetUniqueCodesWithNames()
    {
        var result = new HashSet<string>();
        foreach (var code in _currencies.OrderBy(c => c.Code))
        {
            result.Add($"{code.Code} - {code.Name}");
        }
        return result;
    }

    public bool IsValid(string value, Field[] fields)
    {
        if (fields.Length == 0)
            throw new ArgumentException($"{nameof(fields)} must not be empty.", nameof(fields));
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{nameof(value)} must not be empty.", nameof(value));
        
        var result = false;
        foreach (var field in fields)
        {
            result = field switch
            {
                Field.Code => _currencies.Any(c =>
                    c.Code.Equals(value.Trim(), StringComparison.OrdinalIgnoreCase)),
                Field.Name => _currencies.Any(c =>
                    c.Name.Equals(value.Trim(), StringComparison.OrdinalIgnoreCase)),
                Field.NumericCode => _currencies.Any(c => c.NumericCode == value.Trim()),
                _ => result
            };
            if (result)
                return true;
        }
        return result;
    }

    public CurrencyInfo? Get(string value, Field field)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{nameof(value)} must not be empty.", nameof(value));
        
        CurrencyInfo? result = null;
        return field switch
        {
            Field.Code => _currencies.FirstOrDefault(c =>
                c.Code.Equals(value.Trim(), StringComparison.OrdinalIgnoreCase)),
            Field.Name => _currencies.FirstOrDefault(c =>
                c.Name.Equals(value.Trim(), StringComparison.OrdinalIgnoreCase)),
            Field.NumericCode => _currencies.FirstOrDefault(c => c.NumericCode == value.Trim()),
            _ => result
        };
    }
}