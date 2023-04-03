using MetaParser.Builders.Interfaces;
using MetaParser.Core;
using MetaParser.Parsing.Constructs;

using System.Diagnostics;
using System.Linq;

namespace MetaParser.Builders.TokenLogic.Consumer;
using static CodeCommon;

internal class LogicSingleTokenDetector : MetaCodeBuilder
{
    protected override void Write(ParserContext context)
    {
        var writer = context.Writer;

        writer.WriteLine($"return {context.State.ActiveBufferName} switch");
        writer.WriteLine("{");
        writer.Indent++;

        Debug.Assert(context.State.Targets.Tokens.Length == 1);

        var targetToken = context.State.Targets.Tokens.Single();

        // TODO: Obsolete this once we move to the new lexer/parser split design
        // Check if its possible for the token to appear in the stream already from a lower stage.
        bool hasEarlierStages = targetToken.GetConsumers().Any(static (c) => c.Kind == EConsumerKind.Lexer);
        if (hasEarlierStages)
        {
            writer.WriteLine($"[{Format_Token_Id_Const_Ref(targetToken)}, ..] => true,");
        }

        var tokenConsumers = context.State.Targets.Consumers.Where(static (c) => c.Kind == EConsumerKind.Syntax);
        foreach (var consumer in tokenConsumers)
        {
            writer.Write("[");
            context.Config.CodeFactory.Get_Pattern_Writer().WriteTo(context with { State = context.State with { Targets = new WorkingSet(consumer.Token, consumer, consumer.Start) } });
            writer.Write(", ");

            if (consumer.Consume is not null)
            {
                writer.Write($"var {context.State.NextBufferName}] when (");

                bool first = true;
                foreach (var pattern in consumer.Consume)
                {
                    //Debug.Assert(pattern is PatternTokenRef);
                    if (!first)
                    {
                        writer.Write(" or ");
                    }
                    first = false;

                    if (pattern is PatternTokenRef tokenRef)
                    {
                        writer.Write($"{Format_Token_Start_Detection_Function_Name(tokenRef.TokenName)}({context.State.NextBufferName})");
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
