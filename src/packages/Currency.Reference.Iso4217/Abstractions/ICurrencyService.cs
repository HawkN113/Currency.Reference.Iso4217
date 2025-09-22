using Currency.Reference.Iso4217.Builders.Abstractions;
using Currency.Reference.Iso4217.Common.Models;
namespace Currency.Reference.Iso4217.Abstractions;

/// <summary>
/// Service for working with currencies: validation, retrieval, and querying.
/// </summary>
public interface ICurrencyService
{
    /// <summary>
    /// Checks if the specified value meets all given criteria.
    /// </summary>
    /// <param name="value">Currency code or name.</param>
    /// <param name="criteria">One or more criteria to validate against.</param>
    /// <returns>True if the value satisfies all criteria.</returns>
    bool IsValid(string value, params Criteria[] criteria);

    /// <summary>
    /// Checks if the specified currency code meets all given criteria.
    /// </summary>
    /// <param name="code">Currency code enum.</param>
    /// <param name="criteria">One or more criteria to validate against.</param>
    /// <returns>True if the code satisfies all criteria.</returns>
    bool IsValid(CurrencyCode code, params Criteria[] criteria);

    /// <summary>
    /// Gets currency information by code or name with optional criteria.
    /// </summary>
    /// <param name="value">Currency code or name.</param>
    /// <param name="criteria">Optional criteria to filter the result.</param>
    /// <returns>CurrencyInfo object or null if not found.</returns>
    CurrencyInfo? Get(string value, params Criteria[] criteria);

    /// <summary>
    /// Gets currency information by enum code with optional criteria.
    /// </summary>
    /// <param name="code">Currency code enum.</param>
    /// <param name="criteria">Optional criteria to filter the result.</param>
    /// <returns>CurrencyInfo object or null if not found.</returns>
    CurrencyInfo? Get(CurrencyCode code, params Criteria[] criteria);

    /// <summary>
    /// Starts building a query for currencies with fluent filtering and sorting.
    /// </summary>
    /// <returns>Start object for currency query.</returns>
    ICurrencyQueryStart Query();
}