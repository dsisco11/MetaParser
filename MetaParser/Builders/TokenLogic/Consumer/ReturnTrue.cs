using MetaParser.CodeGen.Interfaces;
using MetaParser.Core;

namespace MetaParser.Builders.TokenLogic.Consumer;

internal class ReturnTrue : IMetaCodeBuilder
{
    public void WriteTo(MetaParserContext context)
    {
        context.writer.WriteLine("return true;");
    }
}
