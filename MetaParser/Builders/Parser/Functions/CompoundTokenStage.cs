using MetaParser.Builders.Interfaces;
using MetaParser.Core;
using Microsoft.CodeAnalysis.CSharp;

namespace MetaParser.Builders.Parser.Functions;

internal class CompoundTokenStage : IMetaCodeBuilder
{
    public static CompoundTokenStage Instance = new CompoundTokenStage();
    public const string FunctionName = "Parse_Compound";

    public void WriteTo(MetaParserContext context)
    {
        var argumentType = SyntaxFactory.ParseTypeName($"{CodeCommon.ReadOnlyMemory}<{CodeCommon.TokenValueStructName}>");
        var resultsBuilderType = SyntaxFactory.ParseTypeName($"{CodeCommon.List}<{CodeCommon.TokenRecordTypeName}>");
        const string VarNameIdBuffer = "idValues";
        const string VarNameResults = "results";
        var writer = context.writer;

        writer.WriteLine($"private static {CodeCommon.TokenRecordTypeName}[] {FunctionName}({argumentType} {CodeCommon.VarNameBufferMajor})");
        writer.WriteLine("{");
        writer.Indent++;
        // Copy all of our value token ids into a uniform array in memory
        writer.WriteLine($"var {VarNameIdBuffer} = new {context.IdType}[{CodeCommon.VarNameBufferMajor}.Length];");
        writer.WriteLine($"for (int i = 0; i < {CodeCommon.VarNameBufferMajor}.Length; i++)");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine($"{VarNameIdBuffer}[i] = {CodeCommon.VarNameBufferMajor}.Span[i].Id;");
        writer.Indent--;
        writer.WriteLine("}");
        writer.WriteLine();

        writer.WriteLine($"var {CodeCommon.VarNameBufferMinor} = new {CodeCommon.ReadOnlyMemory}<{context.IdType}>( {VarNameIdBuffer} );");
        writer.WriteLine($"var {CodeCommon.VarNameBufferLocal} = {CodeCommon.VarNameBufferMinor}.Span;");
        writer.WriteLine($"var {VarNameResults} = new {resultsBuilderType}();");
        writer.WriteLine();

        writer.WriteLine($"while ({CodeCommon.VarNameBufferLocal}.Length > 0)");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine($"if ({CodeCommon.CompoundTokenProcessorFunctionName}({CodeCommon.VarNameBufferLocal}, out var outId, out var outLength))");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine($"var consumed = {CodeCommon.VarNameBufferMajor}.Slice(0, outLength).ToArray();");
        writer.WriteLine($"{VarNameResults}.Add(new {CodeCommon.TokenRecordTypeName}(({CodeCommon.TokenEnum}) outId, consumed) );");
        writer.WriteLine();
        writer.WriteLine($"{CodeCommon.VarNameBufferMajor} = {CodeCommon.VarNameBufferMajor}.Slice(outLength);");
        writer.WriteLine($"{CodeCommon.VarNameBufferMinor} = {CodeCommon.VarNameBufferMinor}.Slice(outLength);");
        writer.WriteLine($"{CodeCommon.VarNameBufferLocal} = {CodeCommon.VarNameBufferMinor}.Span;");
        writer.Indent--;
        writer.WriteLine("}");
        writer.WriteLine("else");
        writer.WriteLine("{");
        writer.Indent++;
#if DEBUG
        writer.WriteLine("/* Proxy the current token as it has no special compound behavior */");
#endif
        writer.WriteLine($"var consumed = {CodeCommon.VarNameBufferMajor}.Span[0];");
        var createNewToken = $"new {CodeCommon.TokenRecordTypeName}(({CodeCommon.TokenEnum}) consumed.Id, new[] {{ consumed }})";
        writer.WriteLine($"{VarNameResults}.Add({createNewToken});");
        writer.WriteLine($"{CodeCommon.VarNameBufferMajor} = {CodeCommon.VarNameBufferMajor}.Slice(1);");
        writer.WriteLine($"{CodeCommon.VarNameBufferMinor} = {CodeCommon.VarNameBufferMinor}.Slice(1);");
        writer.WriteLine($"{CodeCommon.VarNameBufferLocal} = {CodeCommon.VarNameBufferMinor}.Span;");
        writer.Indent--;
        writer.WriteLine("}");

        writer.Indent--;
        writer.WriteLine("}");// end while-loop

        writer.WriteLine();
        writer.WriteLine($"return {VarNameResults}.ToArray();");
        writer.Indent--;
        writer.WriteLine("}");// end function
    }
}
