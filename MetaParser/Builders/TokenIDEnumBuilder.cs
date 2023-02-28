using MetaParser.CodeGen.Core;
using MetaParser.Contexts;
using MetaParser.Structs;
using System.Collections.Immutable;

namespace MetaParser.Builders;

internal class TokenIDEnumBuilder : IMetaCodeBuilder
{
    public static IMetaCodeBuilder Instance = new TokenIDEnumBuilder();

    public void WriteTo(MetaParserContext context)
    {
        var writer = context.writer;
        writer.WriteLine($"{MetaParserContext.Format_TokenId(MetaParserContext.UnknownToken)} = ({context.IdTypeName}) 0,");

        var distinct = context.Consumers.CompleteSet.ToImmutableSortedSet(ConsumerComparer.Instance);
        foreach (var token in distinct)
        {
            var enumName = MetaParserContext.Format_TokenId(token.TokenName);
            writer.WriteLine($"{enumName} = ({context.IdTypeName}) {token.TokenIndex},");
        }
    }
}
