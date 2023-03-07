using MetaParser.CodeGen;
using MetaParser.CodeGen.Interfaces;
using MetaParser.Contexts;
using Microsoft.CodeAnalysis.CSharp;

namespace MetaParser.Builders.Parser.Functions;

internal class CompoundTokenStage : IMetaCodeBuilder
{
    public static CompoundTokenStage Instance = new CompoundTokenStage();
    public const string FunctionName = "Parse_Compound";

    public void WriteTo(MetaParserContext context)
    {
        var argumentType = SyntaxFactory.ParseTypeName($"{CodeCommon.ReadOnlyMemory}<{MetaParserContext.TokenValueStructName}>");
        var resultsBuilderType = SyntaxFactory.ParseTypeName($"{CodeCommon.List}<{MetaParserContext.TokenRecordTypeName}>");
        const string VarNameIdBuffer = "idValues";
        const string VarNameResults = "results";
        var writer = context.writer;

        writer.WriteLine($"private static {MetaParserContext.TokenRecordTypeName}[] {FunctionName}({argumentType} {MetaParserContext.VarNameBufferMajor})");
        writer.WriteLine("{");
        writer.Indent++;
        // Copy all of our value token ids into a uniform array in memory
        writer.WriteLine($"var {VarNameIdBuffer} = new {context.IdType}[{MetaParserContext.VarNameBufferMajor}.Length];");
        writer.WriteLine($"for (int i = 0; i < {MetaParserContext.VarNameBufferMajor}.Length; i++)");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine($"{VarNameIdBuffer}[i] = {MetaParserContext.VarNameBufferMajor}.Span[i].Id;");
        writer.Indent--;
        writer.WriteLine("}");
        writer.WriteLine();

        writer.WriteLine($"var {MetaParserContext.VarNameBufferMinor} = new {CodeCommon.ReadOnlyMemory}<{context.IdType}>( {VarNameIdBuffer} );");
        writer.WriteLine($"var {MetaParserContext.VarNameBufferLocal} = {MetaParserContext.VarNameBufferMinor}.Span;");
        writer.WriteLine($"var {VarNameResults} = new {resultsBuilderType}();");
        writer.WriteLine();

        writer.WriteLine($"while ({MetaParserContext.VarNameBufferLocal}.Length > 0)");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine($"if ({MetaParserContext.CompoundTokenProcessorFunctionName}({MetaParserContext.VarNameBufferLocal}, out var outId, out var outLength))");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine($"var consumed = {MetaParserContext.VarNameBufferMajor}.Slice(0, outLength).ToArray();");
        writer.WriteLine($"{VarNameResults}.Add(new {MetaParserContext.TokenRecordTypeName}(({MetaParserContext.TokenEnum}) outId, consumed) );");
        writer.WriteLine();
        writer.WriteLine($"{MetaParserContext.VarNameBufferMajor} = {MetaParserContext.VarNameBufferMajor}.Slice(outLength);");
        writer.WriteLine($"{MetaParserContext.VarNameBufferMinor} = {MetaParserContext.VarNameBufferMinor}.Slice(outLength);");
        writer.WriteLine($"{MetaParserContext.VarNameBufferLocal} = {MetaParserContext.VarNameBufferMinor}.Span;");
        writer.Indent--;
        writer.WriteLine("}");
        writer.WriteLine("else");
        writer.WriteLine("{");
        writer.Indent++;
#if DEBUG
        writer.WriteLine("/* Proxy the current token as it has no special compound behavior */");
#endif
        writer.WriteLine($"var consumed = {MetaParserContext.VarNameBufferMajor}.Span[0];");
        var createNewToken = $"new {MetaParserContext.TokenRecordTypeName}(({MetaParserContext.TokenEnum}) consumed.Id, new[] {{ consumed }})";
        writer.WriteLine($"{VarNameResults}.Add({createNewToken});");
        writer.WriteLine($"{MetaParserContext.VarNameBufferMajor} = {MetaParserContext.VarNameBufferMajor}.Slice(1);");
        writer.WriteLine($"{MetaParserContext.VarNameBufferMinor} = {MetaParserContext.VarNameBufferMinor}.Slice(1);");
        writer.WriteLine($"{MetaParserContext.VarNameBufferLocal} = {MetaParserContext.VarNameBufferMinor}.Span;");
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
