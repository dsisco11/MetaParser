using MetaParser.Builders.Interfaces;
using MetaParser.Core;

using Microsoft.CodeAnalysis.CSharp;

namespace MetaParser.Builders.Parser.Functions;
using static CodeCommon;

internal class FuncParsingTableExecutor : MetaCodeBuilder, IMetaCodeFunctionBuilder
{
    public string Get_Function_Name(ParserContext context) => $"Execute_Parsing_Table_{context.State.Stage.Index}";

    protected override void Write(ParserContext context)
    {
        var stage = context.State.Stage;
        var argumentType = SyntaxFactory.ParseTypeName($"{ReadOnlyMemory}<{stage.InputType}>");
        var resultsBuilderType = SyntaxFactory.ParseTypeName($"{List}<{stage.OutputType}>");
        const string VarNameResults = "results";
        const string VarNameProcesserReturn = "processed";
        var writer = context.Writer!;

        string VarBufferMajor = context.State.ActiveBufferName;
        context.Increment_Active_Bufffer();
        string VarBufferMinor = context.State.ActiveBufferName;
        context.Increment_Active_Bufffer();
        string VarBufferLocal = context.State.ActiveBufferName;
        context.Decrement_Active_Bufffer();

        writer.WriteLine($"private static {stage.OutputType}[] {Get_Function_Name(context)}({argumentType} {VarBufferMajor})");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine($"var {VarBufferMinor} = {VarBufferMajor};");
        writer.WriteLine($"var {VarBufferLocal} = {VarBufferMinor}.Span;");
        writer.WriteLine($"var {VarNameResults} = new {resultsBuilderType}();");
        writer.WriteLine();

        writer.WriteLine($"while ({VarBufferLocal}.Length > 0)");
        writer.WriteLine("{");
        writer.Indent++;
        var funcBuilder = (IMetaCodeFunctionBuilder)context.Config.CodeFactory.Get_Parsing_Table_Function();
        writer.WriteLine($"var {VarNameProcesserReturn} = {funcBuilder.Get_Function_Name(context)}({VarBufferLocal});");
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
