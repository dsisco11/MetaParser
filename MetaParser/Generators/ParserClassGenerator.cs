using System.CodeDom.Compiler;

using MetaParser.Contexts;

using Microsoft.CodeAnalysis;

namespace MetaParser.Generators;

internal static class ParserClassGenerator
{
    public static void Generate(IndentedTextWriter wr, MetaParserContext context)
    {
        wr.WriteLine($"namespace {context.Namespace};");
        //wr.WriteLine(Common.s_generatedCodeAttributeSource);
        wr.WriteLine(context.ParserClassDeclaration);
        wr.WriteLine("{");
        wr.Indent++;
        Generate_Primary_Parsing_Function(wr, context);
        wr.Indent--;
        wr.WriteLine("}");// end class
    }

    public static void Generate_Primary_Parsing_Function(IndentedTextWriter wr, MetaParserContext context)
    {
        wr.WriteLine($"public System.Collections.Generic.List<Token> Parse({CodeGen.FormatReadOnlyMemoryBuffer(context.InputType)} Input)");
        wr.WriteLine("{");
        wr.Indent++;
        wr.WriteLine("System.Collections.Generic.List<ValueToken> valueTokens = new();");
        wr.WriteLine("var Source = Input;");
        wr.WriteLine("do");
        wr.WriteLine("{");
        wr.Indent++;
        wr.WriteLine($"if ({context.ConstantTokenConsumerFunctionName}(Source, out var constId, out var constLen))");
        wr.WriteLine("{");
        wr.Indent++;
        wr.WriteLine("valueTokens.Add(new ValueToken(constId, Source.Slice(0, constLen)));");
        wr.WriteLine("Source = Source.Slice(constLen);");
        wr.Indent--;
        wr.WriteLine("}");
        wr.WriteLine($"else if ({context.CompoundTokenConsumerFunctionName}(Source, out var compId, out var compLen))");
        wr.WriteLine("{");
        wr.Indent++;
        wr.WriteLine("valueTokens.Add(new ValueToken(compId, Source.Slice(0, compLen)));");
        wr.WriteLine("Source = Source.Slice(compLen);");
        wr.Indent--;
        wr.WriteLine("}");
        wr.WriteLine("else // Consume 'unknown' token");
        wr.WriteLine("{");
        wr.Indent++;
        wr.WriteLine("valueTokens.Add(new ValueToken(0, Source.Slice(0, 1)));");
        wr.WriteLine("Source = Source.Slice(1);");
        wr.Indent--;
        wr.WriteLine("}");
        wr.Indent--;
        wr.WriteLine("}");
        wr.WriteLine("while (Source.Length > 0);");
        wr.WriteLine();
        //wr.WriteLine($"var idSource = new {CodeGen.FormatMemoryBuffer(context.IdType)}(new {context.IdTypeName}[valueTokens.Count]);");
        wr.WriteLine($"var idValues = new {context.IdTypeName}[valueTokens.Count];");
        wr.WriteLine("for (int i = 0; i < valueTokens.Count; i++)");
        wr.WriteLine("{");
        wr.Indent++;
        wr.WriteLine("idValues[i] = valueTokens[i].Id;");
        wr.Indent--;
        wr.WriteLine("}");
        wr.WriteLine($"var idSource = new {CodeGen.FormatReadOnlyMemoryBuffer(context.IdType)}( idValues );");
        // complex-tokens
        wr.WriteLine();
        wr.WriteLine($"{CodeGen.Format(SpecialType.System_Int32)} offset = 0;");
        wr.WriteLine("var buffer = idSource;");
        wr.WriteLine("System.Collections.Generic.List<Token> results = new();");
        wr.WriteLine("do");
        wr.WriteLine("{");
        wr.Indent++;
        wr.WriteLine($"if ({context.ComplexTokenConsumerFunctionName}(buffer, out var outId, out var outLength))");
        wr.WriteLine("{");
        wr.Indent++;
        wr.WriteLine("var values = new ValueToken[outLength];");
        wr.WriteLine("valueTokens.CopyTo(offset, values, 0, outLength);");
        wr.WriteLine($"results.Add(new Token(({context.TokenEnum}) outId, values));");
        wr.WriteLine();
        wr.WriteLine("offset += outLength;");
        wr.WriteLine("buffer = buffer.Slice(outLength);");
        wr.WriteLine("continue;");
        wr.Indent--;
        wr.WriteLine("}");
        wr.WriteLine();
        wr.WriteLine("// Proxy the current value-token");
        wr.WriteLine("var proxyValue = valueTokens[offset];");
        wr.WriteLine($"var token = new Token(({context.TokenEnum}) proxyValue.Id, new[] {{ proxyValue }});");
        wr.WriteLine("results.Add(token);");
        wr.WriteLine("offset += 1;");
        wr.WriteLine("buffer = buffer.Slice(1);");
        wr.Indent--;
        wr.WriteLine("}");
        wr.WriteLine("while (buffer.Length > 0);");
        wr.WriteLine();
        wr.WriteLine("return results;");

        wr.Indent--;
        wr.WriteLine("}");// end function
    }
}
