using MetaParser.Builders.Interfaces;
using MetaParser.Core;

using System.Collections.Immutable;

namespace MetaParser.Builders;
using static CodeCommon;

internal class TokenIDEnumBuilder : MetaCodeBuilder
{
    protected override void Write(ParserContext context)
    {
        var writer = context.Writer;
        writer.WriteLine($"{Format_Token_Id(UnknownToken)} = ({context.Config.IdType}) 0,");

        foreach (var token in context.Registry.Tokens.Values.ToImmutableSortedSet())
        {
            var enumName = Format_Token_Id(token.Name);
            writer.WriteLine($"{enumName} = ({context.Config.IdType}) {token.Index},");
        }
    }
}
