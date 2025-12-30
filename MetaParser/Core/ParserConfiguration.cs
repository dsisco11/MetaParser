namespace MetaParser.Core;

/// <summary>
/// Configuration for parser code generation.
/// </summary>
internal sealed class ParserConfiguration
{
    /// <summary>
    /// Base file name for generated output (derived from schema file name).
    /// </summary>
    public string BaseFileName { get; set; } = string.Empty;

    /// <summary>
    /// Name of the generated parser class.
    /// </summary>
    public string ClassName { get; set; } = "Parser";

    /// <summary>
    /// Namespace for generated code.
    /// </summary>
    public string Namespace { get; set; } = string.Empty;

    /// <summary>
    /// Creates configuration from a schema definition.
    /// </summary>
    public static ParserConfiguration FromSchema(Schema.SchemaDefinition schema, string baseFileName)
    {
        return new ParserConfiguration
        {
            BaseFileName = baseFileName,
            ClassName = schema.Classname,
            Namespace = schema.Namespace
        };
    }
}
