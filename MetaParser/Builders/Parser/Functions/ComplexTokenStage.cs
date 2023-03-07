using MetaParser.Builders.Interfaces;
using MetaParser.Core;
using Microsoft.CodeAnalysis.CSharp;

namespace MetaParser.Builders.Parser.Functions;
using static CodeCommon;

internal class ComplexTokenStage : IMetaCodeBuilder
{
    public static ComplexTokenStage Instance = new ComplexTokenStage();
    public const string FunctionName = "Parse_Complex";

    public void WriteTo(MetaParserContext context)
    {
        var argumentType = SyntaxFactory.ParseTypeName($"{ReadOnlyMemory}<{TokenRecordTypeName}>");
        var writer = context.writer;

        writer.WriteLine($"private static {TokenRecordTypeName}[] {FunctionName}({argumentType} {VarNameBufferMajor})");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine($"return Array.Empty<{TokenRecordTypeName}>();");
        writer.Indent--;
        writer.WriteLine("}");// end function
    }
}
