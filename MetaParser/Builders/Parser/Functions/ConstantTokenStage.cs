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
        var argumentType = SyntaxFactory.ParseTypeName($"{CodeCommon.ReadOnlyMemory}<{context.Get_Consumer_Data_Type(Consumers.EConsumerType.Data)}>");
        var resultsBuilderType = SyntaxFactory.ParseTypeName($"{CodeCommon.List}<{MetaParserContext.TokenValueStructName}>");
        const string VarNameResults = "results";
        var writer = context.writer;

        writer.WriteLine($"private static {MetaParserContext.TokenValueStructName}[] {FunctionName}({argumentType} {MetaParserContext.VarNameBufferMajor})");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine($"var {MetaParserContext.VarNameBufferMinor} = {MetaParserContext.VarNameBufferMajor};");
        writer.WriteLine($"var {MetaParserContext.VarNameBufferLocal} = {MetaParserContext.VarNameBufferMinor}.Span;");
        writer.WriteLine($"var {VarNameResults} = new {resultsBuilderType}();");
        writer.WriteLine();

        writer.WriteLine($"while ({MetaParserContext.VarNameBufferLocal}.Length > 0)");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine($"if ({MetaParserContext.ConstantTokenProcessorFunctionName}({MetaParserContext.VarNameBufferLocal}, out var outId, out var outLength))");
        writer.WriteLine("{");
        writer.Indent++;
        // Be sure to push unknown token if its lingering
        UnknownTokenPusher.Instance.WriteTo(context);
        writer.WriteLine();
        writer.WriteLine($"var consumed = {MetaParserContext.VarNameBufferMinor}.Slice(0, outLength);");
        writer.WriteLine($"{VarNameResults}.Add( new {MetaParserContext.TokenValueStructName}(outId, consumed) );");
        writer.WriteLine($"{MetaParserContext.VarNameBufferMinor} = {MetaParserContext.VarNameBufferMinor}.Slice(outLength);");
        writer.WriteLine($"{MetaParserContext.VarNameBufferLocal} = {MetaParserContext.VarNameBufferMinor}.Span;");
        writer.Indent--;
        writer.WriteLine("}");
        writer.WriteLine("else");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine($"{MetaParserContext.VarNameBufferLocal} = {MetaParserContext.VarNameBufferLocal}.Slice(1);");
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
