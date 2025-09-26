using System.Text.RegularExpressions;

namespace Currency.Reference.Iso4217.Generators.Extensions;

public static class JsonExtensions
{
    public static string? Extract(string json, string key)
    {
        var pattern = $"\"{Regex.Escape(key)}\"\\s*:\\s*\"([^\"]+)\"";
        var match = Regex.Match(json, pattern);
        return match.Success ? match.Groups[1].Value : null;
    }
}