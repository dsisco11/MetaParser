using MetaTranspiler.Schemas.Structs;

using Microsoft.CodeAnalysis.CSharp;

using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace MetaTranspiler.Generators
{
    internal static class ConstantTokens
    {
        public static void Generate_Consumer_Logic(IndentedTextWriter wr, ParserGeneratorContext context, ImmutableArray<TokenDefConst> Tokens)
        {
            var tokenList = new List<(string id, string value)>(Tokens.Length);
            foreach(var token in Tokens)
            {
                foreach (var value in token.Values)
                {
                    tokenList.Add((token.Name!, value));
                }
            }

            wr.WriteLine("private static bool try_consume_constant_token (System.Memory.ReadOnlyMemory<char> source, out int id, out int length)");
            wr.WriteLine("{");
            wr.Indent++;
            wr.WriteLine("var buffer = source.Slice();");

            foreach (var vSizeGroup in tokenList.GroupBy(x => x.value.Length).OrderByDescending(g => g.Key))
            {
                wr.WriteLine($"buffer = buffer.Slice(0, {vSizeGroup.Key});");
                wr.WriteLine("switch(buffer)");
                wr.WriteLine("{");
                wr.Indent++;
                foreach (var idGroup in vSizeGroup.GroupBy(x => x.id))
                {
                    foreach (var (id, value) in idGroup)
                    {
                        wr.WriteLine($"case {SymbolDisplay.FormatLiteral(value, true)}:");
                    }
                    //wr.WriteLine("{");
                    wr.Indent++;
                    wr.WriteLine($"id = {Common.Get_TokenId_Constant(idGroup.Key)};");
                    wr.WriteLine($"length = {vSizeGroup.Key};");
                    wr.WriteLine("return true;");
                    wr.Indent--;
                    //wr.WriteLine("}");
                }
                wr.Indent--;
                wr.WriteLine("}");
                wr.WriteLine("");
            }

            // return failure
            wr.WriteLine("id = default;");
            wr.WriteLine("length = default;");
            wr.WriteLine("return false;");

            wr.Indent--;
            wr.WriteLine("}");// end function
        }
    }
}
