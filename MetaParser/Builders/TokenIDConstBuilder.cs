using MetaParser.Builders.Interfaces;
using MetaParser.Core;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace MetaParser.Builders;
using static CodeCommon;

internal class TokenIDConstBuilder : MetaCodeBuilder
{
    protected override void Write(ParserContext context)
    {
        var writer = context.Writer ?? throw new InvalidOperationException("Writer is null");
        writer.WriteLine($"public const {context.Config.IdType} {Format_Token_Id(UnknownToken)} = 0;");

        int idTracker = 1;
        Dictionary<string, int> tokenIds = new Dictionary<string, int>();
        // add all registry tokens to the tokenId dictionary by their ID
        foreach (var token in context.Registry.Tokens)
        {
            string key = Format_Token_Id(token.ID);
            if (!tokenIds.ContainsKey(key))
            {
                tokenIds.Add(key, idTracker++);
            }
        }
        // first, write out all of the major token ID constants
        foreach (var token in tokenIds)
        {
            writer.WriteLine($"public const {context.Config.IdType} {token.Key} = {token.Value};");
        }

        // next, write out all of the token name constants
        var tokens = context.Registry.Tokens.ToImmutableSortedSet();

        foreach (var token in tokens)
        {
            var id = Format_Token_Id(token.ID);
            var name = Format_Token_Id(token.Name);
            var index = tokenIds[id];

            writer.WriteLine($"public const {context.Config.IdType} {name} = {index};");
        }
    }
}
