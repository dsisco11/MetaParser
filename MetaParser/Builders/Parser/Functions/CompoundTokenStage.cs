using MetaParser.CodeGen;
using MetaParser.CodeGen.Core;
using MetaParser.Contexts;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace MetaParser.Builders.Parser.Functions;

internal class CompoundTokenStage : IMetaCodeBuilder
{
    public static CompoundTokenStage Instance = new CompoundTokenStage();
    public const string FunctionName = "Parse_Compound";

    public void WriteTo(MetaParserContext context)
    {
        var argumentType = SyntaxFactory.ParseTypeName($"{CodeCommon.ReadOnlyMemory}<{MetaParserContext.TokenValueClassName}>");
        var resultsBuilderType = SyntaxFactory.ParseTypeName($"{CodeCommon.List}<{MetaParserContext.TokenClassName}>");
        const string VarNameIdBuffer = "idValues";
        const string VarNameResults = "results";
        const string VarNameOffset = "offset";
        var writer = context.writer;

        writer.WriteLine($"private static {MetaParserContext.TokenClassName}[] {FunctionName}({argumentType} {MetaParserContext.VarNameBufferMajor})");
        writer.WriteLine("{");
        writer.Indent++;
        // Copy all of our value token ids into a uniform array in memory
        writer.WriteLine($"var {VarNameIdBuffer} = new {context.IdTypeName}[{MetaParserContext.VarNameBufferMajor}.Length];");
        writer.WriteLine($"for (int i = 0; i < {MetaParserContext.VarNameBufferMajor}.Length; i++)");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine($"{VarNameIdBuffer}[i] = {MetaParserContext.VarNameBufferMajor}.Span[i].Id;");
        writer.Indent--;
        writer.WriteLine("}");
        writer.WriteLine();

        writer.WriteLine($"var {MetaParserContext.VarNameBufferMinor} = new {CodeCommon.FormatReadOnlyMemoryBuffer(context.IdType)}( {VarNameIdBuffer} );");
        writer.WriteLine($"var {MetaParserContext.VarNameBufferLocal} = {MetaParserContext.VarNameBufferMinor}.Span;");
        writer.WriteLine($"var {VarNameResults} = new {resultsBuilderType}();");
        writer.WriteLine($"{CodeCommon.Format(SpecialType.System_Int32)} {VarNameOffset} = 0;");
        writer.WriteLine();

        writer.WriteLine($"while ({MetaParserContext.VarNameBufferLocal}.Length > 0)");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine($"if ({MetaParserContext.CompoundTokenProcessorFunctionName}({MetaParserContext.VarNameBufferLocal}, out var outId, out var outLength))");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine();
        writer.WriteLine($"var consumed = {MetaParserContext.VarNameBufferMajor}.Slice({VarNameOffset}, outLength).ToArray();");
        writer.WriteLine($"{VarNameResults}.Add(new {MetaParserContext.TokenClassName}(({MetaParserContext.TokenEnum}) outId, consumed) );");
        writer.WriteLine();
        writer.WriteLine($"{VarNameOffset} += outLength;");
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
        writer.WriteLine($"var proxyValue = {MetaParserContext.VarNameBufferMajor}.Span[offset];");
        var createNewToken = $"new {MetaParserContext.TokenClassName}(({MetaParserContext.TokenEnum}) proxyValue.Id, new[] {{ proxyValue }})";
        writer.WriteLine($"{VarNameResults}.Add({createNewToken});");
        writer.WriteLine($"{VarNameOffset} += 1;");
        writer.WriteLine($"{MetaParserContext.VarNameBufferLocal} = {MetaParserContext.VarNameBufferLocal}.Slice(1);");
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
