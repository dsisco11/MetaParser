using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace MetaParser.Diagnostics;

/// <summary>
/// Helper class for reporting diagnostics with proper location information.
/// </summary>
internal static class DiagnosticReporter
{
    /// <summary>
    /// Creates a diagnostic at a specific location in an additional text file.
    /// </summary>
    public static Diagnostic CreateDiagnostic(
        DiagnosticDescriptor descriptor,
        AdditionalText file,
        TextSpan span,
        params object[] messageArgs)
    {
        var text = file.GetText();
        var location = text != null
            ? Location.Create(file.Path, span, text.Lines.GetLinePositionSpan(span))
            : Location.None;
        
        return Diagnostic.Create(descriptor, location, messageArgs);
    }
    
    /// <summary>
    /// Creates a diagnostic at the start of a file.
    /// </summary>
    public static Diagnostic CreateDiagnostic(
        DiagnosticDescriptor descriptor,
        AdditionalText file,
        params object[] messageArgs)
    {
        var location = Location.Create(file.Path, TextSpan.FromBounds(0, 0), new LinePositionSpan());
        return Diagnostic.Create(descriptor, location, messageArgs);
    }
    
    /// <summary>
    /// Creates a diagnostic with no location.
    /// </summary>
    public static Diagnostic CreateDiagnostic(
        DiagnosticDescriptor descriptor,
        params object[] messageArgs)
    {
        return Diagnostic.Create(descriptor, Location.None, messageArgs);
    }
    
    /// <summary>
    /// Reports a generic schema validation error.
    /// </summary>
    public static void ReportSchemaError(
        SourceProductionContext context,
        string message)
    {
        context.ReportDiagnostic(Diagnostic.Create(
            DiagnosticDescriptors.SchemaValidationError,
            Location.None,
            message));
    }
    
    /// <summary>
    /// Reports a generic schema validation warning.
    /// </summary>
    public static void ReportSchemaWarning(
        SourceProductionContext context,
        string message)
    {
        context.ReportDiagnostic(Diagnostic.Create(
            DiagnosticDescriptors.SchemaValidationWarning,
            Location.None,
            message));
    }
    
    /// <summary>
    /// Reports a JSON parse error.
    /// </summary>
    public static void ReportJsonParseError(
        SourceProductionContext context,
        string filePath,
        string message)
    {
        var location = Location.Create(filePath, TextSpan.FromBounds(0, 0), new LinePositionSpan());
        context.ReportDiagnostic(Diagnostic.Create(
            DiagnosticDescriptors.JsonParseError,
            location,
            message));
    }
    
    /// <summary>
    /// Reports a circular dependency error.
    /// </summary>
    public static void ReportCircularDependency(
        SourceProductionContext context,
        string cyclePath)
    {
        context.ReportDiagnostic(Diagnostic.Create(
            DiagnosticDescriptors.CircularDependency,
            Location.None,
            cyclePath));
    }
    
    /// <summary>
    /// Reports an undefined token reference error.
    /// </summary>
    public static void ReportUndefinedTokenReference(
        SourceProductionContext context,
        string tokenName,
        string referencedToken,
        string patternLocation)
    {
        context.ReportDiagnostic(Diagnostic.Create(
            DiagnosticDescriptors.UndefinedTokenReference,
            Location.None,
            tokenName,
            referencedToken,
            patternLocation));
    }
    
    /// <summary>
    /// Reports an unreferenced token warning.
    /// </summary>
    public static void ReportUnreferencedToken(
        SourceProductionContext context,
        string tokenName)
    {
        context.ReportDiagnostic(Diagnostic.Create(
            DiagnosticDescriptors.UnreferencedToken,
            Location.None,
            tokenName));
    }
}
