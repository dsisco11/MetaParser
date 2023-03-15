using MetaParser.Builders.Interfaces;
using MetaParser.Core;

using Microsoft.CodeAnalysis.CSharp;

namespace MetaParser.Builders.Parser.Functions;
using static CodeCommon;

internal class ComplexTokenStage : MetaCodeBuilder
{
    public const string FunctionName = "Parse_Complex";

    protected override void Write(MetaParserContext context)
    {
        var argumentType = SyntaxFactory.ParseTypeName($"{ReadOnlyMemory}<{TokenRecordTypeName}>");
        var writer = context.writer;

        writer.WriteLine($"private static {TokenRecordTypeName}[] {FunctionName}({argumentType} {context.ActiveBufferName})");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine($"return Array.Empty<{TokenRecordTypeName}>();");
        writer.Indent--;
        writer.WriteLine("}");// end function
    }
}
