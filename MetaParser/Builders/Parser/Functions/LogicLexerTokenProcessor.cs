using MetaParser.Builders.Interfaces;
using MetaParser.Core;
using MetaParser.Parsing.Constructs;

using Microsoft.CodeAnalysis.CSharp;

namespace MetaParser.Builders.Parser.Functions;
using static CodeCommon;

internal class LogicLexerTokenProcessor : MetaCodeBuilder
{
    public const string FunctionName = "Parse_Constant";

    protected override void Write(MetaParserContext context)
    {
        var argumentType = SyntaxFactory.ParseTypeName($"{ReadOnlyMemory}<{Get_Consumer_Data_Type(context.Config, EConsumerType.Lexer)}>");
        var resultsBuilderType = SyntaxFactory.ParseTypeName($"{List}<{TokenValueStructName}>");
        const string VarNameResults = "results";
        const string VarNameProcesserReturn = "processed";
        var writer = context.Writer;

        string VarBufferMajor = context.ActiveBufferName;
        context.ActiveBuffer++;
        string VarBufferMinor = context.ActiveBufferName;
        context.ActiveBuffer++;
        string VarBufferLocal = context.ActiveBufferName;
        context.ActiveBuffer--;

        writer.WriteLine($"private static {TokenValueStructName}[] {FunctionName}({argumentType} {VarBufferMajor})");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine($"var {VarBufferMinor} = {VarBufferMajor};");
        writer.WriteLine($"var {VarBufferLocal} = {VarBufferMinor}.Span;");
        writer.WriteLine($"var {VarNameResults} = new {resultsBuilderType}();");
        writer.WriteLine();

        writer.WriteLine($"while ({VarBufferLocal}.Length > 0)");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine($"var {VarNameProcesserReturn} = {LexerProcessingFunctionName}({VarBufferLocal}))");
        writer.WriteLine($"if ({VarNameProcesserReturn}.length != default)");
        writer.WriteLine("{");
        writer.Indent++;
        // Be sure to push unknown token if its lingering
        UnknownTokenPusher.Instance.WriteTo(context);
        writer.WriteLine();
        writer.WriteLine($"var consumed = {VarBufferMinor}.Slice(0, {VarNameProcesserReturn}.length);");
        writer.WriteLine($"{VarNameResults}.Add( new {TokenValueStructName}({VarNameProcesserReturn}.id, consumed) );");
        writer.WriteLine($"{VarBufferMinor} = {VarBufferMinor}.Slice({VarNameProcesserReturn}.length);");
        writer.WriteLine($"{VarBufferLocal} = {VarBufferMinor}.Span;");
        writer.Indent--;
        writer.WriteLine("}");
        writer.WriteLine("else");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine($"{VarBufferLocal} = {VarBufferLocal}.Slice(1);");
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
