using MetaParser.Builders.Interfaces;
using MetaParser.Core;
using MetaParser.Tokens;

using System.Collections.Immutable;

namespace MetaParser.Builders;

internal class TokenIDEnumBuilder : IMetaCodeBuilder
{
    public static IMetaCodeBuilder Instance = new TokenIDEnumBuilder();

    public void WriteTo(MetaParserContext context)
    {
        var writer = context.writer;
        writer.WriteLine($"{CodeCommon.Format_Token_Id(CodeCommon.UnknownToken)} = ({context.Config.IdType}) 0,");

        foreach (var token in context.Tokens.Values.ToImmutableSortedSet(TokenInfoComparer.Instance))
        {
            var enumName = CodeCommon.Format_Token_Id(token.Name);
            writer.WriteLine($"{enumName} = ({context.Config.IdType}) {token.Index},");
        }
    }
}
