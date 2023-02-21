
using Microsoft.CodeAnalysis;

namespace MetaParser
{
    internal static class DIAGNOSTIC_DEFS
    {
        public static DiagnosticDescriptor Info => new DiagnosticDescriptor("MP000", "MetaParser", "{0}", "Compiler", DiagnosticSeverity.Info, true);
        public static DiagnosticDescriptor SchemaException => new DiagnosticDescriptor("MP001", "Schema Error", "[{0}] {1}", "Compiler", DiagnosticSeverity.Info, true);
        public static DiagnosticDescriptor JsonException => new DiagnosticDescriptor("MP002", "Json Exception", "Encountered JSON exception: {0}", "Compiler", DiagnosticSeverity.Error, true);
        public static DiagnosticDescriptor CannotLocateRequiredProperty => new DiagnosticDescriptor("MP003", "Unable to locate required property", "Unable to locate required property '{0}' in file {1}", "Schema", DiagnosticSeverity.Error, true);
        public static DiagnosticDescriptor InvalidTokenName => new DiagnosticDescriptor("MP004", "Invalid token name", "The string '{0}' is not a valid token name", "Schema", DiagnosticSeverity.Warning, true);
        public static DiagnosticDescriptor UnrecognizedPropertyValue => new DiagnosticDescriptor("MP005", "unrecognized property value", "The value '{0}' is not a recognized value for {1}", "Schema", DiagnosticSeverity.Error, true);
    }
}
