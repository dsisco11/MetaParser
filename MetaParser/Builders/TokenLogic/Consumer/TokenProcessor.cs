using MetaParser.Builders.Interfaces;
using MetaParser.Consumers;
using MetaParser.Core;
using MetaParser.Parsing.Constructs.Consumers;
using MetaParser.Parsing.Constructs.Core;

using System.Linq;

namespace MetaParser.Builders.TokenLogic.Consumer;
using static CodeCommon;

internal class TokenProcessor : MetaCodeBuilder
{

    protected override void Write(MetaParserContext context)
    {
        var writer = context.writer;

        #region Token Processing Logic

        var groups = context.WorkingSet.Consumers.GroupBy(static c => c.DependencyInfo.IsRecursive && c.Consume is not null);

        var consumersRecursive = groups.Where(static b => b.Key == true).SelectMany(static b => b);
        if (consumersRecursive.Any())
        {
            context.Config.CodeFactory.Get_Logic_Detect_Recursive_Token()
                                      .And(new ExecuteConsumerAndReturnResult())
                                      .WriteTo(context with { WorkingSet = context.WorkingSet with { Consumers = consumersRecursive.ToArray() } });
        }

        var consumersLinear = groups.Where(static b => b.Key == false).SelectMany(static b => b);
        if (consumersLinear.Any())
        {
            context.Config.CodeFactory.Get_Logic_Detect_Linear_Token()
                                      .And(new ExecuteConsumerAndReturnResult())
                                      .WriteTo(context with { WorkingSet = context.WorkingSet with { Consumers = consumersLinear.ToArray() } });
        }

        // return failure
        writer.WriteLine("id = default;");
        writer.WriteLine("length = default;");
        writer.WriteLine("return false;");
        writer.WriteLine();
        #endregion

        #region Local Sub-Functions

        // generate token start detection
        if (consumersRecursive.Any())
        {
            var referencedTokens = consumersRecursive.SelectMany(static (c) => c.DependencyInfo.Outgoing.Where(static (n) => n.Key.Type == Graphs.GraphNodeType.Token)).Distinct();
            var requiredStartDetectors = context.Registry.Tokens.Values.Where(static (t) => t.DependencyInfo!.Incoming.Any(static (n) => n.IsRecursive));
            foreach (var token in requiredStartDetectors)
            {
                var funcName = Format_Token_Start_Detection_Function_Name(token.Name);
                var funcDef = Get_Local_Token_Detection_Function_Definition(context.Config, EConsumerType.Token, funcName);

                var lesserConsumers = token.GetConsumers().Where(static (c) => c.DependencyInfo.MinDepth < 2).ToArray();
                funcDef.WriteTo(context with { WorkingSet = context.WorkingSet with { Consumers = lesserConsumers } });
            }
        }

        foreach (Parsing.Constructs.Consumers.Consumer consumer in consumersRecursive)
        {
            var funcName = Format_Token_Start_Detection_Function_Name(consumer.Token.Name);
            var funcDef = Get_Local_Token_Detection_Function_Definition(context.Config, consumer.Type, funcName);
            var lesserConsumers = consumer.Token.GetConsumers().Where((c) => c.DependencyInfo.MinDepth <= consumer.DependencyInfo.MinDepth).ToArray();
            funcDef.WriteTo(context with { WorkingSet = context.WorkingSet with { Consumers = lesserConsumers } });
        }

        // generate token consumer functions
        var workingContext = context with { WorkingSet = context.WorkingSet with { Consumers = new Parsing.Constructs.Consumers.Consumer[1] } };
        foreach (Parsing.Constructs.Consumers.Consumer consumer in context.WorkingSet.Consumers)
        {
            workingContext.WorkingSet.Consumers[0] = consumer;
            if (consumer.IsConstant)
            {
                continue;// skip const patterns as they get an inline fast-path
            }

            var funcName = Format_Pattern_Consumer_Function_Name(consumer.NodeID.Index);
            var funcDef = Get_Local_Token_Consumer_Function_Definition(context.Config, consumer.Type, funcName);
            funcDef.WriteTo(workingContext);
        }
        #endregion
    }
}
