using System.Text.Json;
namespace Currency.Reference.Iso4217.Handlers;

internal static class JsonCurrencyHandler
{
    private const string CurrenciesFilePath = "Source\\list-one-original-names.json";

    public static List<CurrencyRaw> LoadCurrencies()
    {
        var path = Path.Combine(AppContext.BaseDirectory, CurrenciesFilePath);
        if (!File.Exists(path))
            throw new InvalidOperationException("Embedded ISO 4217 JSON resource not found.");
        using var stream = File.OpenRead(path);
        using var reader = new StreamReader(stream);
        var jsonFromStream = reader.ReadToEnd();

        var currencies = JsonSerializer.Deserialize<List<CurrencyRaw>>(jsonFromStream,
                             new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                         ?? throw new InvalidOperationException("Failed to parse JSON.");
        return currencies.GroupBy(c => c.Code).Select(g => g.First()).ToList();
    }
}