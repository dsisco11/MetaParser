using MetaParser.Builders.Interfaces;
using MetaParser.Core;
using Microsoft.CodeAnalysis.CSharp;

namespace MetaParser.Builders.Parser.Functions;

internal class ComplexTokenStage : IMetaCodeBuilder
{
    public static ComplexTokenStage Instance = new ComplexTokenStage();
    public const string FunctionName = "Parse_Complex";

    public void WriteTo(MetaParserContext context)
    {
        var argumentType = SyntaxFactory.ParseTypeName($"{CodeCommon.ReadOnlyMemory}<{CodeCommon.TokenRecordTypeName}>");
        var writer = context.writer;

        writer.WriteLine($"private static {CodeCommon.TokenRecordTypeName}[] {FunctionName}({argumentType} {CodeCommon.VarNameBufferMajor})");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine($"return Array.Empty<{CodeCommon.TokenRecordTypeName}>();");
        writer.Indent--;
        writer.WriteLine("}");// end function
    }
}
