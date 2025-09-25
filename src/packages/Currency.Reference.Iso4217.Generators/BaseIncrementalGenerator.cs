using Currency.Reference.Iso4217.Generators.Factories;
using Currency.Reference.Iso4217.Generators.Models;
using Microsoft.CodeAnalysis;
namespace Currency.Reference.Iso4217.Generators;

public abstract class BaseIncrementalGenerator : IIncrementalGenerator
{
    public virtual string HintName { get; } = string.Empty;
    protected virtual string DiagnosticsTitle { get; } = "Generator error";
    protected readonly ErrorFactory ErrorFactory = new();
    public abstract void Initialize(IncrementalGeneratorInitializationContext context);
    protected void ShowIssues(SourceProductionContext context, GeneratorType type)
    {
        ErrorFactory.ShowDiagnostics(context, type);
    }
}