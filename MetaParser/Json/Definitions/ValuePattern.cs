using System.Text.Json.Serialization;
using MetaParser.Contexts;
using Microsoft.CodeAnalysis.CSharp;
using System.Linq;
using MetaParser.Patternization;
using MetaParser.Json.Attributes;

namespace MetaParser.Json.Definitions;

internal sealed record ValuePattern : PatternDeclaration
{
    #region Properties
    [JsonPrimaryProperty]
    [JsonPropertyName("value")]
    public string? value { get; set; }

    [JsonPropertyName("range")]
    public string[]? range { get; set; }
    #endregion

    [JsonConstructor]
    public ValuePattern(string? value = null, string[]? range = null)
    {
        this.value = value;
        this.range = range;
    }

    public override Pattern Resolve(MetaParserContext context)
    {
        if (value is not null)
        {
            return ResolveConst(context);
        }
        else if (range is not null)
        {
            return ResolveRange(context);
        }

        return base.Resolve(context);
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
}