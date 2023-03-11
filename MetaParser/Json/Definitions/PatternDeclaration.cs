using MetaParser.Core;
using MetaParser.Parsing.Constructs;

namespace MetaParser.Json.Definitions;

internal interface IPatternDeclaration
{
    public Pattern? Resolve(MetaParserContext context);
}
