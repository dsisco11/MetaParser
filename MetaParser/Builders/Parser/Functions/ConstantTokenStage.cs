using MetaParser.Builders.Interfaces;
using MetaParser.Core;

using Microsoft.CodeAnalysis.CSharp;

namespace MetaParser.Builders.Parser.Functions;
using static CodeCommon;

internal class ConstantTokenStage : MetaCodeBuilder
{
    public const string FunctionName = "Parse_Constant";

    protected override void Write(MetaParserContext context)
    {
        var argumentType = SyntaxFactory.ParseTypeName($"{ReadOnlyMemory}<{Get_Consumer_Data_Type(context.Config, Consumers.EConsumerType.Data)}>");
        var resultsBuilderType = SyntaxFactory.ParseTypeName($"{List}<{TokenValueStructName}>");
        const string VarNameResults = "results";
        var writer = context.writer;

        writer.WriteLine($"private static {TokenValueStructName}[] {FunctionName}({argumentType} {VarNameBufferMajor})");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine($"var {VarNameBufferMinor} = {VarNameBufferMajor};");
        writer.WriteLine($"var {VarNameBufferLocal} = {VarNameBufferMinor}.Span;");
        writer.WriteLine($"var {VarNameResults} = new {resultsBuilderType}();");
        writer.WriteLine();

        writer.WriteLine($"while ({VarNameBufferLocal}.Length > 0)");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine($"if ({ConstantTokenProcessorFunctionName}({VarNameBufferLocal}, out var outId, out var outLength))");
        writer.WriteLine("{");
        writer.Indent++;
        // Be sure to push unknown token if its lingering
        UnknownTokenPusher.Instance.WriteTo(context);
        writer.WriteLine();
        writer.WriteLine($"var consumed = {VarNameBufferMinor}.Slice(0, outLength);");
        writer.WriteLine($"{VarNameResults}.Add( new {TokenValueStructName}(outId, consumed) );");
        writer.WriteLine($"{VarNameBufferMinor} = {VarNameBufferMinor}.Slice(outLength);");
        writer.WriteLine($"{VarNameBufferLocal} = {VarNameBufferMinor}.Span;");
        writer.Indent--;
        writer.WriteLine("}");
        writer.WriteLine("else");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine($"{VarNameBufferLocal} = {VarNameBufferLocal}.Slice(1);");
        writer.Indent--;
        writer.WriteLine("}");

        writer.Indent--;
        writer.WriteLine("}");// end while-loop

        writer.WriteLine();
        // Be sure to push unknown token if its lingering
        UnknownTokenPusher.Instance.WriteTo(context);
        writer.WriteLine();
        writer.WriteLine($"return {VarNameResults}.ToArray();");
        writer.Indent--;
        writer.WriteLine("}");// end function
    }
}
