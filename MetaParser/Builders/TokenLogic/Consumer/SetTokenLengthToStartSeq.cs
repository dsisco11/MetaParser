using MetaParser.CodeGen.Core;
using MetaParser.Contexts;
using System.Linq;

namespace MetaParser.Builders.TokenLogic.Consumer;

internal class SetTokenLengthToStartSeq : IMetaCodeBuilder
{
    public SetTokenLengthToStartSeq(IMetaCodeBuilder body)
    {
        Body = body;
    }

    public IMetaCodeBuilder Body { get; }

    public void WriteTo(MetaParserContext context)
    {
        var token = context.Tokens.WorkingSet.Single();
        context.writer.WriteLine($"length = {token.Start.Length};");
        Body.WriteTo(context);
    }
}
