using MetaParser.Builders.Interfaces;
using MetaParser.Core;

using Microsoft.CodeAnalysis.CSharp;

using System;
using System.Collections.Generic;

namespace MetaParser.Builders.Parser.Functions;
using static CodeCommon;

internal class ParsingLogic : MetaCodeBuilder
{
    protected override void Write(ParserContext context)
    {
        var writer = context.Writer ?? throw new InvalidOperationException("Writer is null");

        var tyInputBuffer = SyntaxFactory.ParseTypeName($"{ReadOnlyMemory}<{context.Config.InputType}>");
        var tyOutput = SyntaxFactory.ParseTypeName($"{TypeRedGreenTree}");
        var tyGreenNode = SyntaxFactory.ParseTypeName($"{TypeGreenNode}");
        var tyGreenNodeArraySegment = SyntaxFactory.ParseTypeName($"{ArraySegment}<{TypeGreenNode}>");

        writer.Write("public ");
        writer.WriteLine($"{tyOutput} Parse({tyInputBuffer} {context.State.ActiveBufferName})");
        writer.WriteLine("{");
        writer.Indent++;
        var inputVarName = $"input{context.State.ActiveBuffer}";
        var nextInputBuffer = context.State.ActiveBufferName;
        string stageNodesVar = string.Empty;

        foreach (var stage in context.Stages)
        {
            writer.WriteLine($"{StageInputChunk}<{stage.InputType}, {stage.OutputType}> {inputVarName} = new({nextInputBuffer});");
            var ctx = context with { State = context.State with { Stage = stage } };
            var executor = (IMetaCodeFunctionBuilder)context.Config.CodeFactory.Get_Parsing_Table_Executor();
            var functionName = executor.Format_Function_Name(ctx);
            var outputVarName = $"output{context.State.ActiveBuffer}";

            writer.WriteLine($"var {outputVarName} = {functionName}({inputVarName});");

            // Build the next tree layer
            stageNodesVar = $"stage_{stage.Index}_nodes";
            var stageNodesPreviousVar = $"stage_{stage.Index-1}_nodes";
            var stageNodeTrackerVar = $"stage_{stage.Index}_node_tracker";
            var stageLengthVar = $"stage_{stage.Index}_length";

            writer.WriteLine($"var {stageLengthVar} = {outputVarName}.TokenCount;");
            writer.WriteLine($"var {stageNodesVar} = new {tyGreenNode}[{stageLengthVar}];");
            writer.WriteLine($"int {stageNodeTrackerVar} = 0;");

            writer.WriteLine($"for (int i=0; i<{stageLengthVar}; i++)");
            writer.WriteLine("{");
            writer.Indent++;
            writer.WriteLine($"{TokenNodeType} token;");
            writer.WriteLine($"var tokenId = {inputVarName}.Output.Span[i];");
            writer.WriteLine($"var tokenWidth = {inputVarName}.Length.Span[i];");
            if (stage.Index <= 0)
            {
                writer.WriteLine($"var tokenData = {inputVarName}.Input.Slice({stageNodeTrackerVar}, tokenWidth);");
                writer.WriteLine($"token = new {ValueTokenNodeType}(tokenId, tokenData);");
            }
            else
            {
                writer.WriteLine("if (tokenId != 0)");
                writer.WriteLine("{");
                writer.Indent++;
                writer.WriteLine($"var children = new {tyGreenNodeArraySegment}({stageNodesPreviousVar}, {stageNodeTrackerVar}, tokenWidth);");
                writer.WriteLine($"token = new {SyntaxTokenNodeType}(tokenId, children.Array);");
                writer.Indent--;
                writer.WriteLine("}");
                writer.WriteLine("else");
                writer.WriteLine("{");
                writer.Indent++;
                // Forwards the token to the next stage
                writer.WriteLine($"token = {stageNodesPreviousVar}[{stageNodeTrackerVar}];");
                writer.Indent--;
                writer.WriteLine("}");

            }
            writer.WriteLine($"{stageNodesVar}[i] = token;");
            writer.WriteLine($"{stageNodeTrackerVar} += tokenWidth;");

            writer.Indent--;
            writer.WriteLine("}");
            writer.WriteLine();

            // resize the buffer so it matches what the last stage actually used
            context.Increment_Active_Bufffer();
            nextInputBuffer = context.State.ActiveBufferName;
            writer.WriteLine($"var {nextInputBuffer} = {inputVarName}.Output.Slice(0, {outputVarName}.TokenCount);");

            context.Increment_Active_Bufffer();
            inputVarName = $"input{context.State.ActiveBuffer}";
        }

        writer.WriteLine($"var rootNode = new {SyntaxTokenNodeType}({TokenEnum}.{UnknownToken}, {stageNodesVar});");
        writer.WriteLine($"return new {TypeRedGreenTree}(rootNode);");
        writer.WriteLine();

        writer.Indent--;
        writer.WriteLine("}");// end function
    }
}
