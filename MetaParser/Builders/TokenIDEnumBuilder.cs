using MetaParser.Builders.Interfaces;
using MetaParser.Core;
using MetaParser.Tokens;

using System.Collections.Immutable;

namespace MetaParser.Builders;
using static CodeCommon;

internal class TokenIDEnumBuilder : IMetaCodeBuilder
{
    public static IMetaCodeBuilder Instance = new TokenIDEnumBuilder();

    public void WriteTo(MetaParserContext context)
    {
        var writer = context.writer;
        writer.WriteLine($"{Format_Token_Id(UnknownToken)} = ({context.Config.IdType}) 0,");

        foreach (var token in context.Tokens.Values.ToImmutableSortedSet(TokenInfoComparer.Instance))
        {
            var enumName = Format_Token_Id(token.Name);
            writer.WriteLine($"{enumName} = ({context.Config.IdType}) {token.Index},");
        }
    }
}
