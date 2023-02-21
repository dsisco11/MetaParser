using MetaParser.Contexts;

using Microsoft.CodeAnalysis.CSharp;

using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;

namespace MetaParser.Generators.SwitchGenerators
{
    internal class ConstantTokenGenerator : ITokenCodeGenerator
    {
        public void Generate(IndentedTextWriter wr, MetaParserTokenContext context)
        {
            var tokenList = new List<(string id, string value)>(context.ConstantTokens.Length);
            foreach (var token in context.ConstantTokens)
            {
                foreach (var value in token.Values)
                {
                    tokenList.Add((token.Name!, value));
                }
            }

            wr.Write(CodeGen.FormatReadOnlySpanBuffer(context.InputType));
            wr.WriteLine(" buffer;");

            foreach (var vSizeGroup in tokenList.GroupBy(x => x.value.Length).OrderByDescending(g => g.Key))
            {
                wr.WriteLine($"if (source.Span.Length > {vSizeGroup.Key - 1})");
                wr.WriteLine("{");
                wr.Indent++;
                wr.WriteLine($"buffer = source.Span.Slice(0, {vSizeGroup.Key});");
                wr.WriteLine("switch(buffer)");
                wr.WriteLine("{");
                wr.Indent++;
                foreach (var idGroup in vSizeGroup.GroupBy(x => x.id))
                {
                    foreach (var (id, value) in idGroup)
                    {
                        wr.WriteLine($"case {SymbolDisplay.FormatLiteral(value, true)}:");
                    }
                    wr.WriteLine("{");
                    wr.Indent++;
                    wr.WriteLine($"id = {context.Get_TokenId_Ref(idGroup.Key)};");
                    wr.WriteLine($"length = {vSizeGroup.Key};");
                    wr.WriteLine("return true;");
                    wr.Indent--;
                    wr.WriteLine("}");
                }
                wr.Indent--;
                wr.WriteLine("}");
                wr.Indent--;
                wr.WriteLine("}");
                wr.WriteLine("");
            }

            // return failure
            wr.WriteLine("id = default;");
            wr.WriteLine("length = default;");
            wr.WriteLine("return false;");
        }
    }
}
