using MetaParser.Builders.Interfaces;
using MetaParser.Core;

using System.Linq;

namespace MetaParser.Builders.TokenLogic.Consumer;
using static CodeCommon;

internal class TokenProcessor : MetaCodeBuilder
{
    protected override void Write(MetaParserContext context)
    {
        var writer = context.Writer;

        #region Token Processing Logic

        var groups = context.WorkingSet.Consumers.GroupBy(static c => c.DependencyInfo!.IsRecursive && c.Consume is not null);

        var consumersRecursive = groups.Where(static b => b.Key == true).SelectMany(static b => b);
        if (consumersRecursive.Any())
        {
            context.Config.CodeFactory.Get_Logic_Detect_Recursive_Token()
                                      .And(new ExecuteConsumerAndReturnResult())
                                      .WriteTo(context with { WorkingSet = new (consumersRecursive.ToArray()) });
        }

        var consumersLinear = groups.Where(static b => b.Key == false).SelectMany(static b => b);
        if (consumersLinear.Any())
        {
            context.Config.CodeFactory.Get_Logic_Detect_Linear_Token()
                                      .And(new ExecuteConsumerAndReturnResult())
                                      .WriteTo(context with { WorkingSet = new(consumersLinear.ToArray()) });
        }

        // return failure
        writer.WriteLine("id = default;");
        writer.WriteLine("length = default;");
        writer.WriteLine("return false;");
        writer.WriteLine();
        #endregion

        #region Local Sub-Functions

        // generate token consumer functions
        var workingContext = context with { };
        foreach (var consumer in context.WorkingSet.Consumers)
        {
            if (consumer.IsConstant)
            {
                continue;// skip const patterns as they get an inline fast-path
            }

            workingContext.WorkingSet = new(consumer);
            var funcName = Format_Pattern_Consumer_Function_Name(consumer.Key.Index);
            var funcDef = Get_Local_Token_Consumer_Function_Definition(context, consumer.Type, funcName);
            funcDef.WriteTo(workingContext);
        }
        #endregion
    }
}
