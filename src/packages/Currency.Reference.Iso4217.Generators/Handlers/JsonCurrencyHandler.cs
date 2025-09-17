using System.Text.RegularExpressions;
namespace Currency.Reference.Iso4217.Generators.Handlers;

internal class JsonCurrencyHandler(string jsonContent)
{
    public List<CurrencyRaw> LoadCurrencies()
    {
        var currencies = new List<CurrencyRaw>();
        var matches = Regex.Matches(jsonContent, @"\{([^}]*)\}");
        foreach (Match match in matches)
        {
            var obj = match.Groups[1].Value;

            var code   = Extract(obj, "Ccy");
            var name   = Extract(obj, "CcyNm");
            var country= Extract(obj, "CtryNm");
            var num    = Extract(obj, "CcyNbr");
            //var minor  = Extract(obj, "CcyMnrUnts");

            if (!string.IsNullOrEmpty(code))
            {
                currencies.Add(new CurrencyRaw
                {
                    Code = code,
                    Name = name,
                    Country = country,
                    NumericCode = num,
                });
            }
        }

        return currencies
            .GroupBy(c => c.Code)
            .Select(g => g.First())
            .ToList();
    }
    private static string Extract(string json, string key)
    {
        var pattern = $"\"{Regex.Escape(key)}\"\\s*:\\s*\"([^\"]+)\"";
        var match = Regex.Match(json, pattern);
        return match.Success ? match.Groups[1].Value : "";
    }
}