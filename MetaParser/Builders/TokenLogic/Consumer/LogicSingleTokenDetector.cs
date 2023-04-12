using MetaParser.Builders.Interfaces;
using MetaParser.Core;
using MetaParser.Parsing.Constructs;

using System;
using System.Diagnostics;
using System.Linq;

namespace MetaParser.Builders.TokenLogic.Consumer;
using static CodeCommon;

internal class LogicSingleTokenDetector : MetaCodeBuilder
{
    protected override void Write(ParserContext context)
    {
        var writer = context.Writer ?? throw new InvalidOperationException("Writer is null");

        writer.WriteLine($"return {context.State.ActiveBufferName} switch");
        writer.WriteLine("{");
        writer.Indent++;

        Debug.Assert(context.State.Targets.Tokens.Length == 1);

        var targetToken = context.State.Targets.Tokens.Single();
        var tokenConsumers = context.State.Targets.Consumers;

        foreach (var consumer in tokenConsumers)
        {
            writer.Write("[");
            context.Config.CodeFactory.Get_Pattern_Writer().WriteTo(context with { State = context.State with { Targets = new WorkingSet(consumer.Token, consumer, consumer.Start) } });
            writer.Write(", ");

            //if (consumer.Consume is not null)
            //{
            //    writer.Write($"var {context.State.NextBufferName}] when (");

            //    bool first = true;
            //    foreach (var pattern in consumer.Consume)
            //    {
            //        //Debug.Assert(pattern is PatternTokenRef);
            //        if (!first)
            //        {
            //            writer.Write(" or ");
            //        }
            //        first = false;

            //        // TODO: We only have to check this token with a function IF the token is part of the same stage as us
            //        if (pattern is PatternTokenRef tokenRef && tokenRef.GetToken().GraphInfo.Depth >= targetToken.GraphInfo.Depth)
            //        {
            //            writer.Write($"{Format_Token_Start_Detection_Function_Name(tokenRef.TokenName)}({context.State.NextBufferName})");
            //        }
            //        else
            //        {
            //            context.Config.CodeFactory.Get_Pattern_Writer().WriteTo(context with { State = context.State with { Targets = new WorkingSet(pattern) } });
            //        }
            //    }
            //    writer.Write(")");
            //}
            //else
            //{
            //    writer.Write("..]");
            //}
            writer.Write("..]");

            writer.WriteLine(" => true,");
        }
        writer.WriteLine("_ => false");
        writer.Indent--;
        writer.WriteLine("};");
    }
}
