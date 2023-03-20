using MetaParser.Builders.Core;
using MetaParser.Builders.Interfaces;
using MetaParser.Builders.TokenLogic.Consumer;
using MetaParser.Core;
using MetaParser.Parsing.Constructs;

using Microsoft.CodeAnalysis.CSharp;

namespace MetaParser.Builders.Parser;
using static CodeCommon;

internal class GenTokenConsumeFunctions : MetaCodeBuilder
{
    public static FunctionDefinition Get_Function_Definition(MetaParserContext context, EConsumerType type, string name)
    {
        return new FunctionDefinition(SyntaxFactory.ParseTokens("private static"),
                                                          SyntaxFactory.ParseTypeName(TypeConsumerResult),
                                                          name,
                                                          SyntaxFactory.ParseArgumentList($"{Get_Token_Buffer_Type(context.Config, type)} {context.ActiveBufferName}"));
    }

    protected override void Write(MetaParserContext context)
    {
        var bodyBuilder = context.Config.CodeFactory.Get_Switch_Block_For_Consumers().And(new ExecuteConsumer());
        foreach (var token in context.WorkingSet.Tokens)
        {
            var funcName = Format_Token_Consume_Function_Name(token.Name);
            var funcDef = Get_Function_Definition(context, EConsumerType.Syntax, funcName);
            funcDef
                .And(bodyBuilder)
                .WriteTo(context with { WorkingSet = new WorkingSet(token) });
        }
    }
}
