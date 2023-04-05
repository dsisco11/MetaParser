using MetaParser.Builders.Interfaces;
using MetaParser.Core;

using System;
using System.Linq;

namespace MetaParser.Builders;
using static CodeCommon;

internal class TokenIDEnumBuilder : MetaCodeBuilder
{
    protected override void Write(ParserContext context)
    {
        var writer = context.Writer ?? throw new InvalidOperationException("Writer is null");
        writer.WriteLine($"{Format_Token_Id(UnknownToken)} = ({context.Config.IdType}) 0,");

        foreach (var token in context.Registry.Tokens.Select(static (t) => Format_Token_Id(t.ID)).Distinct())
        {
            var enumName = Format_Token_Id(token);
            writer.WriteLine($"{enumName},");
        }
    }
}
