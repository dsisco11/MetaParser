using MetaParser.Builders.Interfaces;
using MetaParser.Core;

using System.Linq;

namespace MetaParser.Builders.TokenLogic.Consumer;
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
                                      .And(new ExecuteConsumerAndReturn())
                                      .WriteTo(context with { WorkingSet = new WorkingSet(consumersRecursive.ToArray()) });
        }

        var consumersLinear = groups.Where(static b => b.Key == false).SelectMany(static b => b);
        if (consumersLinear.Any())
        {
            context.Config.CodeFactory.Get_Switch_Block_For_Consumers()
                                      .And(new ExecuteConsumer())
                                      .WriteTo(context with { WorkingSet = new WorkingSet(consumersLinear.ToArray()) });
        }
        else
        {
            writer.WriteLine("return new (default, default);");
        }

        writer.WriteLine();
        #endregion
    }
}
