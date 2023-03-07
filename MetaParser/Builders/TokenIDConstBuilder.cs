using MetaParser.Builders.Interfaces;
using MetaParser.Core;
using MetaParser.Tokens;

using System.Collections.Immutable;

namespace MetaParser.Builders;
using static CodeCommon;

internal class TokenIDConstBuilder : IMetaCodeBuilder
{
    public static IMetaCodeBuilder Instance = new TokenIDConstBuilder();
    public void WriteTo(MetaParserContext context)
    {
        var writer = context.writer;
        writer.WriteLine($"public const {context.Config.IdType} {Format_Token_Id(UnknownToken)} = 0;");

        foreach (var token in context.Tokens.Values.ToImmutableSortedSet(TokenInfoComparer.Instance))
        {
            writer.WriteLine($"public const {context.Config.IdType} {Format_Token_Id(token.Name)} = {token.Index};");
        }
    }
}
