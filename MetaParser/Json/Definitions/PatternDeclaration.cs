using MetaParser.Parsing.Constructs.Core;
using MetaParser.Parsing.Constructs.Patternization;

namespace MetaParser.Json.Definitions;

internal interface IPatternDeclaration
{
    public Pattern Resolve(MetaParserContext context);
}
