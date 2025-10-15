using Currency.Reference.Iso4217.Builders.Abstractions;
using Currency.Reference.Iso4217.Models;
namespace Currency.Reference.Iso4217.Builders;

internal sealed class CurrencyQueryBuilder:
    ICurrencyQueryStart,
    ICurrencyQueryTypeSelector,
    ICurrencyQueryFilter,
    IIncludeFilterBuilder,
    IExcludeFilterBuilder
{
    private readonly IReadOnlyList<Models.Currency> _actualCurrencies;
    private readonly HashSet<CurrencyType> _includedTypes = [];
    private readonly HashSet<string> _withCodes = new(StringComparer.OrdinalIgnoreCase);
    private readonly HashSet<string> _withoutCodes = new(StringComparer.OrdinalIgnoreCase);
    private readonly HashSet<string> _withNames = new(StringComparer.OrdinalIgnoreCase);
    private readonly HashSet<string> _withoutNames = new(StringComparer.OrdinalIgnoreCase);
    private readonly HashSet<string> _withNumericCodes = new(StringComparer.OrdinalIgnoreCase);
    private readonly HashSet<string> _withoutNumericCodes = new(StringComparer.OrdinalIgnoreCase);

    public CurrencyQueryBuilder(IReadOnlyList<Models.Currency> currencies)
    {
        _actualCurrencies = currencies;
        Includes = this;
    }

    public ICurrencyQueryTypeSelector Includes { get; }
    
    public ICurrencyQueryFilter Type(CurrencyType type)
    {
        if (!_includedTypes.Add(type))
            throw new InvalidOperationException($"CurrencyType '{type}' is already included.");
        return this;
    }

    public ICurrencyQueryFilter With(Action<IIncludeFilterBuilder> configure)
    {
        configure(this);
        return this;
    }

    public ICurrencyQueryFilter Without(Action<IExcludeFilterBuilder> configure)
    {
        configure(this);
        return this;
    }

    public IReadOnlyList<Models.Currency> Build()
    {
        Func<Models.Currency, bool> filter = c =>
            _includedTypes.Contains(c.CurrencyType!.Value) &&
            (_withCodes.Count == 0 || _withCodes.Contains(c.Code)) &&
            (_withoutCodes.Count == 0 || !_withoutCodes.Contains(c.Code)) &&
            (_withNames.Count == 0 || _withNames.Contains(c.Name)) &&
            (_withoutNames.Count == 0 || !_withoutNames.Contains(c.Name)) &&
            (_withNumericCodes.Count == 0 || (c.NumericCode != null && _withNumericCodes.Contains(c.NumericCode!))) &&
            (_withoutNumericCodes.Count == 0 || (c.NumericCode != null && !_withoutNumericCodes.Contains(c.NumericCode!)));

        return _actualCurrencies.Where(filter).ToList();
    }
    
    IIncludeFilterBuilder IIncludeFilterBuilder.Codes(params string[] codes)
    {
        foreach (var code in codes) _withCodes.Add(code);
        return this;
    }

    IIncludeFilterBuilder IIncludeFilterBuilder.Names(params string[] names)
    {
        foreach (var name in names) _withNames.Add(name);
        return this;
    }

    IIncludeFilterBuilder IIncludeFilterBuilder.NumericCodes(params int[] numericCodes)
    {
        foreach (var nc in numericCodes) _withNumericCodes.Add(nc.ToString());
        return this;
    }
    
    IExcludeFilterBuilder IExcludeFilterBuilder.Codes(params string[] codes)
    {
        foreach (var code in codes) _withoutCodes.Add(code);
        return this;
    }

    IExcludeFilterBuilder IExcludeFilterBuilder.Names(params string[] names)
    {
        foreach (var name in names) _withoutNames.Add(name);
        return this;
    }

    IExcludeFilterBuilder IExcludeFilterBuilder.NumericCodes(params int[] numericCodes)
    {
        foreach (var nc in numericCodes) _withoutNumericCodes.Add(nc.ToString());
        return this;
    }
}