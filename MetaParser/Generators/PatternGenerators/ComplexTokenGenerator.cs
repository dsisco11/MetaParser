using Microsoft.CodeAnalysis;
using System.Linq;
using MetaParser.Schemas.Structs;
using System.Collections.Immutable;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using MetaParser.DependencyGraph;
using MetaParser.Contexts;

namespace MetaParser.Generators.PatternGenerators
{
    internal class ComplexTokenGenerator : ITokenCodeGenerator
    {
        public void Generate(IndentedTextWriter wr, MetaParserTokenContext context)
        {
            var Tokens = context.ComplexTokens;
            var depGraph = DependencyGraph.DepsGraph.Build(Tokens);
            var tokenList = depGraph.Values.ToImmutableSortedSet();
            // now write the token consuming functions

            wr.WriteLine("switch (source)");
            wr.WriteLine("{");
            wr.Indent++;

            foreach (var token in tokenList)
            {
                var tokenIdName = context.Get_TokenId_Ref(token.Name);
                var patternSeq = token.Value.Start.Select(context.Get_TokenId_Ref).ToImmutableArray();
                wr.WriteLine($"case [{string.Join(", ", patternSeq)}, ..]:");
                wr.WriteLine("{");
                wr.Indent++;
                wr.WriteLine($"id = {tokenIdName};");
                wr.WriteLine($"return {context.Get_Token_Consumer_Function_Name(token.Name)}(source, out length);");
                wr.Indent--;
                wr.WriteLine("}");
            }

            wr.Indent--;
            wr.WriteLine("}");// end switch block
            wr.WriteLine();
            wr.WriteLine("id = default;");
            wr.WriteLine("length = default;");
            wr.WriteLine("return false;");
            wr.WriteLine();

            // complx tokens all have start pattern seq. so do switch block here to detect pattern and switch to consumer func
            foreach (var token in tokenList)
            {
                Generate_Token_Consumption_Function(wr, context, token);
            }
        }

        private static void Generate_Token_Consumption_Function(IndentedTextWriter wr, MetaParserTokenContext context, DependencyNode<TokenDefComplex> token)
        {
            //const string nameConsumeSeq = "seqConsume";
            const string nameEndTerminatorSeq = "seqEndTerminator";
            const string nameEndEscapeSeq = "seqEndEscape";

            bool hasTerminator = token.Value.End?.Length > 0;
            bool hasEscape = token.Value.Escape.Length > 0;

            List<string>? startSeq = token.Value.Start.Select(context.Get_TokenId_Ref).ToList();
            List<string>? consumeSeq = null;
            List<string>? endTerminatorSeq = null;
            List<string>? endEscapeSeq = null;

            if (token.Value.Consume.Length > 0)
            {
                consumeSeq = token.Value.Consume.Select(context.Get_TokenId_Ref).ToList();
            }

            if (hasTerminator)
            {
                endTerminatorSeq = token.Value.End?.Select(context.Get_TokenId_Ref).ToList();

                if (hasEscape)
                {
                    var escSeq = token.Value.Escape?.Select(context.Get_TokenId_Ref).ToList();
                    endEscapeSeq = escSeq!.Concat(endTerminatorSeq!).ToList();
                }
            }

            wr.WriteLine();
            wr.WriteLine($"static bool {context.Get_Token_Consumer_Function_Name(token.Name)} ({CodeGen.FormatReadOnlySpanBuffer(context.IdType)} start, out {CodeGen.Format(SpecialType.System_Int32)} consumed)");
            wr.WriteLine("{");
            wr.Indent++;

            wr.WriteLine("#if DEBUG");
            wr.Indent++;
            wr.WriteLine($"System.Diagnostics.Debug.Assert(start.StartsWith(stackalloc[] {{ {string.Join(", ", startSeq)} }}));");
            wr.Indent--;
            wr.WriteLine("#endif");
            wr.WriteLine($"var buffer = start.Slice({token.Value.Start!.Length});");// Skip ahead of the token start

            if (endTerminatorSeq is not null)
            {
                wr.WriteLine($"{CodeGen.FormatSpanBuffer(context.IdType)} {nameEndTerminatorSeq} = stackalloc[] {{ {string.Join(", ", endTerminatorSeq)} }};");
            }

            if (endEscapeSeq is not null)
            {
                wr.WriteLine($"{CodeGen.FormatSpanBuffer(context.IdType)} {nameEndEscapeSeq} = stackalloc[] {{ {string.Join(", ", endEscapeSeq)} }};");
            }

            wr.WriteLine();
            wr.WriteLine("while (buffer.Length > 0)");
            wr.WriteLine("{");
            wr.Indent++;

            // If we have an escape set, then check that
            if (endEscapeSeq is not null)
            {
                wr.WriteLine($"if (buffer.StartsWith({nameEndEscapeSeq}))");
                wr.WriteLine("{");
                wr.Indent++;
                wr.WriteLine($"buffer = buffer.Slice({endEscapeSeq.Count});");
                wr.WriteLine($"continue;");
                wr.Indent--;
                wr.WriteLine("}");
                wr.WriteLine();
            }

            // If we have a token terminator (end sequence) set, then check it
            if (endTerminatorSeq is not null)
            {
                wr.WriteLine($"if (buffer.StartsWith({nameEndTerminatorSeq}))");
                wr.WriteLine("{");
                wr.Indent++;
                wr.WriteLine("break;// end consumption");
                wr.Indent--;
                wr.WriteLine("}");
                wr.WriteLine();
            }

            // If we have a set of valid consume targets
            if (consumeSeq is not null)
            {
                wr.WriteLine($"switch (buffer[0])");
                wr.WriteLine("{");
                wr.Indent++;
                foreach (var item in consumeSeq)
                {
                    wr.WriteLine($"case {item}:");
                }
                wr.Indent++;
                wr.WriteLine("buffer = buffer.Slice(1);");
                wr.WriteLine("continue;");
                wr.Indent--;
                wr.Indent--;
                wr.WriteLine("}");
                wr.WriteLine();
                wr.WriteLine("break;");// break out of loop by default
            }
            else// If the token doesnt specify a specific set of valid items to consume, then all items are valid
            {
                if (hasTerminator)// To avoid infinite loops we just do this sanity check here to make sure we dont produce conditionless itteration
                {
                    wr.WriteLine("buffer = buffer.Slice(1);");
                }
            }

            wr.Indent--;
            wr.WriteLine("}");// end while loop
            wr.WriteLine();

            // If we have a token terminator (end sequence) set...
            // The check that it is present, if not then token consumption fails as the end terminator sequence is required when specified
            if (endTerminatorSeq is not null)
            {
                wr.WriteLine($"if (buffer.StartsWith({nameEndTerminatorSeq}))");
                wr.WriteLine("{");
                wr.Indent++;
                wr.WriteLine($"consumed = {endTerminatorSeq.Count} + (start.Length - buffer.Length);");
                wr.WriteLine("return true;");
                wr.Indent--;
                wr.WriteLine("}");
                wr.WriteLine();
                wr.WriteLine("consumed = default;");
                wr.WriteLine("return false;");
            }
            else
            {
                wr.WriteLine("consumed = start.Length - buffer.Length;");
                wr.WriteLine("return true;");
            }

            wr.Indent--;
            wr.WriteLine("}");// end function
        }
    }
}
