using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MetaParser.Schema;

/// <summary>
/// JSON serializer context for MetaParser v2 schema types.
/// Uses source generation for AOT compatibility.
/// </summary>
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    WriteIndented = false,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
[JsonSerializable(typeof(SchemaDefinition))]
[JsonSerializable(typeof(TokenDefinition))]
[JsonSerializable(typeof(PatternDefinition))]
[JsonSerializable(typeof(LiteralPattern))]
[JsonSerializable(typeof(RangePattern))]
[JsonSerializable(typeof(TokenReferencePattern))]
[JsonSerializable(typeof(OneOfPattern))]
[JsonSerializable(typeof(Dictionary<string, TokenDefinition>))]
[JsonSerializable(typeof(List<string>))]
[JsonSerializable(typeof(PatternDefinition[]))]
internal partial class SchemaJsonContext : JsonSerializerContext
{
}
