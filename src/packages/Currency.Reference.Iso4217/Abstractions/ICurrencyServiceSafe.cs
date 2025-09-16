using Currency.Reference.Iso4217.Builders.Abstractions;
namespace Currency.Reference.Iso4217.Abstractions;

public interface ICurrencyServiceSafe
{
    ICurrencyQueryStart Query();
}