using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
namespace Currency.Reference.Iso4217.Generators.Utils;

internal static class JsonParserUtils
{
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

    public static bool HandleEscapeChar(char ch, ref bool inString, ref bool escape)
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

    public static string ExtractPublishedDate(string json)
    {
        var match = Regex.Match(json, @"""_Pblshd""\s*:\s*""([^""]+)""",
            RegexOptions.Compiled | RegexOptions.Singleline);

        if (!match.Success)
            throw new InvalidOperationException("Published date ('_Pblshd') is required.");

        return match.Groups[1].Value;
    }

    public static string ExtractArray(string json, string keyName)
    {
        var keyMatch = Regex.Match(json, $@"""{keyName}""\s*:\s*\[",
            RegexOptions.Compiled | RegexOptions.Singleline);

        if (!keyMatch.Success)
            throw new InvalidOperationException($"The block '{keyName}' is required in the JSON content.");

        var arrayStart = keyMatch.Index + keyMatch.Length - 1;
        if (arrayStart < 0 || arrayStart >= json.Length || json[arrayStart] != '[')
            throw new InvalidOperationException($"Cannot locate '[' for '{keyName}'.");

        var (arrayEnd, _) = FindJsonBlockEnd(json, arrayStart, '[', ']');
        if (arrayEnd == -1)
            throw new InvalidOperationException($"Cannot locate end of '{keyName}' array.");

        return json.Substring(arrayStart + 1, arrayEnd - arrayStart - 1);
    }
    
     public static string? Extract(string json, string key)
    {
        if (string.IsNullOrEmpty(json) || string.IsNullOrEmpty(key))
            return null;
        
        var stringPattern = $@"""{Regex.Escape(key)}""\s*:\s*""((?:\\.|[^""\\])*)""";
        var m = Regex.Match(json, stringPattern, RegexOptions.Singleline | RegexOptions.Compiled);
        if (m.Success)
            return Normalize(UnescapeJsonString(m.Groups[1].Value));
        
        var numberPattern = $@"""{Regex.Escape(key)}""\s*:\s*([0-9]+)";
        m = Regex.Match(json, numberPattern, RegexOptions.Singleline | RegexOptions.Compiled);
        if (m.Success)
            return m.Groups[1].Value;
        
        var objectPattern = $@"""{Regex.Escape(key)}""\s*:\s*\{{(.*?)\}}";
        m = Regex.Match(json, objectPattern, RegexOptions.Singleline | RegexOptions.Compiled);
        if (!m.Success) return null;
        var body = m.Groups[1].Value;

        var textMatch = Regex.Match(body, @"""__text""\s*:\s*""((?:\\.|[^""\\])*)""",
            RegexOptions.Singleline | RegexOptions.Compiled);

        if (textMatch.Success)
            return Normalize(UnescapeJsonString(textMatch.Groups[1].Value));

        return "{" + body.Trim() + "}";

    }
  
    private static string UnescapeJsonString(string s)
    {
        if (string.IsNullOrEmpty(s))
            return string.Empty;

        var sb = new StringBuilder(s.Length);
        for (var i = 0; i < s.Length; i++)
        {
            var c = s[i];
            if (c != '\\')
            {
                sb.Append(c);
                continue;
            }
            if (++i >= s.Length)
            {
                sb.Append('\\');
                break;
            }

            var esc = s[i];
            switch (esc)
            {
                case '"': sb.Append('"'); break;
                case '\\': sb.Append('\\'); break;
                case '/': sb.Append('/'); break;
                case 'b': sb.Append('\b'); break;
                case 'f': sb.Append('\f'); break;
                case 'n': sb.Append('\n'); break;
                case 'r': sb.Append('\r'); break;
                case 't': sb.Append('\t'); break;

                case 'u':
                    if (i + 4 < s.Length)
                    {
                        string hex = s.Substring(i + 1, 4);
                        if (int.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int code))
                        {
                            sb.Append((char)code);
                            i += 4;
                        }
                        else
                        {
                            sb.Append("\\u").Append(hex);
                            i += 4;
                        }
                    }
                    else
                    {
                        sb.Append("\\u");
                    }
                    break;

                default:
                    sb.Append('\\').Append(esc);
                    break;
            }
        }

        return sb.ToString();
    }
    
    private static string Normalize(string s)
    {
        return s.Replace("\"", "'");
    }
}