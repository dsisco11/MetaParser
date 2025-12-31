using Microsoft.CodeAnalysis;

namespace MetaParser.Diagnostics;

/// <summary>
/// Diagnostic descriptors for MetaParser v2 source generator.
/// 
/// Diagnostic ID Ranges:
/// - MP000-MP099: General/Internal diagnostics
/// - MP100-MP199: Schema validation errors
/// - MP200-MP299: Schema validation warnings
/// - MP300-MP399: Code generation errors
/// - MP400-MP499: Code generation warnings
/// </summary>
internal static class DiagnosticDescriptors
{
    private const string Category_General = "MetaParser";
    private const string Category_Schema = "MetaParser.Schema";
    private const string Category_CodeGen = "MetaParser.CodeGen";
    
    #region General Diagnostics (MP000-MP099)
    
    /// <summary>
    /// MP000: General info message.
    /// </summary>
    public static readonly DiagnosticDescriptor Info = new(
        id: "MP0000",
        title: "MetaParser Info",
        messageFormat: "{0}",
        category: Category_General,
        defaultSeverity: DiagnosticSeverity.Info,
        isEnabledByDefault: true);
    
    /// <summary>
    /// MP001: JSON parsing exception.
    /// </summary>
    public static readonly DiagnosticDescriptor JsonParseError = new(
        id: "MP0001",
        title: "JSON Parse Error",
        messageFormat: "Failed to parse schema JSON: {0}",
        category: Category_General,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);
    
    /// <summary>
    /// MP002: Internal generator error.
    /// </summary>
    public static readonly DiagnosticDescriptor InternalError = new(
        id: "MP0002",
        title: "Internal Error",
        messageFormat: "Internal generator error: {0}",
        category: Category_General,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);
    
    #endregion
    
    #region Schema Validation Errors (MP100-MP199)
    
    /// <summary>
    /// MP100: Generic schema validation error.
    /// </summary>
    public static readonly DiagnosticDescriptor SchemaValidationError = new(
        id: "MP0100",
        title: "Schema Validation Error",
        messageFormat: "{0}",
        category: Category_Schema,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);
    
    /// <summary>
    /// MP101: Missing required namespace.
    /// </summary>
    public static readonly DiagnosticDescriptor MissingNamespace = new(
        id: "MP0101",
        title: "Missing Namespace",
        messageFormat: "Schema must specify a 'namespace'",
        category: Category_Schema,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);
    
    /// <summary>
    /// MP102: Missing token definitions.
    /// </summary>
    public static readonly DiagnosticDescriptor MissingTokens = new(
        id: "MP0102",
        title: "Missing Tokens",
        messageFormat: "Schema must define at least one token",
        category: Category_Schema,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);
    
    /// <summary>
    /// MP103: Token missing consume pattern.
    /// </summary>
    public static readonly DiagnosticDescriptor MissingConsumePattern = new(
        id: "MP0103",
        title: "Missing Consume Pattern",
        messageFormat: "Token '{0}' must have a 'consume' pattern",
        category: Category_Schema,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);
    
    /// <summary>
    /// MP104: Token references undefined token.
    /// </summary>
    public static readonly DiagnosticDescriptor UndefinedTokenReference = new(
        id: "MP0104",
        title: "Undefined Token Reference",
        messageFormat: "Token '{0}' references undefined token '{1}' in '{2}'",
        category: Category_Schema,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);
    
    /// <summary>
    /// MP105: Token has self-reference.
    /// </summary>
    public static readonly DiagnosticDescriptor SelfReference = new(
        id: "MP0105",
        title: "Self Reference",
        messageFormat: "Token '{0}' has self-reference in '{1}'",
        category: Category_Schema,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);
    
    /// <summary>
    /// MP106: Circular dependency detected.
    /// </summary>
    public static readonly DiagnosticDescriptor CircularDependency = new(
        id: "MP0106",
        title: "Circular Dependency",
        messageFormat: "Circular dependency detected: {0}",
        category: Category_Schema,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);
    
    /// <summary>
    /// MP107: Invalid character range.
    /// </summary>
    public static readonly DiagnosticDescriptor InvalidCharacterRange = new(
        id: "MP0107",
        title: "Invalid Character Range",
        messageFormat: "Token '{0}' has invalid range in '{1}': start '{2}' is greater than end '{3}'",
        category: Category_Schema,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);
    
    /// <summary>
    /// MP108: Empty literal pattern.
    /// </summary>
    public static readonly DiagnosticDescriptor EmptyLiteral = new(
        id: "MP0108",
        title: "Empty Literal",
        messageFormat: "Token '{0}' has empty literal in '{1}'",
        category: Category_Schema,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);
    
    /// <summary>
    /// MP109: Trivia references undefined token.
    /// </summary>
    public static readonly DiagnosticDescriptor UndefinedTriviaReference = new(
        id: "MP0109",
        title: "Undefined Trivia Reference",
        messageFormat: "Trivia references undefined token '{0}'",
        category: Category_Schema,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);
    
    /// <summary>
    /// MP110: Duplicate trivia entry.
    /// </summary>
    public static readonly DiagnosticDescriptor DuplicateTrivia = new(
        id: "MP0110",
        title: "Duplicate Trivia",
        messageFormat: "Trivia '{0}' is listed multiple times",
        category: Category_Schema,
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true);
    
    #endregion
    
    #region Schema Validation Warnings (MP200-MP299)
    
    /// <summary>
    /// MP200: Generic schema validation warning.
    /// </summary>
    public static readonly DiagnosticDescriptor SchemaValidationWarning = new(
        id: "MP0200",
        title: "Schema Validation Warning",
        messageFormat: "{0}",
        category: Category_Schema,
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true);
    
    /// <summary>
    /// MP201: Token is never referenced.
    /// </summary>
    public static readonly DiagnosticDescriptor UnreferencedToken = new(
        id: "MP0201",
        title: "Unreferenced Token",
        messageFormat: "Token '{0}' is never referenced by other tokens",
        category: Category_Schema,
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true);
    
    #endregion
    
    #region Code Generation Errors (MP300-MP399)
    
    /// <summary>
    /// MP300: Code generation failed.
    /// </summary>
    public static readonly DiagnosticDescriptor CodeGenerationFailed = new(
        id: "MP0300",
        title: "Code Generation Failed",
        messageFormat: "Failed to generate code: {0}",
        category: Category_CodeGen,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);
    
    #endregion
}
