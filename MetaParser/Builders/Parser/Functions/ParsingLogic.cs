using MetaParser.CodeGen;
using MetaParser.CodeGen.Core;
using MetaParser.Contexts;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace MetaParser.Builders.Parser.Functions
{
    internal class ParsingLogic : IMetaCodeBuilder
    {
        public void WriteTo(MetaParserContext context)
        {
            const string VarBuffer = "buffer";
            const string VarReader = "reader";
            const string VarIdBuffer = "idBuffer";
            var wr = context.writer;

            var tyInputBuffer = SyntaxFactory.ParseTypeName($"{CodeCommon.ReadOnlyMemory}<{context.InputTypeName}>");
            var tyTokenList = SyntaxFactory.ParseTypeName($"{CodeCommon.List}<Token>");
            var tyValueTokenList = SyntaxFactory.ParseTypeName($"{CodeCommon.List}<ValueToken>");

            ConsumeNextToken funcConsumeNext = new();
            UnknownTokenPusher unkPush = new();

            //SyntaxFactory.MethodDeclaration(default, default, tyTokenList, default, SyntaxFactory.Identifier("Parse"), default, SyntaxFactory.TypeParameterList(SyntaxFactory.SeparatedList())) );
            //FunctionDefinition parseFunction = new(tyTokenList, "Parse", )

            wr.Write("public ");
            wr.WriteLine($"{tyTokenList} Parse({tyInputBuffer} Input)");
            wr.WriteLine("{");
            wr.Indent++;
            wr.WriteLine($"var {VarBuffer} = Input;");
            wr.WriteLine($"var {VarReader} = {VarBuffer}.Span;");
            wr.WriteLine($"var valueTokens = new {tyValueTokenList}();");
            wr.WriteLine();
            wr.WriteLine("do");
            wr.WriteLine("{");
            wr.Indent++;
            wr.WriteLine($"if (TryConsume({VarReader}, out var outId, out var outLen))");
            wr.WriteLine("{");
            wr.Indent++;
            // Be sure to push unknown token if its lingering
            unkPush.WriteTo(context);
            wr.WriteLine();
            wr.WriteLine($"var consumed = {VarBuffer}.Slice(0, outLen);");
            wr.WriteLine("valueTokens.Add( new ValueToken(outId, consumed) );");
            wr.WriteLine($"{VarBuffer} = {VarBuffer}.Slice(outLen);");
            wr.WriteLine($"{VarReader} = {VarBuffer}.Span;");
            wr.Indent--;
            wr.WriteLine("}");
            wr.WriteLine("else");
            wr.WriteLine("{");
            wr.Indent++;
            wr.WriteLine($"{VarReader} = {VarReader}.Slice(1);");
            wr.Indent--;
            wr.WriteLine("}");// end else
            wr.Indent--;
            wr.WriteLine("}");// end while
            wr.WriteLine($"while ({VarReader}.Length > 0);");
            wr.WriteLine();
            // Be sure to push unknown token if its lingering
            unkPush.WriteTo(context);
            wr.WriteLine();
            // Copy all of our value token ids into a uniform array in memory
            wr.WriteLine($"var idValues = new {context.IdTypeName}[valueTokens.Count];");
            wr.WriteLine("for (int i = 0; i < valueTokens.Count; i++)");
            wr.WriteLine("{");
            wr.Indent++;
            wr.WriteLine("idValues[i] = valueTokens[i].Id;");
            wr.Indent--;
            wr.WriteLine("}");
            wr.WriteLine($"var idSource = new {CodeCommon.FormatReadOnlyMemoryBuffer(context.IdType)}( idValues );");
            // complex-tokens
            wr.WriteLine();
            wr.WriteLine($"{CodeCommon.Format(SpecialType.System_Int32)} offset = 0;");
            wr.WriteLine($"var {VarIdBuffer} = idSource;");
            wr.WriteLine("System.Collections.Generic.List<Token> results = new();");
            wr.WriteLine("do");
            wr.WriteLine("{");
            wr.Indent++;
            wr.WriteLine($"if ({context.ComplexTokenConsumerFunctionName}({VarIdBuffer}.Span, out var outId, out var outLength))");
            wr.WriteLine("{");
            wr.Indent++;
            wr.WriteLine("var values = new ValueToken[outLength];");
            wr.WriteLine("valueTokens.CopyTo(offset, values, 0, outLength);");
            wr.WriteLine($"results.Add(new Token(({context.TokenEnum}) outId, values));");
            wr.WriteLine();
            wr.WriteLine("offset += outLength;");
            wr.WriteLine($"{VarIdBuffer} = {VarIdBuffer}.Slice(outLength);");
            wr.WriteLine("continue;");
            wr.Indent--;
            wr.WriteLine("}");
            wr.WriteLine();
            wr.WriteLine("// Proxy the current value-token");
            wr.WriteLine("var proxyValue = valueTokens[offset];");
            wr.WriteLine($"var token = new Token(({context.TokenEnum}) proxyValue.Id, new[] {{ proxyValue }});");
            wr.WriteLine("results.Add(token);");
            wr.WriteLine("offset += 1;");
            wr.WriteLine($"{VarIdBuffer} = {VarIdBuffer}.Slice(1);");
            wr.Indent--;
            wr.WriteLine("}");
            wr.WriteLine($"while ({VarIdBuffer}.Length > 0);");
            wr.WriteLine();
            wr.WriteLine("return results;");

            wr.Indent--;
            wr.WriteLine("}");// end function
        }
    }
}
