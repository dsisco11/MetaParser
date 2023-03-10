using MetaParser.CodeGen.Core;
using MetaParser.Contexts;

using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using System.Linq;

namespace MetaParser.Builders.TokenConsumers.Constant;

internal class PatternBasedConsumer : IMetaCodeBuilder
{
    public void WriteTo(MetaParserContext context)
    {
        var wr = context.writer;
        var Tokens = context.ConstantTokens;

        var tokenList = new List<(string id, string value)>(Tokens.Length);
        foreach (var token in Tokens)
        {
            foreach (var value in token.Values)
            {
                tokenList.Add((token.Name!, value));
            }
        }

        wr.WriteLine("switch (source)");
        wr.WriteLine("{");
        wr.Indent++;
        foreach (var token in tokenList)
        {
            // We have to express string patterns as char arrays, so we need to expand this string value
            var chars = token.value.ToCharArray().Select(c => SymbolDisplay.FormatLiteral(c, true));
            wr.WriteLine($"case [{string.Join(", ", chars)}, ..]:");
            wr.WriteLine("{");
            wr.Indent++;
            wr.WriteLine($"id = {context.Get_TokenId_Ref(token.id)};");
            wr.WriteLine($"length = {token.value.Length};");
            wr.WriteLine("return true;");
            wr.Indent--;
            wr.WriteLine("}");
        }
        wr.Indent--;
        wr.WriteLine("}");// end switch

        // return failure
        wr.WriteLine("id = default;");
        wr.WriteLine("length = default;");
        wr.WriteLine("return false;");
    }
}
