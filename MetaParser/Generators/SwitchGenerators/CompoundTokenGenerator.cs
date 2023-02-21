using MetaParser.Contexts;
using MetaParser.Schemas.Structs;

using Microsoft.CodeAnalysis.CSharp;

using System;
using System.CodeDom.Compiler;

namespace MetaParser.Generators.SwitchGenerators
{
    internal class CompoundTokenGenerator : ITokenCodeGenerator
    {
        private string getTokenConsumerFuncName(TokenDefCompound token) => $"consume_all_{token?.Name?.ToLowerInvariant()}";

        public void Generate(IndentedTextWriter wr, MetaParserTokenContext context)
        {
            // generate token type detection switch map
            wr.WriteLine("switch (source.Span[0])");
            wr.WriteLine("{");
            wr.Indent++;
            foreach (var token in context.CompoundTokens)
            {
                writeSwitchCases(wr, token);
                wr.WriteLine("{");
                wr.Indent++;
                wr.WriteLine($"id = {context.Get_TokenId_Ref(token.Name)};");
                wr.WriteLine($"length = {getTokenConsumerFuncName(token)} (source.Span);");
                wr.WriteLine("return true;");
                wr.WriteLine("");
                wr.Indent--;
                wr.WriteLine("}");
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
            foreach (var token in context.CompoundTokens)
            {
                wr.WriteLine($"static {typeof(int).FullName} {getTokenConsumerFuncName(token)} ({CodeGen.FormatReadOnlySpanBuffer(context.InputType)} buffer)");
                wr.WriteLine("{");
                wr.Indent++;
                wr.WriteLine("int consumed = 0;");
                wr.WriteLine("while (buffer.Length > consumed)");
                wr.WriteLine("{");
                wr.Indent++;
                wr.WriteLine("switch (buffer[consumed])");
                wr.WriteLine("{");
                wr.Indent++;
                writeSwitchCases(wr, token);
                wr.WriteLine("{");
                wr.Indent++;
                wr.WriteLine("consumed++;");
                wr.WriteLine("continue;");
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
        }

        private void writeSwitchCases(IndentedTextWriter writer, TokenDefCompound token)
        {
            foreach (var value in token.Values)
            {
                var strCasePattern = value switch
                {
                    CompoundValueString single => SymbolDisplay.FormatLiteral(single.value, true),
                    CompoundValueRange range => $"(>= {SymbolDisplay.FormatLiteral(range.Start, true)} and <= {SymbolDisplay.FormatLiteral(range.End, true)})",
                    _ => throw new NotImplementedException($"Unrecognized Compound-TokenDefinition value item type ({value})")
                };
                writer.WriteLine($"case {strCasePattern}:");
            }
        }

    }

}
