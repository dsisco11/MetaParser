using MetaParser.Builders.Interfaces;
using MetaParser.Core;

using Microsoft.CodeAnalysis.CSharp;

namespace MetaParser.Builders.Parser.Functions;

internal class ConstantTokenStage : IMetaCodeBuilder
{
    public static ConstantTokenStage Instance = new ConstantTokenStage();
    public const string FunctionName = "Parse_Constant";

    public void WriteTo(MetaParserContext context)
    {
        var argumentType = SyntaxFactory.ParseTypeName($"{CodeCommon.ReadOnlyMemory}<{CodeCommon.Get_Consumer_Data_Type(context.Config, Consumers.EConsumerType.Data)}>");
        var resultsBuilderType = SyntaxFactory.ParseTypeName($"{CodeCommon.List}<{CodeCommon.TokenValueStructName}>");
        const string VarNameResults = "results";
        var writer = context.writer;

        writer.WriteLine($"private static {CodeCommon.TokenValueStructName}[] {FunctionName}({argumentType} {CodeCommon.VarNameBufferMajor})");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine($"var {CodeCommon.VarNameBufferMinor} = {CodeCommon.VarNameBufferMajor};");
        writer.WriteLine($"var {CodeCommon.VarNameBufferLocal} = {CodeCommon.VarNameBufferMinor}.Span;");
        writer.WriteLine($"var {VarNameResults} = new {resultsBuilderType}();");
        writer.WriteLine();

        writer.WriteLine($"while ({CodeCommon.VarNameBufferLocal}.Length > 0)");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine($"if ({CodeCommon.ConstantTokenProcessorFunctionName}({CodeCommon.VarNameBufferLocal}, out var outId, out var outLength))");
        writer.WriteLine("{");
        writer.Indent++;
        // Be sure to push unknown token if its lingering
        UnknownTokenPusher.Instance.WriteTo(context);
        writer.WriteLine();
        writer.WriteLine($"var consumed = {CodeCommon.VarNameBufferMinor}.Slice(0, outLength);");
        writer.WriteLine($"{VarNameResults}.Add( new {CodeCommon.TokenValueStructName}(outId, consumed) );");
        writer.WriteLine($"{CodeCommon.VarNameBufferMinor} = {CodeCommon.VarNameBufferMinor}.Slice(outLength);");
        writer.WriteLine($"{CodeCommon.VarNameBufferLocal} = {CodeCommon.VarNameBufferMinor}.Span;");
        writer.Indent--;
        writer.WriteLine("}");
        writer.WriteLine("else");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine($"{CodeCommon.VarNameBufferLocal} = {CodeCommon.VarNameBufferLocal}.Slice(1);");
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
