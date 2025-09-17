using Currency.Reference.Iso4217.Common.Models;
namespace Currency.Reference.Iso4217.Builders.Abstractions;

/// <summary>
/// Interface for filtering a currency query.
/// Allows selecting currency types and including or excluding specific codes, names, or numeric codes.
/// </summary>
public interface ICurrencyQueryFilter : ICurrencyQueryFinal
{
    /// <summary>
    /// Adds a currency type to the filter.
    /// </summary>
    /// <param name="type">The <see cref="CurrencyType"/> to include.</param>
    /// <returns>The current <see cref="ICurrencyQueryFilter"/> instance.</returns>
    ICurrencyQueryFilter Type(CurrencyType type);
    
    /// <summary>
    /// Includes specific currencies by configuring an include filter.
    /// </summary>
    /// <param name="configure">Action to configure included codes, names, or numeric codes.</param>
    /// <returns>The current <see cref="ICurrencyQueryFilter"/> instance.</returns>
    ICurrencyQueryFilter With(Action<IIncludeFilterBuilder> configure);
    
    /// <summary>
    /// Excludes specific currencies by configuring an exclude filter.
    /// </summary>
    /// <param name="configure">Action to configure excluded codes, names, or numeric codes.</param>
    /// <returns>The current <see cref="ICurrencyQueryFilter"/> instance.</returns>
    ICurrencyQueryFilter Without(Action<IExcludeFilterBuilder> configure);
    
}