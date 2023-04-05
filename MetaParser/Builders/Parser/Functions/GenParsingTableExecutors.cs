using MetaParser.Builders.Interfaces;
using MetaParser.Core;

namespace MetaParser.Builders.Parser.Functions;
internal class GenParsingTableExecutors : MetaCodeBuilder
{
    protected override void Write(ParserContext context)
    {
        foreach (var stage in context.Stages)
        {
            var ctx = context with { State = context.State with { Stage = stage } };
            context.Config.CodeFactory.Get_Parsing_Table_Executor()
                                      .WriteTo(ctx);
        }
    }
}
