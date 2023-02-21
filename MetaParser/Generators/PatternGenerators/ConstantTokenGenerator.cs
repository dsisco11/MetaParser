using MetaParser.Contexts;

using Microsoft.CodeAnalysis.CSharp;

using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;

namespace MetaParser.Generators.PatternGenerators
{
    internal class ConstantTokenGenerator : ITokenCodeGenerator
    {
        public void Generate(IndentedTextWriter wr, MetaParserTokenContext context)
        {
            var Tokens = context.ConstantTokens;
            var tokenList = new List<(string id, string value)>(Tokens.Length);
            foreach (var token in Tokens)
            {
                foreach (var value in token.Values)
                {
                    tokenList.Add((token.Name!, value));
                }
            }

            wr.WriteLine("switch (source.Span)");
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

        //public static void Generate(IndentedTextWriter wr, MetaParserTokenContext context, ImmutableArray<TokenDefConstant> Tokens)
        //{
        //    var tokenList = new List<(string id, string value)>(Tokens.Length);
        //    foreach(var token in Tokens)
        //    {
        //        foreach (var value in token.Values)
        //        {
        //            tokenList.Add((token.Name!, value));
        //        }
        //    }

        //    wr.WriteLine($"private bool try_consume_token_constant ({CodeGen.FormatReadOnlyMemoryBuffer(context.InputType)} source, out {context.IdTypeName} id, out {typeof(int).FullName} length)");
        //    wr.WriteLine("{");
        //    wr.Indent++;
        //    wr.Write(CodeGen.FormatReadOnlySpanBuffer(context.InputType));
        //    wr.WriteLine(" buffer;");

        //    foreach (var vSizeGroup in tokenList.GroupBy(x => x.value.Length).OrderByDescending(g => g.Key))
        //    {
        //        wr.WriteLine($"if (source.Span.Length > {vSizeGroup.Key-1})");
        //        wr.WriteLine("{");
        //        wr.Indent++;
        //        wr.WriteLine($"buffer = source.Span.Slice(0, {vSizeGroup.Key});");
        //        wr.WriteLine("switch(buffer)");
        //        wr.WriteLine("{");
        //        wr.Indent++;
        //        foreach (var idGroup in vSizeGroup.GroupBy(x => x.id))
        //        {
        //            foreach (var (id, value) in idGroup)
        //            {
        //                wr.WriteLine($"case {SymbolDisplay.FormatLiteral(value, true)}:");
        //            }
        //            //wr.WriteLine("{");
        //            wr.Indent++;
        //            wr.WriteLine($"id = { context.Get_TokenId_Ref(idGroup.Key) };");
        //            wr.WriteLine($"length = {vSizeGroup.Key};");
        //            wr.WriteLine("return true;");
        //            wr.Indent--;
        //            //wr.WriteLine("}");
        //        }
        //        wr.Indent--;
        //        wr.WriteLine("}");
        //        wr.Indent--;
        //        wr.WriteLine("}");
        //        wr.WriteLine("");
        //    }

        //    // return failure
        //    wr.WriteLine("id = default;");
        //    wr.WriteLine("length = default;");
        //    wr.WriteLine("return false;");

        //    wr.Indent--;
        //    wr.WriteLine("}");// end function
        //}
    }
}
