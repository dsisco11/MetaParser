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
    public static FunctionDefinition Get_Function_Definition(ParserContext context, EConsumerKind type, string name)
    {
        return new FunctionDefinition(SyntaxFactory.ParseTokens("private static"),
                                                          SyntaxFactory.ParseTypeName(ConsumerResult),
                                                          name,
                                                          SyntaxFactory.ParseArgumentList($"{Get_Token_Buffer_Type(context.Config, type)} {context.State.ActiveBufferName}"));
    }

    protected override void Write(ParserContext context)
    {
        var bodyBuilder = context.Config.CodeFactory.Get_Switch_Block_For_Consumers().And(new ExecuteConsumer());
        foreach (var token in context.State.Targets.Tokens)
        {
            var funcName = Format_Token_Consume_Function_Name(token.Name);
            var funcDef = Get_Function_Definition(context, EConsumerKind.Syntax, funcName);
            funcDef
                .And(bodyBuilder)
                .WriteTo(context with { State = context.State with { Targets = new WorkingSet(token) } });
        }
    }
}
