using MetaParser.CodeGen.Core;
using MetaParser.Contexts;
using MetaParser.Structs;

using System.Collections.Immutable;

namespace MetaParser.Builders;

internal class TokenIDConstBuilder : IMetaCodeBuilder
{
    public static IMetaCodeBuilder Instance = new TokenIDConstBuilder();
    public void WriteTo(MetaParserContext context)
    {
        var writer = context.writer;
        writer.WriteLine($"public const {context.IdTypeName} {MetaParserContext.Format_TokenId(MetaParserContext.UnknownToken)} = 0;");

        var distinct = context.Consumers.CompleteSet.ToImmutableSortedSet(ConsumerComparer.Instance);
        foreach (var token in distinct)
        {
            writer.WriteLine($"public const {context.IdTypeName} {MetaParserContext.Format_TokenId(token.TokenName)} = {token.TokenIndex};");
        }
    }
}
