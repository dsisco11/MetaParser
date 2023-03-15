using MetaParser.Builders.Interfaces;
using MetaParser.Core;
using MetaParser.Parsing.Constructs;

using System.Diagnostics;
using System.Linq;

namespace MetaParser.Builders.TokenLogic.Consumer;
using static CodeCommon;

internal class LogicDetectPatternRecursive : MetaCodeBuilder
{
    protected override void Write(MetaParserContext context)
    {
        var writer = context.writer;

        writer.WriteLine($"return {context.ActiveBufferName} switch");
        writer.WriteLine("{");
        writer.Indent++;

        Debug.Assert(context.WorkingSet.Tokens.Length == 1);

        var targetToken = context.WorkingSet.Tokens.Single();

        // Check if its possible for the token to appear in the stream already from a lower stage.
        bool hasEarlierStages = targetToken.GetConsumers().Any(static (c) => c.Type == EConsumerType.Data);
        //bool hasEarlierStages = targetToken.GetConsumers().Any(static (c) => c.DependencyInfo.Depth[(int)NodeType.Consumer].Min < );
        if (hasEarlierStages)
        {
            writer.WriteLine($"[{Format_Token_Id_Const_Ref(targetToken)}, ..] => true,");
        }

        var tokenConsumers = context.WorkingSet.Consumers.Where(static (c) => c.Type == EConsumerType.Token);
        foreach (var consumer in tokenConsumers)
        {
            writer.Write("[");
            context.Config.CodeFactory.Get_Logic_Pattern_Matching_Switch_Clause().WriteTo(context with { WorkingSet = new WorkingSet(consumer.Token, consumer, consumer.Start) });
            writer.Write(", ");

            if (consumer.Consume is not null)
            {
                writer.Write($"var {context.NextBufferName}] when (");

                bool first = true;
                foreach (var pattern in consumer.Consume)
                {
                    Debug.Assert(pattern is PatternTokenRef);
                    if (!first)
                    {
                        writer.Write(" or ");
                    }
                    first = false;

                    if (pattern is PatternTokenRef tokenRef)
                    {
                        writer.Write($"{Format_Token_Start_Detection_Function_Name(tokenRef.TokenName)}({context.NextBufferName})");
                    }
                }
                writer.Write(")");
            }
            else
            {
                writer.Write("..]");
            }

            writer.WriteLine(" => true,");
        }
        writer.WriteLine("_ => false");
        writer.Indent--;
        writer.WriteLine("};");
    }
}
