using MetaParser.Builders.Core;
using MetaParser.Builders.Interfaces;
using MetaParser.Core;

using Microsoft.CodeAnalysis.CSharp;

namespace MetaParser.Builders.Parser.Functions;
using static CodeCommon;

internal class FuncParsingTableExecutor : MetaCodeBuilder, IMetaCodeFunctionBuilder
{
    private static readonly FunctionLogic functionLogic = new();
    public string Format_Function_Name(ParserContext context) => $"Execute_Parsing_Table_{context.State.Stage.Index}";

    public FunctionDefinition Get_Definition(ParserContext context)
    {
        var stage = context.State.Stage;
        var funcInput = SyntaxFactory.ParseArgumentList($"{StageInputChunk}<{stage.InputType}, {stage.OutputType}> {context.State.ActiveBufferName}");
        return new FunctionDefinition(SyntaxPrivateStatic, TypeStageOutput, Format_Function_Name(context), funcInput);
    }

    protected override void Write(ParserContext context)
    {
        var definition = Get_Definition(context);
        definition.And(functionLogic);
        definition.WriteTo(context);
    }

    private class FunctionLogic : FunctionBodyBuilder
    {
        protected override void Write(ParserContext context)
        {
            var stage = context.State.Stage;
            var resultsBuilderType = stage.OutputType;
            const string VarNameProcesserReturn = "processed";
            const string VarNameOutputIndex = "outIndex";
            const string VarNameInputIndex = "inIndex";
            const string VarNameOutputId = "outId";
            const string VarNameOutputLength = "outLength";
            var writer = context.Writer!;

            string VarBufferMajor = context.State.ActiveBufferName;
            context.Increment_Active_Bufffer();
            string VarBufferMinor = context.State.ActiveBufferName;
            context.Increment_Active_Bufffer();
            string VarBufferLocal = context.State.ActiveBufferName;
            context.Decrement_Active_Bufffer();

            writer.WriteLine($"var {VarBufferMinor} = {VarBufferMajor}.Input;");
            writer.WriteLine($"var {VarBufferLocal} = {VarBufferMinor}.Span;");
            writer.WriteLine($"int {VarNameInputIndex} = 0;");
            writer.WriteLine($"int {VarNameOutputIndex} = 0;");
            writer.WriteLine($"var {VarNameOutputId} = {VarBufferMajor}.Output;");
            writer.WriteLine($"var {VarNameOutputLength} = {VarBufferMajor}.Length;");
            writer.WriteLine();

            writer.WriteLine($"while ({VarBufferLocal}.Length > 0)");
            writer.WriteLine("{");
            writer.Indent++;
            var funcBuilder = (IMetaCodeFunctionBuilder)context.Config.CodeFactory.Get_Parsing_Table_Function();
            writer.WriteLine($"var {VarNameProcesserReturn} = {funcBuilder.Format_Function_Name(context)}({VarBufferLocal});");
            writer.WriteLine($"if ({VarNameProcesserReturn}.length != default)");
            writer.WriteLine("{");
            writer.Indent++;
            // Be sure to push unknown token if its lingering
            UnknownTokenPusher.Instance.WriteTo(context);
            writer.WriteLine();
            writer.WriteLine($"var consumed = {VarBufferMinor}.Slice(0, {VarNameProcesserReturn}.length);");
            //writer.WriteLine($"{VarNameOutput}.Add( new {TokenValueStructName}({VarNameProcesserReturn}.id, consumed) );");
            writer.WriteLine($"{VarNameOutputId}.Span[{VarNameOutputIndex}] = {VarNameProcesserReturn}.id;");
            writer.WriteLine($"{VarNameOutputLength}.Span[{VarNameOutputIndex}] = {VarNameProcesserReturn}.length;");
            writer.WriteLine($"{VarNameInputIndex} += {VarNameProcesserReturn}.length;");
            writer.WriteLine($"{VarNameOutputIndex}++;");
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
            writer.WriteLine($"return new ({VarNameOutputIndex}, {VarNameInputIndex});");
        }
    }
}
