namespace Currency.Reference.Iso4217.Builders.Abstractions;

/// <summary>
/// Start interface for building a currency query.
/// Provides access to the <see cref="Includes"/> selector.
/// </summary>
public interface ICurrencyQueryStart
{
    /// <summary>
    /// Selector for including currencies by type.
    /// </summary>
    ICurrencyQueryTypeSelector Includes { get; }
}