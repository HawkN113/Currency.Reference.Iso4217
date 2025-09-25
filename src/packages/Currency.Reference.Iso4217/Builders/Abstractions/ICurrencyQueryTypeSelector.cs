using Currency.Reference.Iso4217.Models;
namespace Currency.Reference.Iso4217.Builders.Abstractions;

/// <summary>
/// Selector interface for specifying currency types in the query.
/// </summary>
public interface ICurrencyQueryTypeSelector
{
    /// <summary>
    /// Adds a currency type to the query filter.
    /// </summary>
    /// <param name="type">The <see cref="CurrencyType"/> to include.</param>
    /// <returns>An <see cref="ICurrencyQueryFilter"/> to continue building the query.</returns>
    ICurrencyQueryFilter Type(CurrencyType type);
}