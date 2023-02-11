using MetaTranspiler.Schemas.Structs;

using Microsoft.CodeAnalysis.CSharp;

using System;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace MetaTranspiler.Generators
{
    internal static class CompoundTokens
    {
        private static string getTokenConsumerFuncName(TokenDefCompound token) => $"_consume_all_{ token.Name.ToLowerInvariant() }";

        public static void Generate_Consumer_Logic(IndentedTextWriter wr, ParserGeneratorContext context, ImmutableArray<TokenDefCompound> Tokens)
        {
            wr.WriteLine("private static bool try_consume_compound_token (System.Memory.ReadOnlyMemory<char> source, out int id, out int length)");
            wr.WriteLine("{");
            wr.Indent++;
            // generate token type detection switch map
            wr.WriteLine("switch (source.span[0])");
            wr.WriteLine("{");
            wr.Indent++;
            foreach (var token in Tokens)
            {
                writeSwitchCases(wr, token);
                //wr.WriteLine("{");
                wr.Indent++;
                wr.WriteLine($"id = { Common.Get_TokenId_Constant(token.Name) };");
                wr.WriteLine($"length = { getTokenConsumerFuncName(token) } (source.span);");
                wr.WriteLine("return true;");
                wr.WriteLine("");
                wr.Indent--;
                //wr.WriteLine("}");
            }
            wr.Indent--;
            wr.WriteLine("}");

            // return failure
            wr.WriteLine("");
            wr.WriteLine("id = default;");
            wr.WriteLine("length = default;");
            wr.WriteLine("return false;");
            wr.WriteLine("");

            // Generate all token consumer functions
            foreach (var token in Tokens)
            {
                wr.WriteLine($"static int {getTokenConsumerFuncName(token)} (System.Memory.ReadOnlySpan<char> buf)");
                wr.WriteLine("{");
                wr.Indent++;
                wr.WriteLine("int consumed = 0;");
                wr.WriteLine("while (buf.Length > consumed)");
                wr.WriteLine("{");
                wr.Indent++;
                wr.WriteLine("switch (buf[consumed])");
                wr.WriteLine("{");
                wr.Indent++;
                writeSwitchCases(wr, token);
                wr.WriteLine("{");
                wr.Indent++;
                wr.WriteLine("consumed++;");
                wr.WriteLine("break;");
                wr.Indent--;
                wr.WriteLine("}");// end case block
                wr.WriteLine("default: return consumed;");
                wr.Indent--;
                wr.WriteLine("}");// end switch block
                wr.Indent--;
                wr.WriteLine("}");// end while block
                wr.WriteLine("return consumed;");
                wr.Indent--;
                wr.WriteLine("}");// end function
                wr.WriteLine();
            }
            wr.Indent--;
            wr.WriteLine("}");// end function
        }

        private static void writeSwitchCases(IndentedTextWriter writer, TokenDefCompound token)
        {
            foreach (var value in token.Values)
            {
                var strCasePattern = value switch
                {
                    CompoundValueString single => SymbolDisplay.FormatLiteral(single.value, true),
                    CompoundValueRange range => $">= {SymbolDisplay.FormatLiteral(range.Start, true)} and <= {SymbolDisplay.FormatLiteral(range.End, true)}",
                    _ => throw new NotImplementedException($"Unrecognized Compound-TokenDefinition value item type ({value})")
                };
                writer.WriteLine($"case {strCasePattern}:");
            }
        }

    }

}
