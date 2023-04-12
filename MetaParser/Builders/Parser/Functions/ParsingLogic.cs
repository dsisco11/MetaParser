using MetaParser.Builders.Interfaces;
using MetaParser.Core;

using Microsoft.CodeAnalysis.CSharp;

using System;

namespace MetaParser.Builders.Parser.Functions;
using static CodeCommon;

internal class ParsingLogic : MetaCodeBuilder
{
    protected override void Write(ParserContext context)
    {
        var writer = context.Writer ?? throw new InvalidOperationException("Writer is null");

        var tyInputBuffer = SyntaxFactory.ParseTypeName($"{ReadOnlyMemory}<{context.Config.InputType}>");
        var tyTokenList = SyntaxFactory.ParseTypeName($"{TokenRecordTypeName}[]");
        // TODO: we should be outputting an AST red-green tree

        writer.Write("public ");
        writer.WriteLine($"{tyTokenList} Parse({tyInputBuffer} {context.State.ActiveBufferName})");
        writer.WriteLine("{");
        writer.Indent++;
        var inputVarName = $"input{context.State.ActiveBuffer}";
        var nextInputBuffer = context.State.ActiveBufferName;
        foreach (var stage in context.Stages)
        {
            writer.WriteLine($"{StageInputChunk}<{stage.InputType}, {stage.OutputType}> {inputVarName} = new({nextInputBuffer});");
            var ctx = context with { State = context.State with { Stage = stage } };
            var executor = (IMetaCodeFunctionBuilder)context.Config.CodeFactory.Get_Parsing_Table_Executor();
            var functionName = executor.Format_Function_Name(ctx);
            var outputVarName = $"output{context.State.ActiveBuffer}";

            writer.WriteLine($"var {outputVarName} = {functionName}({inputVarName});");
            context.Increment_Active_Bufffer();

            nextInputBuffer = $"{inputVarName}.Outputs";
            inputVarName = $"input{context.State.ActiveBuffer}";
        }

        writer.WriteLine($"return Array.Empty<{TokenRecordTypeName}>();");
        writer.WriteLine();

        writer.Indent--;
        writer.WriteLine("}");// end function
    }
}
