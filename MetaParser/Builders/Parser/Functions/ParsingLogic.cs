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
        foreach (var stage in context.Stages)
        {
            var ctx = context with { State = context.State with { Stage = stage } };
            var executor = (IMetaCodeFunctionBuilder)context.Config.CodeFactory.Get_Parsing_Table_Executor();
            var functionName = executor.Format_Function_Name(ctx);
            writer.WriteLine($"var {context.State.NextBufferName} = {functionName}({context.State.ActiveBufferName});");
            context.Increment_Active_Bufffer();
        }

        writer.WriteLine($"return {context.State.ActiveBufferName};");
        writer.WriteLine();

        writer.Indent--;
        writer.WriteLine("}");// end function
    }
}
