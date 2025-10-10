namespace Currency.Reference.Iso4217.Generators.Handlers;

internal sealed class CurrencyDataSet
{
    public string PublishedDate { get; set; } = string.Empty;
    public List<CurrencyRaw> Currencies { get; set; } = [];
}