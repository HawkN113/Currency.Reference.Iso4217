using System.Text.RegularExpressions;
using Currency.Reference.Iso4217.Generators.Extensions;
namespace Currency.Reference.Iso4217.Generators.Handlers;

internal sealed class JsonHistoricalCurrencyHandler(string jsonContent)
{
    public List<CurrencyRaw> LoadHistoricalCurrencies()
    {
        var matches = Regex.Matches(jsonContent, @"\{([^}]*)\}");
        var currencies = (from Match match in matches
            select match.Groups[1].Value
            into obj
            let code = JsonExtensions.Extract(obj, "Ccy")
            let name = JsonExtensions.Extract(obj, "CcyNm")
            let country = JsonExtensions.Extract(obj, "CtryNm")
            let num = JsonExtensions.Extract(obj, "CcyNbr")
            let wthdrwlDt = JsonExtensions.Extract(obj, "WthdrwlDt")

            where !string.IsNullOrEmpty(code)
            select new CurrencyRaw
            {
                Code = !string.IsNullOrEmpty(code) ? code : string.Empty,
                Name = !string.IsNullOrEmpty(name) ? name : string.Empty,
                Country = !string.IsNullOrEmpty(country) ? country : string.Empty,
                NumericCode = !string.IsNullOrEmpty(num) ? num : null,
                WithdrawalDate = !string.IsNullOrEmpty(wthdrwlDt) ? wthdrwlDt : null
            }).ToList();

        return currencies
            .GroupBy(c => c.Code)
            .Select(g => g.First())
            .ToList();
    }
}