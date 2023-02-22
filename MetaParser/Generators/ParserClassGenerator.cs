using System.CodeDom.Compiler;

using MetaParser.Contexts;

using Microsoft.CodeAnalysis;

namespace MetaParser.Generators;

internal static class ParserClassGenerator
{
    #region Constants
    const string ConsumeNextFunc = "TryConsume";
    const string CallConsumeNext = $"{ConsumeNextFunc}({VarReader}, out var outId, out var outLen)";

    //const string VarUnkStart = "unk_start";
    //const string VarFlagWasLastUnk = "was_last_unk";
    const string VarBuffer = "buffer";
    const string VarReader = "reader";
    const string VarIdBuffer = "idBuffer";
    #endregion

    public static void Generate(IndentedTextWriter wr, MetaParserContext context)
    {
        wr.WriteLine($"namespace {context.Namespace};");
        //wr.WriteLine(Common.s_generatedCodeAttributeSource);
        wr.WriteLine(context.ParserClassDeclaration);
        wr.WriteLine("{");
        wr.Indent++;
        Generate_Primary_Parsing_Function(wr, context);
        Generate_Consume_Next(wr, context);
        wr.Indent--;
        wr.WriteLine("}");// end class
    }

    public static void Generate_Primary_Parsing_Function(IndentedTextWriter wr, MetaParserContext context)
    {
        wr.WriteLine($"public System.Collections.Generic.List<Token> Parse({CodeGen.FormatReadOnlyMemoryBuffer(context.InputType)} Input)");
        wr.WriteLine("{");
        wr.Indent++;
        wr.WriteLine($"var {VarBuffer} = Input;");
        wr.WriteLine($"var {VarReader} = {VarBuffer}.Span;");
        wr.WriteLine("var valueTokens = new System.Collections.Generic.List<ValueToken>();");
        wr.WriteLine();
        wr.WriteLine("do");
        wr.WriteLine("{");
        wr.Indent++;
        wr.WriteLine($"if ({CallConsumeNext})");
        wr.WriteLine("{");
        wr.Indent++;
        // Be sure to push unknown token if its lingering
        Generate_Push_Unknown_Token(wr, context);
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
        Generate_Push_Unknown_Token(wr, context);
        wr.WriteLine();
        // Copy all of our value token ids into a uniform array in memory
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

    public static void Generate_Push_Unknown_Token(IndentedTextWriter wr, MetaParserContext context)
    {
        wr.WriteLine($"if ({VarBuffer}.Length != {VarReader}.Length)");
        wr.WriteLine("{");
        wr.Indent++;
        wr.WriteLine($"var unk_content_size = {VarBuffer}.Length - {VarReader}.Length;");
        wr.WriteLine($"var unk_content = {VarBuffer}.Slice(0, unk_content_size);");
        wr.WriteLine($"valueTokens.Add(new ValueToken({context.Get_TokenId_Ref(context.UnknownToken)}, unk_content));");
        wr.WriteLine($"{VarBuffer} = {VarBuffer}.Slice(unk_content_size);");
        wr.WriteLine($"{VarReader} = {VarBuffer}.Span;");
        wr.Indent--;
        wr.WriteLine("}");
    }
    
    public static void Generate_Consume_Next(IndentedTextWriter wr, MetaParserContext context)
    {
        wr.WriteLine($"private static bool {ConsumeNextFunc}({CodeGen.FormatReadOnlySpanBuffer(context.InputType)} source, out {context.IdTypeName} Id, out {CodeGen.Format(SpecialType.System_Int32)} Length)");
        wr.WriteLine("{");
        wr.Indent++;
        wr.WriteLine($"if ({context.ConstantTokenConsumerFunctionName}(source, out var constId, out var constLen))");
        wr.WriteLine("{");
        wr.Indent++;
        wr.WriteLine("Id = constId;");
        wr.WriteLine("Length = constLen;");
        wr.WriteLine("return true;");
        wr.Indent--;
        wr.WriteLine("}");
        wr.WriteLine($"else if ({context.CompoundTokenConsumerFunctionName}(source, out var compId, out var compLen))");
        wr.WriteLine("{");
        wr.Indent++;
        wr.WriteLine("Id = compId;");
        wr.WriteLine("Length = compLen;");
        wr.WriteLine("return true;");
        wr.Indent--;
        wr.WriteLine("}");
        wr.WriteLine();
        wr.WriteLine("Id = default;");
        wr.WriteLine("Length = default;");
        wr.WriteLine("return false;");

        wr.Indent--;
        wr.WriteLine("}");// end function
    }

}
