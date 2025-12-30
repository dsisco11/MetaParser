
using Microsoft.CodeAnalysis;

namespace MetaParser;

internal static class DIAGNOSTIC_DEFS
{
    // General
    public static DiagnosticDescriptor Info => new DiagnosticDescriptor("MP000", "MetaParser", "{0}", "Compiler", DiagnosticSeverity.Info, true);
    
    // v2 Schema Validation
    public static DiagnosticDescriptor SchemaValidationError => new DiagnosticDescriptor("MP100", "Schema Validation Error", "{0}", "Schema", DiagnosticSeverity.Error, true);
    public static DiagnosticDescriptor SchemaValidationWarning => new DiagnosticDescriptor("MP110", "Schema Validation Warning", "{0}", "Schema", DiagnosticSeverity.Warning, true);
    public static DiagnosticDescriptor InvalidTokenReference => new DiagnosticDescriptor("MP101", "Invalid Token Reference", "Token '{0}' references undefined token '{1}'", "Schema", DiagnosticSeverity.Error, true);
    public static DiagnosticDescriptor CircularDependency => new DiagnosticDescriptor("MP102", "Circular Dependency", "Circular dependency detected: {0}", "Schema", DiagnosticSeverity.Error, true);
    public static DiagnosticDescriptor InvalidCharacterRange => new DiagnosticDescriptor("MP103", "Invalid Character Range", "Invalid range in token '{0}': start '{1}' is greater than end '{2}'", "Schema", DiagnosticSeverity.Error, true);
    public static DiagnosticDescriptor MissingConsumePattern => new DiagnosticDescriptor("MP104", "Missing Consume Pattern", "Token '{0}' must have a 'consume' pattern", "Schema", DiagnosticSeverity.Error, true);
    public static DiagnosticDescriptor DuplicateTokenName => new DiagnosticDescriptor("MP105", "Duplicate Token Name", "Token '{0}' is defined multiple times", "Schema", DiagnosticSeverity.Error, true);
    public static DiagnosticDescriptor InvalidTriviaReference => new DiagnosticDescriptor("MP106", "Invalid Trivia Reference", "Trivia references undefined token '{0}'", "Schema", DiagnosticSeverity.Error, true);
    
    // Legacy v1 (kept for compatibility)
    public static DiagnosticDescriptor SchemaException => new DiagnosticDescriptor("MP001", "Schema Error", "[{0}] {1}", "Compiler", DiagnosticSeverity.Warning, true);
    public static DiagnosticDescriptor JsonException => new DiagnosticDescriptor("MP002", "Json Exception", "Encountered JSON exception: {0}", "Compiler", DiagnosticSeverity.Error, true);
    public static DiagnosticDescriptor MissingValue => new DiagnosticDescriptor("MP003", "MetaParser definition missing required value", "{0}", "Schema", DiagnosticSeverity.Error, true);
    public static DiagnosticDescriptor CannotLocateRequiredProperty => new DiagnosticDescriptor("MP004", "Unable to locate required property", "Unable to locate required property '{0}' in file {1}", "Schema", DiagnosticSeverity.Error, true);
    public static DiagnosticDescriptor InvalidTokenName => new DiagnosticDescriptor("MP005", "Invalid token name", "The string '{0}' is not a valid token name", "Schema", DiagnosticSeverity.Warning, true);
    public static DiagnosticDescriptor UnrecognizedPropertyValue => new DiagnosticDescriptor("MP006", "unrecognized property value", "The value '{0}' is not a recognized value for {1}", "Schema", DiagnosticSeverity.Error, true);
}
