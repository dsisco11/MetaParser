using System.Text.Json.Serialization;
using System.Linq;
using MetaParser.Parsing.Constructs;
using MetaParser.Compiler.Structs;
using System;

namespace MetaParser.Json.Definitions;

internal sealed record PatternDeclaration : IPatternDeclaration
{
    #region Properties
    [JsonPropertyName("value")]
    public string? value { get; set; }

    [JsonPropertyName("range")]
    public string[]? range { get; set; }

    [JsonPropertyName("oneof")]
    public PatternDeclaration[]? oneof { get; set; }

    [JsonPropertyName("not")]
    public PatternDeclaration? not { get; set; }
    #endregion

    #region Constructors
    public PatternDeclaration()
    {
    }

    [JsonConstructor]
    public PatternDeclaration(string? value, string[]? range, PatternDeclaration[]? oneof, PatternDeclaration? not)
    {
        this.value = value;
        this.range = range;
        this.oneof = oneof;
        this.not = not;
    }
    #endregion

    #region Interpreting
    public IPatternClause? Interpret()
    {
        if (value is not null)
        {
            return new PatternItemClause(EPatternKind.Literal, value);
        }
        else if (not is not null)
        {
            return new PatternSequenceClause(EPatternKind.Not, not.Interpret()!);
        }
        else if (oneof is not null)
        {
            var patterns = oneof.Select(static (o) => o.Interpret()!).ToArray();
            return new PatternSequenceClause(EPatternKind.OneOf, patterns ?? Array.Empty<IPatternClause>());
        }
        else if (range is not null)
        {
            var start = new PatternItemClause(EPatternKind.Literal, range[0]);
            var stop = new PatternItemClause(EPatternKind.Literal, range[1]);
            return new PatternSequenceClause(EPatternKind.Range, start, stop);
        }

        return null;
    }
    #endregion
}
