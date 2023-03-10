using MetaParser.Parsing.Constructs;
using MetaParser.Parsing.Constructs.Core;

namespace MetaParser.Json.Definitions;

internal interface IPatternDeclaration
{
    public Pattern Resolve(MetaParserContext context);
}
