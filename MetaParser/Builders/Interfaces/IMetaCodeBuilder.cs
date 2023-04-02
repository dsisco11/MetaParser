using MetaParser.Builders.Core;
using MetaParser.Core;

namespace MetaParser.Builders.Interfaces;

internal interface IMetaCodeBuilder : ICodeBuilder<ParserContext>
{
}

internal interface IMetaCodeFunctionBuilder : IMetaCodeBuilder
{
    FunctionDefinition Get_Definition(ParserContext context);
    string Format_Function_Name(ParserContext context);
}
