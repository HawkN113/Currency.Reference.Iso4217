using System;
using System.Collections.Generic;
using System.Linq;
using Currency.Reference.Iso4217.Generators.Abstractions;
using Currency.Reference.Iso4217.Generators.Models;
namespace Currency.Reference.Iso4217.Generators.Handlers;

internal class CurrencyLoader : ICurrencyLoader
{
    private readonly List<CurrencyInfo> _currencies;
    private readonly string _jsonContent;

    private static readonly HashSet<string> PreciousMetalsCodes = new(StringComparer.OrdinalIgnoreCase)
        { "XAG", "XAU", "XPD", "XPT" };

    private static readonly HashSet<string> SpecialReserveCodes = new(StringComparer.OrdinalIgnoreCase)
        { "XXX", "XTS" };

    private static readonly HashSet<string> SpecialUnits = new(StringComparer.OrdinalIgnoreCase)
        { "XBA", "XBB", "XBC", "XBD", "XSU", "XUA" };
    
    public List<CurrencyInfo> Currencies => _currencies;

    public CurrencyLoader(string jsonContent)
    {
        _jsonContent= jsonContent;
        var loadedCurrencies = new JsonCurrencyHandler(_jsonContent).LoadCurrencies();
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
            _currencies.Add(new CurrencyInfo()
                {
                    Code = item.Code,
                    Name = replacements.TryGetValue(item.Code, out var newName) ? newName : item.Name,
                    Country = item.Country,
                    NumericCode = item.NumericCode,
                    CurrencyType = GetCurrencyType(item.Code)
                }
            );
        _currencies = _currencies.OrderBy(c => c.Code).ToList();
    }

    public CurrencyType GetCurrencyType(string code)
    {
        if (PreciousMetalsCodes.Contains(code))
            return CurrencyType.PreciousMetal;
        if (SpecialReserveCodes.Contains(code))
            return CurrencyType.SpecialReserve;
        return SpecialUnits.Contains(code) ? CurrencyType.SpecialUnit : CurrencyType.Fiat;
    }
}