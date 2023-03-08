using MetaParser.Builders.Interfaces;
using MetaParser.Consumers;
using MetaParser.Core;

using System.Linq;

namespace MetaParser.Builders.TokenLogic.Consumer;
using static CodeCommon;

internal class TokenProcessor : MetaCodeBuilder
{
    protected override void Write(MetaParserContext context)
    {
        var writer = context.writer;

        #region Token Processing Logic

        var consumersRecursive = context.Consumers.WorkingSet.Where(static c => c.DependencyInfo.IsRecursive);
        if (consumersRecursive.Any())
        {
            context.Config.CodeFactory.Get_Logic_Detect_Recursive_Token()
                                      .And(new ExecuteConsumerAndReturnResult())
                                      .WriteTo(context with { Consumers = context.Consumers with { WorkingSet = consumersRecursive.ToArray() } });
        }

        var consumersLinear = context.Consumers.WorkingSet.Where(static c => !c.DependencyInfo.IsRecursive);
        if (consumersLinear.Any())
        {
            context.Config.CodeFactory.Get_Logic_Detect_Linear_Token()
                                      .And(new ExecuteConsumerAndReturnResult())
                                      .WriteTo(context with { Consumers = context.Consumers with { WorkingSet = consumersLinear.ToArray() } });
        }

        // return failure
        writer.WriteLine("id = default;");
        writer.WriteLine("length = default;");
        writer.WriteLine("return false;");
        writer.WriteLine();
        #endregion

        #region Local Sub-Functions
        var workingContext = context with { Consumers = context.Consumers with { WorkingSet = new TokenConsumer[1] } };

        // write recursive consumer function generation 
        foreach (TokenConsumer consumer in consumersRecursive)
        {
            workingContext.Consumers.WorkingSet[0] = consumer;
            if (consumer.IsConstant)
            {
                continue;// skip const patterns as they get an inline fast-path
            }

            var funcName = Format_Pattern_Start_Detection_Function_Name(consumer.Index);
            var funcDef = Get_Local_Pattern_Start_Detection_Function_Definition(context.Config, consumer.Type, funcName);
            funcDef.WriteTo(workingContext);
        }
        
        // write linear consumer function generation
        foreach (TokenConsumer consumer in consumersLinear)
        {
            workingContext.Consumers.WorkingSet[0] = consumer;
            if (consumer.IsConstant)
            {
                continue;// skip const patterns as they get an inline fast-path
            }

            var funcName = Format_Pattern_Consumer_Function_Name(consumer.Index);
            var funcDef = Get_Local_Token_Consumer_Function_Definition(context.Config, consumer.Type, funcName);
            funcDef.WriteTo(workingContext);
        }
        #endregion
    }
}
