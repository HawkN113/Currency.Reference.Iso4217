using Currency.Reference.Iso4217.Builders.Abstractions;
using Currency.Reference.Iso4217.Models;

namespace Currency.Reference.Iso4217.Builders;

internal sealed class CurrencyQueryBuilder : 
    ICurrencyQueryStart,
    ICurrencyQueryFilter,
    IExcludeFilterBuilder,
    IIncludeFilterBuilder
{
    private readonly IReadOnlyCollection<CurrencyInfo> _allCurrencies;
    private readonly HashSet<CurrencyType> _includedTypes = new();
    private readonly HashSet<string> _withCodes = new(StringComparer.OrdinalIgnoreCase);
    private readonly HashSet<string> _withoutCodes = new(StringComparer.OrdinalIgnoreCase);
    private readonly HashSet<string> _withNames = new(StringComparer.OrdinalIgnoreCase);
    private readonly HashSet<string> _withoutNames = new(StringComparer.OrdinalIgnoreCase);
    private readonly HashSet<string> _withNumericCodes = new(StringComparer.OrdinalIgnoreCase);
    private readonly HashSet<string> _withoutNumericCodes = new(StringComparer.OrdinalIgnoreCase);

    public CurrencyQueryBuilder(IReadOnlyCollection<CurrencyInfo> currencies)
    {
        _allCurrencies = currencies;
        Includes = this;
    }

    public ICurrencyQueryFilter Includes { get; }

    public ICurrencyQueryFilter Types(params CurrencyType[] types)
    {
        foreach (var t in types)
            _includedTypes.Add(t);

        return this;
    }

    public ICurrencyQueryFilter Without(Action<IExcludeFilterBuilder> configure)
    {
        configure(this);
        return this;
    }

    public ICurrencyQueryFilter With(Action<IIncludeFilterBuilder> configure)
    {
        configure(this);
        return this;
    }

    public IReadOnlyCollection<CurrencyInfo> Build()
    {
        var query = _allCurrencies
            .Where(c => _includedTypes.Count == 0 || _includedTypes.Contains(c.CurrencyType));

        if (_withCodes.Count > 0)
            query = query.Where(c => _withCodes.Contains(c.Code));

        if (_withNames.Count > 0)
            query = query.Where(c => _withNames.Contains(c.Name));

        if (_withNumericCodes.Count > 0)
            query = query.Where(c => c.NumericCode != null && _withNumericCodes.Contains(c.NumericCode));

        if (_withoutCodes.Count > 0)
            query = query.Where(c => !_withoutCodes.Contains(c.Code));

        if (_withoutNames.Count > 0)
            query = query.Where(c => !_withoutNames.Contains(c.Name));

        if (_withoutNumericCodes.Count > 0)
            query = query.Where(c => c.NumericCode != null && !_withoutNumericCodes.Contains(c.NumericCode));

        return query.ToList();
    }
    
    IExcludeFilterBuilder IExcludeFilterBuilder.Codes(params string[] codes)
    {
        foreach (var code in codes)
            _withoutCodes.Add(code);
        return this;
    }

    IExcludeFilterBuilder IExcludeFilterBuilder.Names(params string[] names)
    {
        foreach (var name in names)
            _withoutNames.Add(name);
        return this;
    }

    IExcludeFilterBuilder IExcludeFilterBuilder.NumericCodes(params int[] numericCodes)
    {
        foreach (var nc in numericCodes)
            _withoutNumericCodes.Add(nc.ToString());
        return this;
    }
    
    IIncludeFilterBuilder IIncludeFilterBuilder.Codes(params string[] codes)
    {
        foreach (var code in codes)
            _withCodes.Add(code);
        return this;
    }

    IIncludeFilterBuilder IIncludeFilterBuilder.Names(params string[] names)
    {
        foreach (var name in names)
            _withNames.Add(name);
        return this;
    }

    IIncludeFilterBuilder IIncludeFilterBuilder.NumericCodes(params int[] numericCodes)
    {
        foreach (var nc in numericCodes)
            _withNumericCodes.Add(nc.ToString());
        return this;
    }
}