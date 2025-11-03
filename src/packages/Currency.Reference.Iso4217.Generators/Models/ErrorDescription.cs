using Microsoft.CodeAnalysis;

namespace Currency.Reference.Iso4217.Generators.Models;

public class ErrorDescription
{
    public DiagnosticDescriptor DiagnosticDescriptor { get; set; } = null!;
    public object?[]? MessageArgs { get; set; }
    public Location? Location { get; set; } = Location.None; 
    public GeneratorType GeneratorType { get; set; } = GeneratorType.Factory;
}