using MetaParser.Compiler.Structs;

namespace MetaParser.Json.Definitions;

internal interface IPatternDeclaration
{
    public IPatternClause? Interpret();
}
