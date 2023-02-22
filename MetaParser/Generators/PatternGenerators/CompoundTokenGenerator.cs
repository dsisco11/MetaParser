using MetaParser.Contexts;
using MetaParser.Schemas.Structs;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using System;
using System.CodeDom.Compiler;

namespace MetaParser.Generators.PatternGenerators
{
    internal class CompoundTokenGenerator : ITokenCodeGenerator
    {
        public void Generate(IndentedTextWriter wr, MetaParserTokenContext context)
        {
            var Tokens = context.CompoundTokens;
            // generate token type detection switch map
            wr.WriteLine("switch (source[0])");
            wr.WriteLine("{");
            wr.Indent++;
            foreach (var token in Tokens)
            {
                WriteSwitchCases(wr, token);
                wr.WriteLine("{");
                wr.Indent++;
                wr.WriteLine($"id = {context.Get_TokenId_Ref(token.Name)};");
                wr.WriteLine($"length = {context.Get_Token_Consumer_Function_Name(token.Name)} (source);");
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
            foreach (var token in Tokens)
            {
                wr.WriteLine($"static {CodeGen.Format(SpecialType.System_Int32)} {context.Get_Token_Consumer_Function_Name(token.Name)} ({CodeGen.FormatReadOnlySpanBuffer(context.InputType)} buffer)");
                wr.WriteLine("{");
                wr.Indent++;
                wr.WriteLine("int consumed = 0;");
                wr.WriteLine("while (buffer.Length > consumed)");
                wr.WriteLine("{");
                wr.Indent++;
                wr.WriteLine("switch (buffer[consumed])");
                wr.WriteLine("{");
                wr.Indent++;
                WriteSwitchCases(wr, token);
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

        private static void WriteSwitchCases(IndentedTextWriter writer, TokenDefCompound token)
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
