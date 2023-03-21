using MetaParser.Builders.Interfaces;
using MetaParser.Core;

using System.Collections.Immutable;

namespace MetaParser.Builders;
using static CodeCommon;

internal class TokenIDConstBuilder : MetaCodeBuilder
{
    protected override void Write(ParserContext context)
    {
        var writer = context.Writer;
        writer.WriteLine($"public const {context.Config.IdType} {Format_Token_Id(UnknownToken)} = 0;");

        foreach (var token in context.Registry.Tokens.Values.ToImmutableSortedSet())
        {
            writer.WriteLine($"public const {context.Config.IdType} {Format_Token_Id(token.Name)} = {token.Index};");
        }
    }
}
