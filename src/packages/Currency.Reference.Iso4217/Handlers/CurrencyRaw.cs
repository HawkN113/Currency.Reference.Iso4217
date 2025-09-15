using System.Text.Json.Serialization;
namespace Currency.Reference.Iso4217.Handlers;

internal sealed record CurrencyRaw
{
    [JsonPropertyName("Ccy")] public required string Code { get; set; }
    [JsonPropertyName("CcyNm")] public required string Name { get; set; }
    [JsonPropertyName("CtryNm")] public required string Country { get; set; }
    [JsonPropertyName("CcyNbr")] public required string NumericCode { get; set; }
}