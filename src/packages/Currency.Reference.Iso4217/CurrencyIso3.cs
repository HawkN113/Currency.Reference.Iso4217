using System.Reflection;
using System.Text.Json;

namespace Currency.Reference.Iso4217;

internal static class CurrencyIso3
    {
        private static readonly Lazy<List<CurrencyInfo>> Currencies =
            new Lazy<List<CurrencyInfo>>(LoadCurrencies);

        public static IReadOnlyCollection<CurrencyInfo> All => Currencies.Value;

        public static bool IsValidCode(string? code) =>
            !string.IsNullOrWhiteSpace(code) &&
            Currencies.Value.Any(c => c.Code.Equals(code.Trim(), StringComparison.OrdinalIgnoreCase));

        public static bool IsValidName(string? name) =>
            !string.IsNullOrWhiteSpace(name) &&
            Currencies.Value.Any(c => c.Name.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase));

        public static bool IsValidNumeric(string? numericCode) =>
            !string.IsNullOrWhiteSpace(numericCode) &&
            Currencies.Value.Any(c => c.NumericCode == numericCode.Trim());

        public static CurrencyInfo? GetByCode(string code) =>
            Currencies.Value.FirstOrDefault(c => c.Code.Equals(code.Trim(), StringComparison.OrdinalIgnoreCase));

        public static CurrencyInfo? GetByNumeric(string numericCode) =>
            Currencies.Value.FirstOrDefault(c => c.NumericCode == numericCode.Trim());

        public static CurrencyInfo? GetByName(string name) =>
            Currencies.Value.FirstOrDefault(c => c.Name.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase));

        public static IEnumerable<string> UniqueCodesWithNames()
        {
            var result = new HashSet<string>();
            foreach (var code in Currencies.Value)
            {
                result.Add($"{code.Code} - {code.Name}");
            }

            return result;
        }

        public static IEnumerable<CurrencyInfo> GetAllExcept(params string[] excludedCodes)
        {
            var excluded = new HashSet<string>(excludedCodes, StringComparer.OrdinalIgnoreCase);
            return Currencies.Value.Where(c => !excluded.Contains(c.Code));
        }

        private static List<CurrencyInfo> LoadCurrencies()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = assembly.GetManifestResourceNames()
                .FirstOrDefault(n => n.EndsWith("list-one-original-names.json", StringComparison.OrdinalIgnoreCase));

            if (resourceName == null)
                throw new InvalidOperationException("Embedded ISO 4217 JSON resource not found.");

            using var stream = assembly.GetManifestResourceStream(resourceName)
                               ?? throw new FileNotFoundException("Embedded ISO 4217 resource stream not found.");

            using var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();

            var list = JsonSerializer.Deserialize<List<CurrencyInfo>>(json,
                           new JsonSerializerOptions
                           {
                               PropertyNameCaseInsensitive = true
                           })
                       ?? throw new InvalidOperationException("Failed to parse ISO 4217 JSON.");

            return list.GroupBy(c => c.Code).Select(g => g.First()).ToList();
        }
    }