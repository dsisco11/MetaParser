using MetaParser.Contexts;
using MetaParser.Patternization;

namespace MetaParser.Json.Definitions;

internal interface IPatternDeclaration
{
    public Pattern Resolve(MetaParserContext context);
}
