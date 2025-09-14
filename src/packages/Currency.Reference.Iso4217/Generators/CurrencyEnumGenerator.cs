using System.Text;
using System.Text.Json;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Currency.Reference.Iso4217.Generators;

[Generator]
public class CurrencyEnumGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
    }

    public void Execute(GeneratorExecutionContext context)
    {
        var file = context.AdditionalFiles.FirstOrDefault(f => f.Path.EndsWith("list-one-original-names.json"));
        if (file == null) return;

        var text = file.GetText()?.ToString();
        if (string.IsNullOrWhiteSpace(text)) return;

        var currencies = JsonSerializer.Deserialize<List<CurrencyStub>>(text) ?? new();

        var sb = new StringBuilder();
        sb.AppendLine("namespace Currency.Reference.Iso4217");
        sb.AppendLine("{");
        sb.AppendLine("    public enum CurrencyCode");
        sb.AppendLine("    {");

        foreach (var c in currencies.Select(c => c.Code).Distinct().OrderBy(c => c))
        {
            if (!string.IsNullOrWhiteSpace(c))
            {
                sb.AppendLine($"        {Sanitize(c)},");
            }
        }

        sb.AppendLine("    }");
        sb.AppendLine();
        sb.AppendLine("    public static class CurrencyCodeExtensions");
        sb.AppendLine("    {");
        sb.AppendLine(
            "        public static readonly System.Collections.Generic.Dictionary<CurrencyCode, CurrencyInfo> Dictionary = new()");
        sb.AppendLine("        {");

        foreach (var c in currencies.GroupBy(x => x.Code).Select(g => g.First()).OrderBy(c => c.Code))
        {
            if (!string.IsNullOrWhiteSpace(c.Code))
            {
                sb.AppendLine(
                    $"            [CurrencyCode.{Sanitize(c.Code)}] = new CurrencyInfo {{ Code = \"{c.Code}\", Name = \"{c.Name}\", Country = \"{c.Country}\", NumericCode = \"{c.NumericCode}\", MinorUnits = {(c.MinorUnits?.ToString() ?? "null")} }},");
            }
        }

        sb.AppendLine("        };");
        sb.AppendLine("    }");
        sb.AppendLine("}");

        context.AddSource("CurrencyCode.g.cs", SourceText.From(sb.ToString(), Encoding.UTF8));
    }

    private static string Sanitize(string code)
    {
        if (char.IsDigit(code[0]))
            return "_" + code;
        return code.Replace("-", "_");
    }

    private class CurrencyStub
    {
        public required string Code { get; set; }
        public required string Name { get; set; }
        public required string Country { get; set; }
        public required string NumericCode { get; set; }
        public int? MinorUnits { get; set; }
    }
}