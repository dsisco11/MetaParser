using MetaParser.Core;

namespace MetaParser.Builders.Interfaces;

internal interface IMetaCodeBuilder : ICodeBuilder<ParserContext>
{
}

internal interface IMetaCodeFunctionBuilder : IMetaCodeBuilder
{
    string Get_Function_Name(ParserContext context);
}
