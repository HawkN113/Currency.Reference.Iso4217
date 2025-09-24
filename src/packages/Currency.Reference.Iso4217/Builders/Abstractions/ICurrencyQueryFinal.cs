using Currency.Reference.Iso4217.Domain.Models;
namespace Currency.Reference.Iso4217.Builders.Abstractions;

/// <summary>
/// Final interface for building a currency query.
/// Use <see cref="Build"/> to get the resulting collection of currencies.
/// </summary>
public interface ICurrencyQueryFinal
{
    /// <summary>
    /// Builds and returns a collection of currencies matching the configured filters.
    /// </summary>
    /// <returns>A read-only collection of <see cref="Currency"/> objects.</returns>
    IReadOnlyList<Domain.Models.Currency> Build();
}