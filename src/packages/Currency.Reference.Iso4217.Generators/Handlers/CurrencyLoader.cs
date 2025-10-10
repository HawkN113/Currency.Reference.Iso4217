using Currency.Reference.Iso4217.Generators.Models;
namespace Currency.Reference.Iso4217.Generators.Handlers;

internal class CurrencyLoader
{
    private readonly CurrencyData _actualCurrencyData = new();
    private readonly CurrencyData _historicalCurrencyData = new();

    private static readonly HashSet<string> PreciousMetalsCodes = new(StringComparer.OrdinalIgnoreCase)
        { "XAG", "XAU", "XPD", "XPT" };

    private static readonly HashSet<string> SpecialReserveCodes = new(StringComparer.OrdinalIgnoreCase)
        { "XXX", "XTS" };

    private static readonly HashSet<string> SpecialUnits = new(StringComparer.OrdinalIgnoreCase)
        { "XBA", "XBB", "XBC", "XBD", "XSU", "XUA" };
    
    private static readonly HashSet<string> ExcludedCodes = new(StringComparer.OrdinalIgnoreCase)
        { "VED", "XAD", "XCG", "ZWG" };
    
    public CurrencyData ActualCurrencyData => _actualCurrencyData;
    public CurrencyData HistoricalCurrencyData => _historicalCurrencyData;

    public CurrencyLoader(string originalJson, string replacementJson, string historicalJson)
    {
        var actualCurrencyData = new JsonCurrencyHandler(originalJson).LoadActualCurrencies();
        var replacements = new JsonReplacementCurrencyHandler(replacementJson).LoadNameReplacements();
        var historicalCurrencyData = new JsonHistoricalCurrencyHandler(historicalJson).LoadHistoricalCurrencies();
        _actualCurrencyData!.Currencies = new List<Models.Currency>(actualCurrencyData.Currencies.Count);
        _historicalCurrencyData!.Currencies = new List<Models.Currency>(historicalCurrencyData.Currencies.Count);

        _actualCurrencyData.PublishedDate = actualCurrencyData.PublishedDate;
        foreach (var item in actualCurrencyData.Currencies.Where(c => !ExcludedCodes.Contains(c.Code)))
        {
            _actualCurrencyData.Currencies.Add(new Models.Currency()
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

        _historicalCurrencyData.PublishedDate = historicalCurrencyData.PublishedDate;
        foreach (var item in historicalCurrencyData.Currencies.Where(c => !ExcludedCodes.Contains(c.Code)))
        {
            _historicalCurrencyData.Currencies.Add(new Models.Currency()
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

        _actualCurrencyData.Currencies = _actualCurrencyData.Currencies.OrderBy(c => c.Code).ToList();
        _historicalCurrencyData.Currencies = _historicalCurrencyData.Currencies.OrderBy(c => c.Code).ToList();
    }

    private static CurrencyType GetCurrencyType(string code)
    {
        if (PreciousMetalsCodes.Contains(code))
            return CurrencyType.PreciousMetal;
        if (SpecialReserveCodes.Contains(code))
            return CurrencyType.SpecialReserve;
        return SpecialUnits.Contains(code) ? CurrencyType.SpecialUnit : CurrencyType.Fiat;
    }
}