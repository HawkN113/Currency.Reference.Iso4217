using Currency.Reference.Iso4217.Abstractions;
using Currency.Reference.Iso4217.Builders;
using Currency.Reference.Iso4217.Builders.Abstractions;
using Currency.Reference.Iso4217.Handlers;
using Currency.Reference.Iso4217.Models;
namespace Currency.Reference.Iso4217.Services;

internal sealed class CurrencyService : ICurrencyService
{
    private readonly List<CurrencyInfo> _currencies;
    private static readonly HashSet<string> PreciousMetalsCodes = new(StringComparer.OrdinalIgnoreCase)
        { "XAG", "XAU", "XPD", "XPT" };
    private static readonly HashSet<string> SpecialReserveCodes = new(StringComparer.OrdinalIgnoreCase)
        { "XXX", "XTS" };
    private static readonly HashSet<string> SpecialUnits = new(StringComparer.OrdinalIgnoreCase)
        { "XBA", "XBB", "XBC", "XBD", "XSU", "XUA" };
    public CurrencyService()
    {
        var loadedCurrencies = JsonCurrencyHandler.LoadCurrencies();
        _currencies = new List<CurrencyInfo>(loadedCurrencies.Count);

        var replacements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["AFN"] = "Afghan Afghani",
            ["AMD"] = "Armenian Dram",
            ["AOA"] = "Angolan Kwanza",
            ["BDT"] = "Bangladeshi Taka",
            ["BOB"] = "Bolivian Boliviano",
            ["BOV"] = "Bolivian Mvdol",
            ["BWP"] = "Botswana Pula",
            ["ERN"] = "Eritrean Nakfa",
            ["GEL"] = "Georgian Lari",
            ["GMD"] = "Gambian Dalasi",
            ["GTQ"] = "Guatemalan Quetzal",
            ["JPY"] = "Japanese Yen",
            ["KGS"] = "Kyrgyzstani Som",
            ["LSL"] = "Lesotho Loti",
            ["MOP"] = "Macanese Pataca",
            ["MRU"] = "Mauritanian Ouguiya",
            ["NGN"] = "Nigerian Naira",
            ["PAB"] = "Panamanian Balboa",
            ["PEN"] = "Peruvian Sol",
            ["PGK"] = "Papua New Guinean Kina",
            ["PYG"] = "Paraguayan Guarani",
            ["SLE"] = "Sierra Leonean Leone",
            ["STN"] = "São Tomé and Príncipe Dobra",
            ["SZL"] = "Swazi Lilangeni",
            ["TJS"] = "Tajikistani Somoni",
            ["TOP"] = "Tongan Paʻanga",
            ["UAH"] = "Ukrainian Hryvnia",
            ["VUV"] = "Vanuatu Vatu",
            ["WST"] = "Samoan Tala",
            ["XAG"] = "Silver (one troy ounce)",
            ["XAU"] = "Gold (one troy ounce)",
            ["XPD"] = "Palladium (one troy ounce)",
            ["XPF"] = "CFP Franc",
            ["ZAR"] = "South African Rand",
            ["XPT"] = "Platinum (one troy ounce)",
            ["XSU"] = "Sucre (Unidad de Cuenta del ALBA)",
            ["VND"] = "Vietnamese Dong",
            ["KZT"] = "Kazakhstani Tenge",
            ["KRW"] = "South Korean Won",
            ["KPW"] = "North Korean Won",
            ["MNT"] = "Mongolian Tugrik",
            ["THB"] = "Thai Baht",
            ["LAK"] = "Lao Kip",
            ["MMK"] = "Myanmar Kyat",
            ["KHR"] = "Cambodian Riel",
            ["MVR"] = "Maldivian Rufiyaa",
            ["BTN"] = "Bhutanese Ngultrum",
            ["NPR"] = "Nepalese Rupee",
            ["PKR"] = "Pakistani Rupee",
            ["INR"] = "Indian Rupee",
            ["LKR"] = "Sri Lankan Rupee",
            ["MUR"] = "Mauritian Rupee",
            ["SCR"] = "Seychelles Rupee",
            ["HNL"] = "Honduran Lempira",
            ["HTG"] = "Haitian Gourde",
            ["HUF"] = "Hungarian Forint",
            ["IDR"] = "Indonesian Rupiah",
            ["ISK"] = "Icelandic Krona",
            ["NOK"] = "Norwegian Krone",
            ["SEK"] = "Swedish Krona",
            ["DKK"] = "Danish Krone",
            ["CZK"] = "Czech Koruna",
            ["PLN"] = "Polish Zloty",
            ["RON"] = "Romanian Leu",
            ["MDL"] = "Moldovan Leu",
            ["BGN"] = "Bulgarian Lev",
            ["ALL"] = "Albanian Lek",
            ["MKD"] = "Macedonian Denar",
            ["RSD"] = "Serbian Dinar",
            ["DZD"] = "Algerian Dinar",
            ["IQD"] = "Iraqi Dinar",
            ["JOD"] = "Jordanian Dinar",
            ["KWD"] = "Kuwaiti Dinar",
            ["LYD"] = "Libyan Dinar",
            ["TND"] = "Tunisian Dinar",
            ["MAD"] = "Moroccan Dirham",
            ["QAR"] = "Qatari Riyal",
            ["SAR"] = "Saudi Riyal",
            ["YER"] = "Yemeni Rial",
            ["OMR"] = "Omani Rial",
            ["IRR"] = "Iranian Rial",
            ["NAD"] = "Namibian Dollar",
            ["CAD"] = "Canadian Dollar",
            ["AUD"] = "Australian Dollar",
            ["NZD"] = "New Zealand Dollar",
            ["HKD"] = "Hong Kong Dollar",
            ["SGD"] = "Singapore Dollar",
            ["FJD"] = "Fiji Dollar",
            ["XCD"] = "East Caribbean Dollar",
            ["TWD"] = "New Taiwan Dollar",
            ["BBD"] = "Barbados Dollar",
            ["BZD"] = "Belize Dollar",
            ["BSD"] = "Bahamian Dollar",
            ["BMD"] = "Bermudian Dollar",
            ["TTD"] = "Trinidad and Tobago Dollar",
            ["GYD"] = "Guyanese Dollar",
            ["SRD"] = "Surinamese Dollar",
            ["JMD"] = "Jamaican Dollar",
            ["LRD"] = "Liberian Dollar",
            ["SBD"] = "Solomon Islands Dollar",
            ["USD"] = "US Dollar",
            ["ZWL"] = "Zimbabwe Dollar",
            ["CNY"] = "Chinese Yuan",
            ["CVE"] = "Cape Verde Escudo"
        };
        foreach (var item in loadedCurrencies)
            _currencies.Add(new CurrencyInfo(
                Code: item.Code,
                Name: replacements.TryGetValue(item.Code, out var newName) ? newName : item.Name,
                Country: item.Country,
                NumericCode: item.NumericCode,
                CurrencyType: GetCurrencyType(item.Code))
            );
        _currencies = _currencies.OrderBy(c => c.Code).ToList();
    }
    
    public bool IsValid(string value, CriteriaField[] fields, CurrencyType? type = null)
    {
        if (fields.Length == 0)
            throw new ArgumentException($"{nameof(fields)} must not be empty.", nameof(fields));
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{nameof(value)} must not be empty.", nameof(value));

        var result = false;
        var trimmed = value.Trim();
        foreach (var field in fields)
        {
            result = field switch
            {
                CriteriaField.Code => _currencies.Any(c =>
                    c.Code.Equals(trimmed, StringComparison.OrdinalIgnoreCase)),
                CriteriaField.Name => _currencies.Any(c =>
                    c.Name.Equals(trimmed, StringComparison.OrdinalIgnoreCase)),
                CriteriaField.NumericCode => _currencies.Any(c => c.NumericCode == trimmed),
                CriteriaField.CurrencyType => type.HasValue && _currencies.Any(c =>
                    GetCurrencyType(c.Code) == type.Value && c.Code.Equals(trimmed, StringComparison.OrdinalIgnoreCase)),
                _ => result
            };
            if (result)
                return true;
        }

        return result;
    }

    public CurrencyInfo? Get(string value, CriteriaField field, CurrencyType? type = null)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{nameof(value)} must not be empty.", nameof(value));

        CurrencyInfo? result = null;
        var trimmed = value.Trim();
        return field switch
        {
            CriteriaField.Code => _currencies.FirstOrDefault(c =>
                c.Code.Equals(trimmed, StringComparison.OrdinalIgnoreCase)),
            CriteriaField.Name => _currencies.FirstOrDefault(c =>
                c.Name.Equals(trimmed, StringComparison.OrdinalIgnoreCase)),
            CriteriaField.NumericCode => _currencies.FirstOrDefault(c => c.NumericCode == trimmed),
            CriteriaField.CurrencyType => type.HasValue
                ? _currencies.FirstOrDefault(c =>
                    GetCurrencyType(c.Code) == type.Value && c.Code.Equals(trimmed, StringComparison.OrdinalIgnoreCase))
                : null,
            _ => result
        };
    }
    public ICurrencyQueryStart Query()
    {
        return new CurrencyQueryBuilder(_currencies);
    }
    
    private static CurrencyType GetCurrencyType(string code)
    {
        if (PreciousMetalsCodes.Contains(code))
            return CurrencyType.PreciousMetal;
        if (SpecialReserveCodes.Contains(code))
            return CurrencyType.SpecialReserve;
        return SpecialUnits.Contains(code) ? CurrencyType.SpecialUnit : CurrencyType.Fiat;
    }
}