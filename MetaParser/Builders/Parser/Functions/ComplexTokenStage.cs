using MetaParser.CodeGen;
using MetaParser.CodeGen.Core;
using MetaParser.Contexts;
using Microsoft.CodeAnalysis.CSharp;

namespace MetaParser.Builders.Parser.Functions;

internal class ComplexTokenStage : IMetaCodeBuilder
{
    public static ComplexTokenStage Instance = new ComplexTokenStage();
    public const string FunctionName = "Parse_Complex";

    public void WriteTo(MetaParserContext context)
    {
        var argumentType = SyntaxFactory.ParseTypeName($"{CodeCommon.ReadOnlyMemory}<{MetaParserContext.TokenClassName}>");
        var writer = context.writer;

        writer.WriteLine($"private static {MetaParserContext.TokenClassName}[] {FunctionName}({argumentType} {MetaParserContext.VarNameBufferMajor})");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine($"return Array.Empty<{MetaParserContext.TokenClassName}>();");
        writer.Indent--;
        writer.WriteLine("}");// end function
    }
}
