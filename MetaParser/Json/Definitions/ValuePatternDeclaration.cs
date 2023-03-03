using System.Text.Json.Serialization;
using MetaParser.Contexts;
using Microsoft.CodeAnalysis.CSharp;
using System.Linq;
using MetaParser.Patternization;

namespace MetaParser.Json.Definitions;

internal sealed record ValuePatternDeclaration : IPatternDeclaration
{
    #region Properties
    [JsonPropertyName("value")]
    public string? value { get; set; }

    [JsonPropertyName("range")]
    public string[]? range { get; set; }

    [JsonPropertyName("oneof")]
    public ValuePatternDeclaration[]? oneof { get; set; }
    #endregion

    #region Constructors
    public ValuePatternDeclaration()
    {
    }

    [JsonConstructor]
    public ValuePatternDeclaration(string? value, string[]? range, ValuePatternDeclaration[]? oneof)
    {
        this.value = value;
        this.range = range;
        this.oneof = oneof;
    }
    #endregion

    public Pattern Resolve(MetaParserContext context)
    {
        if (value is not null)
        {
            return ResolveConst(context);
        }
        else if (range is not null)
        {
            return ResolveRange(context);
        }
        else if (oneof is not null)
        {
            return ResolveOneOf(context);
        }

        return Pattern.Empty;
    }

    private Pattern ResolveConst(MetaParserContext context)
    {
        if (string.IsNullOrEmpty(value))
        {
            return Pattern.Empty;
        }

        if (value.Length == 1)
        {
            return new PatternConst(SymbolDisplay.FormatLiteral(value[0], true));
        }
        // A string value is matched as a sequence of chars
        var consts = value.ToCharArray().Select(ch => new PatternConst(SymbolDisplay.FormatLiteral(ch, true))).ToArray();
        return new PatternGroup(EPatternCondition.AllOf, consts);
    }

    private Pattern ResolveRange(MetaParserContext context)
    {
        var start = SymbolDisplay.FormatLiteral(range[0][0], true);
        var stop = SymbolDisplay.FormatLiteral(range[1][0], true);
        return new PatternRange(start, stop);
    }

    private Pattern ResolveOneOf(MetaParserContext context)
    {
        if (oneof is null || oneof.Length == 0)
        {
            return Pattern.Empty;
        }

        if (oneof.Length == 1)
        {
            return oneof.Single().Resolve(context);
        }

        var consts = oneof.Select(o => o.Resolve(context)).ToArray();
        return new PatternGroup(EPatternCondition.OneOf, consts);
    }
}
