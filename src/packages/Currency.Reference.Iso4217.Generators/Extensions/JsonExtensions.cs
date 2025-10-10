using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
namespace Currency.Reference.Iso4217.Generators.Extensions;

public static class JsonExtensions
{
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

            if (i + 1 >= s.Length)
            {
                sb.Append('\\');
                break;
            }

            i++;
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
                        var hex = s.Substring(i + 1, 4);
                        if (int.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int codePoint))
                        {
                            sb.Append((char)codePoint);
                            i += 4;
                        }
                        else
                        {
                            sb.Append("\\u");
                        }
                    }
                    else
                    {
                        sb.Append("\\u");
                    }
                    break;
                default:
                    sb.Append(esc);
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