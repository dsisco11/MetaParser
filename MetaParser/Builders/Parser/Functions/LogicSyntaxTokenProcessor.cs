using MetaParser.Builders.Interfaces;
using MetaParser.Core;

using Microsoft.CodeAnalysis.CSharp;

namespace MetaParser.Builders.Parser.Functions;
using static CodeCommon;

internal class LogicSyntaxTokenProcessor : MetaCodeBuilder
{
    public const string FunctionName = "Parse_Compound";

    protected override void Write(MetaParserContext context)
    {
        var argumentType = SyntaxFactory.ParseTypeName($"{ReadOnlyMemory}<{TokenValueStructName}>");
        var resultsBuilderType = SyntaxFactory.ParseTypeName($"{List}<{TokenRecordTypeName}>");
        const string VarNameIdBuffer = "idValues";
        const string VarNameResults = "results";
        var writer = context.Writer;

        string VarBufferMajor = context.ActiveBufferName;
        context.ActiveBuffer++;
        string VarBufferMinor = context.ActiveBufferName;
        context.ActiveBuffer++;
        string VarBufferLocal = context.ActiveBufferName;
        context.ActiveBuffer--;

        writer.WriteLine($"private static {TokenRecordTypeName}[] {FunctionName}({argumentType} {VarBufferMajor})");
        writer.WriteLine("{");
        writer.Indent++;
        // Copy all of our value token ids into a uniform array in memory
        writer.WriteLine($"var {VarNameIdBuffer} = new {context.Config.IdType}[{VarBufferMajor}.Length];");
        writer.WriteLine($"for (int i = 0; i < {VarBufferMajor}.Length; i++)");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine($"{VarNameIdBuffer}[i] = {VarBufferMajor}.Span[i].Id;");
        writer.Indent--;
        writer.WriteLine("}");
        writer.WriteLine();

        writer.WriteLine($"var {VarBufferMinor} = new {ReadOnlyMemory}<{context.Config.IdType}>( {VarNameIdBuffer} );");
        writer.WriteLine($"var {VarBufferLocal} = {VarBufferMinor}.Span;");
        writer.WriteLine($"var {VarNameResults} = new {resultsBuilderType}();");
        writer.WriteLine();

        writer.WriteLine($"while ({VarBufferLocal}.Length > 0)");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine($"if ({SyntaxProcessingFunctionName}({VarBufferLocal}, out var outId, out var outLength))");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine($"var consumed = {VarBufferMajor}.Slice(0, outLength).ToArray();");
        writer.WriteLine($"{VarNameResults}.Add(new {TokenRecordTypeName}(({TokenEnum}) outId, consumed) );");
        writer.WriteLine();
        writer.WriteLine($"{VarBufferMajor} = {VarBufferMajor}.Slice(outLength);");
        writer.WriteLine($"{VarBufferMinor} = {VarBufferMinor}.Slice(outLength);");
        writer.WriteLine($"{VarBufferLocal} = {VarBufferMinor}.Span;");
        writer.Indent--;
        writer.WriteLine("}");
        writer.WriteLine("else");
        writer.WriteLine("{");
        writer.Indent++;
#if DEBUG
        writer.WriteLine("/* Proxy the current token as it has no special compound behavior */");
#endif
        writer.WriteLine($"var consumed = {VarBufferMajor}.Span[0];");
        var createNewToken = $"new {TokenRecordTypeName}(({TokenEnum}) consumed.Id, new[] {{ consumed }})";
        writer.WriteLine($"{VarNameResults}.Add({createNewToken});");
        writer.WriteLine($"{VarBufferMajor} = {VarBufferMajor}.Slice(1);");
        writer.WriteLine($"{VarBufferMinor} = {VarBufferMinor}.Slice(1);");
        writer.WriteLine($"{VarBufferLocal} = {VarBufferMinor}.Span;");
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
