using MetaParser.Builders.Interfaces;
using MetaParser.Core;
using Microsoft.CodeAnalysis.CSharp;

namespace MetaParser.Builders.Parser.Functions;
using static CodeCommon;

internal class CompoundTokenStage : IMetaCodeBuilder
{
    public static CompoundTokenStage Instance = new CompoundTokenStage();
    public const string FunctionName = "Parse_Compound";

    public void WriteTo(MetaParserContext context)
    {
        var argumentType = SyntaxFactory.ParseTypeName($"{ReadOnlyMemory}<{TokenValueStructName}>");
        var resultsBuilderType = SyntaxFactory.ParseTypeName($"{List}<{TokenRecordTypeName}>");
        const string VarNameIdBuffer = "idValues";
        const string VarNameResults = "results";
        var writer = context.writer;

        writer.WriteLine($"private static {TokenRecordTypeName}[] {FunctionName}({argumentType} {VarNameBufferMajor})");
        writer.WriteLine("{");
        writer.Indent++;
        // Copy all of our value token ids into a uniform array in memory
        writer.WriteLine($"var {VarNameIdBuffer} = new {context.Config.IdType}[{VarNameBufferMajor}.Length];");
        writer.WriteLine($"for (int i = 0; i < {VarNameBufferMajor}.Length; i++)");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine($"{VarNameIdBuffer}[i] = {VarNameBufferMajor}.Span[i].Id;");
        writer.Indent--;
        writer.WriteLine("}");
        writer.WriteLine();

        writer.WriteLine($"var {VarNameBufferMinor} = new {ReadOnlyMemory}<{context.Config.IdType}>( {VarNameIdBuffer} );");
        writer.WriteLine($"var {VarNameBufferLocal} = {VarNameBufferMinor}.Span;");
        writer.WriteLine($"var {VarNameResults} = new {resultsBuilderType}();");
        writer.WriteLine();

        writer.WriteLine($"while ({VarNameBufferLocal}.Length > 0)");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine($"if ({CompoundTokenProcessorFunctionName}({VarNameBufferLocal}, out var outId, out var outLength))");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine($"var consumed = {VarNameBufferMajor}.Slice(0, outLength).ToArray();");
        writer.WriteLine($"{VarNameResults}.Add(new {TokenRecordTypeName}(({TokenEnum}) outId, consumed) );");
        writer.WriteLine();
        writer.WriteLine($"{VarNameBufferMajor} = {VarNameBufferMajor}.Slice(outLength);");
        writer.WriteLine($"{VarNameBufferMinor} = {VarNameBufferMinor}.Slice(outLength);");
        writer.WriteLine($"{VarNameBufferLocal} = {VarNameBufferMinor}.Span;");
        writer.Indent--;
        writer.WriteLine("}");
        writer.WriteLine("else");
        writer.WriteLine("{");
        writer.Indent++;
#if DEBUG
        writer.WriteLine("/* Proxy the current token as it has no special compound behavior */");
#endif
        writer.WriteLine($"var consumed = {VarNameBufferMajor}.Span[0];");
        var createNewToken = $"new {TokenRecordTypeName}(({TokenEnum}) consumed.Id, new[] {{ consumed }})";
        writer.WriteLine($"{VarNameResults}.Add({createNewToken});");
        writer.WriteLine($"{VarNameBufferMajor} = {VarNameBufferMajor}.Slice(1);");
        writer.WriteLine($"{VarNameBufferMinor} = {VarNameBufferMinor}.Slice(1);");
        writer.WriteLine($"{VarNameBufferLocal} = {VarNameBufferMinor}.Span;");
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
