using System.Text.Json.Serialization;

namespace Currency.Reference.Iso4217;

public sealed class CurrencyInfo
{
    [JsonPropertyName("Ccy")] public required string Code { get; set; }
    [JsonPropertyName("CcyNm")] public required string Name { get; set; }
    [JsonPropertyName("CtryNm")] public required string Country { get; set; }
    [JsonPropertyName("CcyNbr")] public required string NumericCode { get; set; }
    [JsonPropertyName("CcyMnrUnts")] public string? MinorUnits { get; set; }
    [JsonIgnore]
    public int? MinorUnitsAsInt =>
        int.TryParse(MinorUnits, out var value) ? value : null;
    public override string ToString() => $"{Code} - {Name} ({Country})";
}