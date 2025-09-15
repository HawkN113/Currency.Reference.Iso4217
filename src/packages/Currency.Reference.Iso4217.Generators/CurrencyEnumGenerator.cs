using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Currency.Reference.Iso4217.Generators;

[Generator]
public class CurrencyEnumGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new CurrencyEnumSyntaxReceiver());
    }

    private sealed class CurrencyEnumSyntaxReceiver : ISyntaxReceiver
    {
        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
        }
    }

    public void Execute(GeneratorExecutionContext context)
    {
        try
        {
            if (context.SyntaxReceiver is not CurrencyEnumSyntaxReceiver)
            {
                Report(context, "CRY007",
                    "CurrencyEnumSyntaxReceiver is not match.",
                    DiagnosticSeverity.Warning);
            }

            string? text = null;
            var file = context.AdditionalFiles.FirstOrDefault(f => f.Path.EndsWith("list-one-original-names.json"));
            if (file != null)
            {
                text = file.GetText()?.ToString();
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                var asm = typeof(CurrencyEnumGenerator).Assembly;
                var resName = asm.GetManifestResourceNames()
                    .FirstOrDefault(n => n.EndsWith("list-one-original-names.json"));

                if (resName == null)
                {
                    Report(context, "CRY001",
                        "CurrencyEnumGenerator: JSON file not found (AdditionalFiles or EmbeddedResource).",
                        DiagnosticSeverity.Error);
                    return;
                }

                using var s = asm.GetManifestResourceStream(resName);
                if (s == null)
                {
                    Report(context, "CRY002", $"CurrencyEnumGenerator: Failed to open resource stream {resName}.",
                        DiagnosticSeverity.Error);
                    return;
                }

                using var r = new StreamReader(s, Encoding.UTF8);
                text = r.ReadToEnd();
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                Report(context, "CRY003", "CurrencyEnumGenerator: JSON content is empty.", DiagnosticSeverity.Error);
                return;
            }

            List<CurrencyStub>? currencies;
            try
            {
                currencies = JsonSerializer.Deserialize<List<CurrencyStub>>(text,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
            catch (Exception ex)
            {
                Report(context, "CRY004", $"CurrencyEnumGenerator: Failed to parse JSON. {ex.Message}",
                    DiagnosticSeverity.Error);
                return;
            }

            if (currencies == null || currencies.Count == 0)
            {
                Report(context, "CRY005", "CurrencyEnumGenerator: No currencies found in JSON.",
                    DiagnosticSeverity.Warning);
                return;
            }

            var sb = new StringBuilder();

            /*
              sb.AppendLine("using Currency.Reference.Iso4217;");
            sb.AppendLine("namespace Currency.Reference.Iso4217.Generated");
            sb.AppendLine("{");
             */

            //sb.AppendLine("using Currency.Reference.Iso4217;");
            sb.AppendLine("namespace Currency.Reference.Iso4217;");
            sb.AppendLine("    public enum CurrencyCode");
            sb.AppendLine("    {");
            foreach (var c in currencies.Select(c => c.Code).Distinct().OrderBy(c => c))
            {
                if (!string.IsNullOrWhiteSpace(c))
                    sb.AppendLine($"        {Sanitize(c)},");
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
                        $"            [CurrencyCode.{Sanitize(c.Code)}] = new CurrencyInfo {{ Code = \"{Escape(c.Code)}\", Name = \"{Escape(c.Name)}\", Country = \"{Escape(c.Country)}\", NumericCode = \"{Escape(c.NumericCode)}\", MinorUnits = {(!string.IsNullOrEmpty(c.MinorUnits) ? $"\"{c.MinorUnits}\"" : "null")} }},");
                }
            }

            sb.AppendLine("        };");
            sb.AppendLine("    }");
            //sb.AppendLine("}");

            context.AddSource("CurrencyCode.g.cs", SourceText.From(sb.ToString(), Encoding.UTF8));
        }
        catch (Exception ex)
        {
            Report(context, "CRY006", ex.Message, DiagnosticSeverity.Error);
        }
    }

    private static string Sanitize(string code)
    {
        if (string.IsNullOrEmpty(code)) return "_";
        if (char.IsDigit(code[0]))
            return "_" + code;
        return code.Replace("-", "_");
    }

    private static string Escape(string value) =>
        value.Replace("\\", "\\\\").Replace("\"", "\\\"");

    private static void Report(GeneratorExecutionContext context, string id, string message,
        DiagnosticSeverity severity)
    {
        var descriptor = new DiagnosticDescriptor(
            id: id,
            title: "CurrencyEnumGenerator",
            messageFormat: message,
            category: "Currency.Reference.Iso4217",
            defaultSeverity: severity,
            isEnabledByDefault: true);

        context.ReportDiagnostic(Diagnostic.Create(descriptor, Location.None));
    }

    private class CurrencyStub
    {
        [JsonPropertyName("Ccy")] public required string Code { get; set; }
        [JsonPropertyName("CcyNm")] public required string Name { get; set; }
        [JsonPropertyName("CtryNm")] public required string Country { get; set; }
        [JsonPropertyName("CcyNbr")] public required string NumericCode { get; set; }
        [JsonPropertyName("CcyMnrUnts")] public string? MinorUnits { get; set; }
        public override string ToString() => $"{Code} - {Name} ({Country})";
    }
}