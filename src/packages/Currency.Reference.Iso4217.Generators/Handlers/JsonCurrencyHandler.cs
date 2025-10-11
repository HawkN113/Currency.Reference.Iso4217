namespace Currency.Reference.Iso4217.Generators.Handlers;

internal sealed class JsonCurrencyHandler(string jsonContent)
    : JsonCurrencyHandlerBase(jsonContent)
{
    protected override string ArrayKey => "CcyNtry";
    protected override bool IsHistorical => false;
}