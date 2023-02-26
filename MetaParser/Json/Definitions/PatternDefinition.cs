using MetaParser.Contexts;
using MetaParser.Patternization;

namespace MetaParser.Json.Definitions;

internal abstract record PatternDefinition
{
    public abstract Pattern Resolve(MetaParserContext context);
}
