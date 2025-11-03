using Currency.Reference.Iso4217.Generators.Handlers;
using Currency.Reference.Iso4217.Generators.Models;
namespace Currency.Reference.Iso4217.Generators.Tests.Handlers;

public class CurrencyLoaderTests
{
    private const string ActualJson = """
                                      {
                                      "ISO_4217": {
                                        "_Pblshd": "2025-05-12",
                                        "CcyTbl": {
                                          "CcyNtry": [
                                                    {
                                                      "CtryNm": "AFGHANISTAN",
                                                      "CcyNm": "Afghani",
                                                      "Ccy": "AFN",
                                                      "CcyNbr": "971",
                                                      "CcyMnrUnts": "2"
                                                    },
                                                    {
                                                      "CtryNm": "ÅLAND ISLANDS",
                                                      "CcyNm": "Euro",
                                                      "Ccy": "EUR",
                                                      "CcyNbr": "978",
                                                      "CcyMnrUnts": "2"
                                                    },
                                                    {
                                                      "CtryNm": "ALBANIA",
                                                      "CcyNm": "Lek",
                                                      "Ccy": "ALL",
                                                      "CcyNbr": "008",
                                                      "CcyMnrUnts": "2"
                                                    },
                                                    {
                                                      "CtryNm": "ALGERIA",
                                                      "CcyNm": "Algerian Dinar",
                                                      "Ccy": "DZD",
                                                      "CcyNbr": "012",
                                                      "CcyMnrUnts": "2"
                                                    },
                                                    {
                                                      "CtryNm": "AMERICAN SAMOA",
                                                      "CcyNm": "US Dollar",
                                                      "Ccy": "USD",
                                                      "CcyNbr": "840",
                                                      "CcyMnrUnts": "2"
                                                    }
                                                  ]
                                              }
                                          }
                                      }
                                      """;

    private const string ReplacementJson = """
                                           [
                                               { "Ccy": "AFN", "CcyNm": "Afghani" },
                                               { "Ccy": "AMD", "CcyNm": "Armenian Dram" },
                                               { "Ccy": "AOA", "CcyNm": "Angolan Kwanza" },
                                               { "Ccy": "BDT", "CcyNm": "Bangladeshi Taka" },
                                               { "Ccy": "BOB", "CcyNm": "Bolivian Boliviano" }
                                           ]
                                           """;

    private const string HistoricalJson = """
                                          {
                                              "ISO_4217": {
                                                "_Pblshd": "2025-05-12",
                                                "HstrcCcyTbl": {
                                                  "HstrcCcyNtry": [
                                                        {
                                                          "CtryNm": "AFGHANISTAN",
                                                          "CcyNm": "Afghani",
                                                          "Ccy": "AFA",
                                                          "CcyNbr": "004",
                                                          "WthdrwlDt": "2003-01"
                                                        },
                                                        {
                                                          "CtryNm": "ÅLAND ISLANDS",
                                                          "CcyNm": "Markka",
                                                          "Ccy": "FIM",
                                                          "CcyNbr": "246",
                                                          "WthdrwlDt": "2002-03"
                                                        },
                                                        {
                                                          "CtryNm": "ALBANIA",
                                                          "CcyNm": "Old Lek",
                                                          "Ccy": "ALK",
                                                          "CcyNbr": "008",
                                                          "WthdrwlDt": "1989-12"
                                                        },
                                                        {
                                                          "CtryNm": "ANDORRA",
                                                          "CcyNm": "Andorran Peseta",
                                                          "Ccy": "ADP",
                                                          "CcyNbr": "020",
                                                          "WthdrwlDt": "2003-07"
                                                        },
                                                        {
                                                          "CtryNm": "ANDORRA",
                                                          "CcyNm": "Spanish Peseta",
                                                          "Ccy": "ESP",
                                                          "CcyNbr": "724",
                                                          "WthdrwlDt": "2002-03"
                                                        }
                                                      ]
                                                  }
                                              }
                                          }
                                          """;

    [Fact]
    public void Constructor_Should_Load_And_Process_Currencies_Correctly()
    {
        var loader = new CurrencyLoader(ActualJson, ReplacementJson, HistoricalJson);

        Assert.NotNull(loader.ActualCurrencyData);
        Assert.NotNull(loader.HistoricalCurrencyData);

        Assert.Equal("2025-05-12", loader.ActualCurrencyData.PublishedDate);
        Assert.Equal("2025-05-12", loader.HistoricalCurrencyData.PublishedDate);

        var actualCodes = loader.ActualCurrencyData.Currencies.Select(c => c.Code).ToArray();
        Assert.DoesNotContain("VED", actualCodes);
        Assert.Contains("USD", actualCodes);
        Assert.Contains("EUR", actualCodes);
    }

    [Fact]
    public void Replacement_Should_Update_Names()
    {
        var loader = new CurrencyLoader(ActualJson, ReplacementJson, HistoricalJson);

        var usd = loader.ActualCurrencyData.Currencies.First(c => c.Code == "USD");

        Assert.Equal("US Dollar", usd.Name);
    }

    [Fact]
    public void CurrencyTypes_Should_Be_Assigned_Correctly()
    {
        var loader = new CurrencyLoader(ActualJson, ReplacementJson, HistoricalJson);

        var usd = loader.ActualCurrencyData.Currencies.First(c => c.Code == "USD");

        Assert.Equal(CurrencyType.Fiat, usd.CurrencyType);
    }

    [Fact]
    public void HistoricalCurrencies_Should_Have_IsHistoric_True()
    {
        var loader = new CurrencyLoader(ActualJson, ReplacementJson, HistoricalJson);

        var adp = loader.HistoricalCurrencyData.Currencies.First(c => c.Code == "ADP");
        Assert.True(adp.IsHistoric);
        Assert.Equal("2003-07", adp.WithdrawalDate);
    }

    [Fact]
    public void ExcludedCodes_Should_Be_Filtered_Out()
    {
        var loader = new CurrencyLoader(ActualJson, ReplacementJson, HistoricalJson);

        Assert.DoesNotContain(loader.ActualCurrencyData.Currencies, c => c.Code == "VED");
        Assert.DoesNotContain(loader.HistoricalCurrencyData.Currencies, c => c.Code == "ZWG");
    }

    [Fact]
    public void Currencies_Should_Be_Sorted_By_Code()
    {
        var loader = new CurrencyLoader(ActualJson, ReplacementJson, HistoricalJson);

        var actualCodes = loader.ActualCurrencyData.Currencies.Select(c => c.Code).ToList();
        var sorted = actualCodes.OrderBy(c => c).ToList();

        Assert.Equal(sorted, actualCodes);
    }
}