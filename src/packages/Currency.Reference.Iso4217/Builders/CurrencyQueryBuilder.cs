using Currency.Reference.Iso4217.Models;

namespace Currency.Reference.Iso4217.Builders;

public sealed class CurrencyQueryBuilder
{
    private readonly List<CurrencyInfo> _source;
    private bool _includeFiats;
    private bool _includeMetals;
    private bool _includeSpecialReserves;
    private bool _includeSpecialUnits;
    private HashSet<string>? _codes;

    internal CurrencyQueryBuilder(List<CurrencyInfo> source)
    {
        _source = source;
    }

    public CurrencyQueryBuilder IncludesFiats()
    {
        _includeFiats = true;
        return this;
    }

    public CurrencyQueryBuilder IncludesMetals()
    {
        _includeMetals = true;
        return this;
    }

    public CurrencyQueryBuilder IncludesSpecialReserves()
    {
        _includeSpecialReserves = true;
        return this;
    }

    public CurrencyQueryBuilder IncludesSpecialUnits()
    {
        _includeSpecialUnits = true;
        return this;
    }

    public CurrencyQueryBuilder WithCodes(params string[] codes)
    {
        if (codes is { Length: > 0 })
            _codes = new HashSet<string>(codes, StringComparer.OrdinalIgnoreCase);
        return this;
    }

    public CurrencyQueryBuilder WithCodeContains(string substring)
    {
        if (!string.IsNullOrWhiteSpace(substring))
            _codes = _source
                .Where(c => c.Code.Contains(substring, StringComparison.OrdinalIgnoreCase))
                .Select(c => c.Code)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);
        return this;
    }

    public IReadOnlyCollection<CurrencyInfo> Build()
    {
        var result = _source.Where(c =>
            (_includeFiats && c.CurrencyType == CurrencyType.Fiat) ||
            (_includeMetals && c.CurrencyType == CurrencyType.PreciousMetal) ||
            (_includeSpecialReserves && c.CurrencyType == CurrencyType.SpecialReserve) ||
            (_includeSpecialUnits && c.CurrencyType == CurrencyType.SpecialUnit)
        );

        if (_codes is { Count: > 0 })
            result = result.Where(c => _codes.Contains(c.Code));

        return result.OrderBy(c => c.Code).ToList().AsReadOnly();
    }
}