using Currency.Reference.Iso4217.Builders.Abstractions;
using Currency.Reference.Iso4217.Common.Models;
namespace Currency.Reference.Iso4217.Builders;

internal sealed class CurrencyQueryBuilder:
    ICurrencyQueryStart,
    ICurrencyQueryTypeSelector,
    ICurrencyQueryFilter,
    IIncludeFilterBuilder,
    IExcludeFilterBuilder
{
    private readonly IReadOnlyCollection<CurrencyInfo> _allCurrencies;
    private readonly HashSet<CurrencyType> _includedTypes = [];
    private readonly HashSet<CurrencyCode> _withCodes = [];
    private readonly HashSet<CurrencyCode> _withoutCodes = [];
    private readonly HashSet<string> _withNames = new(StringComparer.OrdinalIgnoreCase);
    private readonly HashSet<string> _withoutNames = new(StringComparer.OrdinalIgnoreCase);
    private readonly HashSet<string> _withNumericCodes = new(StringComparer.OrdinalIgnoreCase);
    private readonly HashSet<string> _withoutNumericCodes = new(StringComparer.OrdinalIgnoreCase);

    public CurrencyQueryBuilder(IReadOnlyCollection<CurrencyInfo> currencies)
    {
        _allCurrencies = currencies;
        Includes = this;
    }

    public ICurrencyQueryTypeSelector Includes { get; }
    
    public ICurrencyQueryFilter Type(CurrencyType type)
    {
        if (!_includedTypes.Add(type))
            throw new InvalidOperationException($"CurrencyType {type} is already included.");
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

    public IReadOnlyCollection<CurrencyInfo> Build()
    {
        var query = _allCurrencies.Where(c => _includedTypes.Contains(c.CurrencyType));
        
        if (_withCodes.Count > 0)
            query = query.Where(c => _withCodes.Contains(Enum.Parse<CurrencyCode>(c.Code)));
        if (_withoutCodes.Count > 0)
            query = query.Where(c => !_withoutCodes.Contains(Enum.Parse<CurrencyCode>(c.Code)));
        if (_withNames.Count > 0)
            query = query.Where(c => _withNames.Contains(c.Name));
        if (_withoutNames.Count > 0)
            query = query.Where(c => !_withoutNames.Contains(c.Name));
        if (_withNumericCodes.Count > 0)
            query = query.Where(c => c.NumericCode is not null && _withNumericCodes.Contains(c.NumericCode!));
        if (_withoutNumericCodes.Count > 0)
            query = query.Where(c => c.NumericCode is not null && !_withoutNumericCodes.Contains(c.NumericCode!));
        
        return query.ToList();
    }
    
    IIncludeFilterBuilder IIncludeFilterBuilder.Codes(params CurrencyCode[] codes)
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
    
    IExcludeFilterBuilder IExcludeFilterBuilder.Codes(params CurrencyCode[] codes)
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