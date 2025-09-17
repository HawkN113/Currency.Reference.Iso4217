using Currency.Reference.Iso4217.Builders.Abstractions;
using Currency.Reference.Iso4217.Common.Models;
namespace Currency.Reference.Iso4217.Abstractions;

internal interface ICurrencyService
{
    bool IsValid(string value, CriteriaField[] fields, CurrencyType? type = null);
    CurrencyInfo? Get(string value, CriteriaField field, CurrencyType? type = null);
    ICurrencyQueryStart Query();
}