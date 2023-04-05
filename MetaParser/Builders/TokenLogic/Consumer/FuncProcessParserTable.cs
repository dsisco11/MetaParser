using MetaParser.Builders.Core;
using MetaParser.Builders.Interfaces;
using MetaParser.Core;
using Microsoft.CodeAnalysis.CSharp;

using System;
using System.Linq;

namespace MetaParser.Builders.TokenLogic.Consumer;

internal class FuncProcessParserTable : MetaCodeBuilder, IMetaCodeFunctionBuilder
{
    private static readonly FunctionLogic functionLogic = new();

    public string Format_Function_Name(ParserContext context) => $"process_parser_table_{context.State.Stage.Index}";
    public FunctionDefinition Get_Definition(ParserContext context)
    {
        var stage = context.State.Stage;
        var funcParams = SyntaxFactory.ParseArgumentList($"{CodeCommon.ReadOnlySpan}<{stage.InputType}> {context.State.ActiveBufferName}");
        return new FunctionDefinition(CodeCommon.SyntaxPrivateStatic, stage.OutputType, Format_Function_Name(context), funcParams);
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

            var groups = context.State.Targets.Consumers.GroupBy(static c => c.DependencyInfo!.IsRecursive && c.Consume is not null);

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
                context.Config.CodeFactory.Get_Switch_Block_For_Consumers()
                                          .And(new ExecuteConsumer())
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
