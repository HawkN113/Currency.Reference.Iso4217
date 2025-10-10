using System.Text.RegularExpressions;
using Currency.Reference.Iso4217.Generators.Extensions;
namespace Currency.Reference.Iso4217.Generators.Handlers;

internal sealed class JsonHistoricalCurrencyHandler(string jsonContent)
{
    public CurrencyDataSet LoadHistoricalCurrencies()
    {
        var publishedDate = ExtractPublishedDate(jsonContent);
        var arrayContent = ExtractHistoricalArray(jsonContent);
        var currencies = ParseHistoricalCurrencies(arrayContent);

        return new CurrencyDataSet
        {
            PublishedDate = publishedDate,
            Currencies = currencies
                .GroupBy(c => c.Code)
                .Select(g => g.First())
                .ToList()
        };
    }

    private static string ExtractPublishedDate(string json)
    {
        var match = Regex.Match(
            json,
            @"""_Pblshd""\s*:\s*""([^""]+)""",
            RegexOptions.Compiled | RegexOptions.Singleline);

        if (!match.Success)
            throw new InvalidOperationException("Published date ('_Pblshd') is required in historical data.");

        return match.Groups[1].Value;
    }

    private static string ExtractHistoricalArray(string json)
    {
        var keyMatch = Regex.Match(
            json,
            @"""HstrcCcyNtry""\s*:\s*\[",
            RegexOptions.Compiled | RegexOptions.Singleline);

        if (!keyMatch.Success)
            throw new InvalidOperationException("The block 'HstrcCcyNtry' is required in the JSON content.");

        var arrayStart = keyMatch.Index + keyMatch.Length - 1;
        if (arrayStart < 0 || arrayStart >= json.Length || json[arrayStart] != '[')
            throw new InvalidOperationException("Cannot locate '[' for 'HstrcCcyNtry'.");

        var (arrayEnd, _) = FindJsonBlockEnd(json, arrayStart, '[', ']');
        if (arrayEnd == -1)
            throw new InvalidOperationException("Cannot locate end of 'HstrcCcyNtry' array.");

        return json.Substring(arrayStart + 1, arrayEnd - arrayStart - 1);
    }

    private static List<CurrencyRaw> ParseHistoricalCurrencies(string arrayContent)
    {
        var currencies = new List<CurrencyRaw>();
        var (depth, inString, escape, objStart) = (0, false, false, -1);

        for (var i = 0; i < arrayContent.Length; i++)
        {
            var ch = arrayContent[i];
            if (HandleEscapeChar(ch, ref inString, ref escape))
                continue;

            if (inString)
                continue;

            switch (ch)
            {
                case '{':
                    if (depth == 0) objStart = i;
                    depth++;
                    break;

                case '}':
                    depth--;
                    if (depth == 0 && objStart >= 0)
                    {
                        var obj = arrayContent.Substring(objStart, i - objStart + 1);
                        var currency = ParseHistoricalCurrency(obj);
                        if (currency != null)
                            currencies.Add(currency);
                        objStart = -1;
                    }
                    break;
            }
        }

        return currencies;
    }

    private static CurrencyRaw? ParseHistoricalCurrency(string obj)
    {
        var code = JsonExtensions.Extract(obj, "Ccy");
        if (string.IsNullOrEmpty(code))
            return null;

        var name = ExtractCurrencyName(obj);
        var country = JsonExtensions.Extract(obj, "CtryNm") ?? string.Empty;
        var num = JsonExtensions.Extract(obj, "CcyNbr");
        var withdrawalDate = JsonExtensions.Extract(obj, "WthdrwlDt");

        return new CurrencyRaw
        {
            Code = code!,
            Name = name,
            Country = country,
            NumericCode = string.IsNullOrEmpty(num) ? null : num,
            WithdrawalDate = string.IsNullOrEmpty(withdrawalDate) ? null : withdrawalDate
        };
    }

    private static string ExtractCurrencyName(string obj)
    {
        var name = JsonExtensions.Extract(obj, "CcyNm");
        if (!string.IsNullOrWhiteSpace(name) &&
            !name!.TrimStart().StartsWith("{") &&
            !name.Contains("\"__text\"")) return name;
        var innerMatch = Regex.Match(
            obj,
            @"""__text""\s*:\s*""([^""]*)""",
            RegexOptions.Compiled | RegexOptions.Singleline);
        if (innerMatch.Success)
            name = innerMatch.Groups[1].Value;

        return name ?? string.Empty;
    }

    private static (int EndIndex, int Depth) FindJsonBlockEnd(string json, int startIndex, char open, char close)
    {
        var depth = 0;
        var inString = false;
        var escape = false;

        for (var i = startIndex; i < json.Length; i++)
        {
            var ch = json[i];
            if (HandleEscapeChar(ch, ref inString, ref escape))
                continue;

            if (inString)
                continue;

            if (ch == open) depth++;
            else if (ch == close)
            {
                depth--;
                if (depth == 0)
                    return (i, depth);
            }
        }

        return (-1, depth);
    }

    private static bool HandleEscapeChar(char ch, ref bool inString, ref bool escape)
    {
        if (escape)
        {
            escape = false;
            return true;
        }
        switch (ch)
        {
            case '\\':
                escape = true;
                return true;
            case '"':
                inString = !inString;
                return true;
            default:
                return false;
        }
    }
}