using MetaParser.CodeGen.Core;
using MetaParser.Contexts;

using System.Linq;

namespace MetaParser.Builders.TokenLogic.Consumer;

internal class ConsumeTokenAndReturn : IMetaCodeBuilder
{
    public void WriteTo(MetaParserContext context)
    {
        var consumerId = context.Tokens.WorkingSet.Single().ConsumerIndex;
        var consumerFunc = context.Get_Token_Consumer_Function_Name(consumerId);
        context.writer.WriteLine($"return {consumerFunc}();");
    }
}
