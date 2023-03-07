using MetaParser.Core;
using MetaParser.Patternization;

namespace MetaParser.Json.Definitions;

internal interface IPatternDeclaration
{
    public Pattern Resolve(MetaParserContext context);
}
