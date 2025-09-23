using Currency.Reference.Iso4217.Generators.Models;
namespace Currency.Reference.Iso4217.Generators.Handlers;

internal class CurrencyLoader
{
    private readonly List<CurrencyInfo> _currencies;

    private static readonly HashSet<string> PreciousMetalsCodes = new(StringComparer.OrdinalIgnoreCase)
        { "XAG", "XAU", "XPD", "XPT" };

    private static readonly HashSet<string> SpecialReserveCodes = new(StringComparer.OrdinalIgnoreCase)
        { "XXX", "XTS" };

    private static readonly HashSet<string> SpecialUnits = new(StringComparer.OrdinalIgnoreCase)
        { "XBA", "XBB", "XBC", "XBD", "XSU", "XUA" };
    
    private static readonly HashSet<string> ExcludedCodes = new(StringComparer.OrdinalIgnoreCase)
        { "VED", "XAD", "XCG", "ZWG" };
    
    public List<CurrencyInfo> Currencies => _currencies;

    public CurrencyLoader(string originalJson, string replacementJson, string historicalJson)
    {
        var actualCurrencies = new JsonCurrencyHandler(originalJson).LoadActualCurrencies();
        var replacements = new JsonReplacementCurrencyHandler(replacementJson).LoadNameReplacements();
        //var historicalCurrencies = new JsonHistoricalCurrencyHandler(historicalJson).LoadHistoricalCurrencies();
        _currencies = new List<CurrencyInfo>(actualCurrencies.Count);
        
        foreach (var item in actualCurrencies.Where(c => !ExcludedCodes.Contains(c.Code)))
        {
            _currencies.Add(new CurrencyInfo()
            {
                Code = item.Code,
                Name = replacements.TryGetValue(item.Code, out var newName) ? newName : item.Name,
                Country = item.Country,
                NumericCode = item.NumericCode,
                CurrencyType = GetCurrencyType(item.Code),
                IsHistoric = !string.IsNullOrEmpty(item.WithdrawalDate), 
                WithdrawalDate = item.WithdrawalDate 
            });
        }
        /*
        foreach (var item in historicalCurrencies.Where(c => !ExcludedCodes.Contains(c.Code)))
        {
            if (_currencies.Any(x => x.Code == item.Code))
                continue;
            _currencies.Add(new CurrencyInfo()
            {
                Code = item.Code,
                Name = replacements.TryGetValue(item.Code, out var newName) ? newName : item.Name,
                Country = item.Country,
                NumericCode = item.NumericCode,
                CurrencyType = GetCurrencyType(item.Code),
                IsHistoric = !string.IsNullOrEmpty(item.WithdrawalDate),
                WithdrawalDate = item.WithdrawalDate
            });
        }
        */
        
        _currencies = _currencies.OrderBy(c => c.Code).ToList();
    }

    public CurrencyType GetCurrencyType(string code)
    {
        if (PreciousMetalsCodes.Contains(code))
            return CurrencyType.PreciousMetal;
        if (SpecialReserveCodes.Contains(code))
            return CurrencyType.SpecialReserve;
        return SpecialUnits.Contains(code) ? CurrencyType.SpecialUnit : CurrencyType.Fiat;
    }
}