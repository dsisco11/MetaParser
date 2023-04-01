using MetaParser.Builders.Core;
using MetaParser.Builders.Interfaces;
using MetaParser.Core;
using MetaParser.Parsing.Constructs;

using Microsoft.CodeAnalysis.CSharp;

namespace MetaParser.Builders.Parser;
using static CodeCommon;

internal class GenPatternConsumerFunctions : MetaCodeBuilder
{
    public static FunctionDefinition Get_Function_Definition(ParserContext context, EConsumerKind type, string name)
    {
        return new FunctionDefinition(SyntaxFactory.ParseTokens("private static"),
                                                          SyntaxFactory.ParseTypeName(TypeConsumerResult),
                                                          name,
                                                          SyntaxFactory.ParseArgumentList($"{Get_Token_Buffer_Type(context.Config, type)} {context.State.ActiveBufferName}"));
    }

    protected override void Write(ParserContext context)
    {
        foreach (var consumer in context.WorkingSet.Consumers)
        {
            if (consumer.IsConstant)
            {
                continue;// skip const patterns as they get an inline fast-path
            }

            var body = context.Config.CodeFactory.Get_Logic_Consumer_Match();
            var funcName = Format_Pattern_Consumer_Function_Name(consumer.Key.Index);
            var funcDef = Get_Function_Definition(context, consumer.Kind, funcName);
            funcDef.And(body).WriteTo(context with { WorkingSet = new WorkingSet(consumer) });
        }
    }
}
