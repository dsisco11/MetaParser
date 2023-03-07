using MetaParser.CodeGen.Interfaces;
using MetaParser.Contexts;
using MetaParser.Tokens;

using System.Collections.Immutable;

namespace MetaParser.Builders;

internal class TokenIDConstBuilder : IMetaCodeBuilder
{
    public static IMetaCodeBuilder Instance = new TokenIDConstBuilder();
    public void WriteTo(MetaParserContext context)
    {
        var writer = context.writer;
        writer.WriteLine($"public const {context.IdType} {MetaParserContext.Format_Token_Id(MetaParserContext.UnknownToken)} = 0;");

        foreach (var token in context.Tokens.Values.ToImmutableSortedSet(TokenInfoComparer.Instance))
        {
            writer.WriteLine($"public const {context.IdType} {MetaParserContext.Format_Token_Id(token.Name)} = {token.Index};");
        }
    }
}
