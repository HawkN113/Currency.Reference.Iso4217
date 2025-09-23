using System.Text.RegularExpressions;
namespace Currency.Reference.Iso4217.Generators.Handlers;

internal sealed class JsonHistoricalCurrencyHandler(string jsonContent)
{
    public List<CurrencyRaw> LoadHistoricalCurrencies()
    {
        var matches = Regex.Matches(jsonContent, @"\{([^}]*)\}");
        var currencies = (from Match match in matches
        select match.Groups[1].Value
        into obj
        let code = Extract(obj, "Ccy")
        let name = Extract(obj, "CcyNm")
        let country = Extract(obj, "CtryNm")
        let num = Extract(obj, "CcyNbr")
        let wthdrwlDt = Extract(obj, "WthdrwlDt")
        
        where !string.IsNullOrEmpty(code)
        select new CurrencyRaw
        {
            Code = code, 
            Name = name, 
            Country = country, 
            NumericCode = num, 
            WithdrawalDate = wthdrwlDt
        }).ToList();

        return currencies
            .GroupBy(c => c.Code)
            .Select(g => g.First())
            .ToList();
    }
    private static string Extract(string json, string key)
    {
        var pattern = $"\"{Regex.Escape(key)}\"\\s*:\\s*\"([^\"]+)\"";
        var match = Regex.Match(json, pattern);
        return match.Success ? match.Groups[1].Value : string.Empty;
    }
}