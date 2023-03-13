using MetaParser.Builders.Core;
using MetaParser.Builders.Interfaces;
using MetaParser.Core;
using MetaParser.Parsing.Constructs;

using Microsoft.CodeAnalysis.CSharp;

namespace MetaParser.Builders.Parser;
using static CodeCommon;

internal class GenTokenStartDetectors : MetaCodeBuilder
{
    public static FunctionDefinition Get_Function_Definition(MetaParserConfig config, EConsumerType type, string name)
    {
        return (FunctionDefinition)new FunctionDefinition(SyntaxFactory.ParseTokens("private static"), SyntaxFactory.ParseTypeName("bool"), name, SyntaxFactory.ParseArgumentList($"{Get_Token_Buffer_Type(config, type)} {VarNameBufferMajor}")).And(config.CodeFactory.Get_Logic_Detect_Recursive_Pattern());
    }

    protected override void Write(MetaParserContext context)
    {
        foreach (var token in context.WorkingSet.Tokens)
        {
            var funcName = Format_Token_Start_Detection_Function_Name(token.Name);
            var funcDef = Get_Function_Definition(context.Config, EConsumerType.Token, funcName);
            funcDef.WriteTo(context with { WorkingSet = new WorkingSet(token) });
        }
    }
}
