using MetaParser.Builders.Core;
using MetaParser.Builders.Interfaces;
using MetaParser.Core;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System;
using System.Linq;

namespace MetaParser.Builders.TokenLogic.Consumer;
using static CodeCommon;

internal class FuncParseNextChunk : MetaCodeBuilder, IMetaCodeFunctionBuilder
{
    private static readonly FunctionLogic functionLogic = new();

    public string Format_Function_Name(ParserContext context) => $"process_next_chunk";
    public FunctionDefinition Get_Definition(ParserContext context)
    {
        var stage = context.State.Stage;
        var funcParams = SyntaxFactory.ParseArgumentList($"{ReadOnlyMemory}<{context.Config.InputType}> {context.State.ActiveBufferName}");
        TypeSyntax outputType = SyntaxFactory.ParseTypeName($"IEnumerable<{TokenRecordTypeName}>");
        return new FunctionDefinition(SyntaxPrivateStatic, outputType, Format_Function_Name(context), funcParams);
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
            var writer = context.Writer ?? throw new InvalidOperationException("Writer is null");

            context.Config.CodeFactory.Get_Switch_Block_For_Consumers()
                                      .And(LogicReturnDroppedConsumer.Instance)
                                      .WriteTo(context with { State = context.State with { Targets = new WorkingSet(context.State.Stage.Dropped) } });


            var groups = context.State.Targets.Consumers.GroupBy(static c => c.GraphInfo!.IsRecursive && c.Consume is not null);
            var consumersRecursive = groups.Where(static b => b.Key == true).SelectMany(static b => b);
            if (consumersRecursive.Any())
            {
                context.Config.CodeFactory.Get_Logic_Detect_Recursive_Token()
                                          .And(new ExecuteConsumerAndReturn())
                                          .WriteTo(context with { State = context.State with { Targets = new WorkingSet(consumersRecursive.ToArray()) } });
            }

            var consumersLinear = groups.Where(static b => b.Key == false).SelectMany(static b => b);
            if (consumersLinear.Any())
            {
                context.Config.CodeFactory.Get_Switch_Return_For_Consumers()
                                          .And(LogicExecuteConsumer.Instance)
                                          .WriteTo(context with { State = context.State with { Targets = new WorkingSet(consumersLinear.ToArray()) } });
            }
            else
            {
                writer.WriteLine("return new (default, default);");
            }

            writer.WriteLine();
        }
    }
}
