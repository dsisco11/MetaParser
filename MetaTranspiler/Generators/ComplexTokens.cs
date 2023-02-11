using Microsoft.CodeAnalysis;
using System.Linq;
using MetaTranspiler.Schemas.Structs;
using System.Collections.Immutable;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using MetaTranspiler.DependencyGraph;

namespace MetaTranspiler.Generators
{
    internal static class ComplexTokens
    {
        private static string Get_Token_Consumer_Function_Name(string tokenName) => $"_consume_{tokenName.ToLowerInvariant()}_token";
        public static void Generate_Consumer_Logic(IndentedTextWriter wr, ParserGeneratorContext context, ImmutableArray<TokenDefComplex> Tokens)
        {
            var depGraph = Build_Dependency_Graph(Tokens);
            var tokenList = depGraph.Values.ToImmutableSortedSet();
            // now write the token consuming functions

            wr.WriteLine($"private static bool try_consume_complex_token (System.Memory.ReadOnlyMemory<{context.IdValueTypeName}> source, out {context.IdValueTypeName} id, out int length)");
            wr.WriteLine("{");
            wr.Indent++;
            // Token Detection

            wr.WriteLine("switch (source)");
            wr.WriteLine("{");
            wr.Indent++;

            foreach (var token in tokenList)
            {
                var tokenIdName = Common.Get_TokenId_Constant(token.Name);
                var patternSeq = token.Value.Start.Select(Common.Get_TokenId_Constant).ToImmutableArray();
                wr.WriteLine($"case [{string.Join(", ", patternSeq)}]:");
                wr.WriteLine("{");
                wr.Indent++;
                wr.WriteLine($"id = {tokenIdName};");
                wr.WriteLine($"return {Get_Token_Consumer_Function_Name(token.Name)}(source, out length);");
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
            wr.Indent--;
            wr.WriteLine("}");// end function
        }

        private static void Generate_Token_Consumption_Function(IndentedTextWriter wr, ParserGeneratorContext context, DependencyNode<TokenDefComplex> token)
        {
            const string nameConsumeSeq = "seqConsume";
            const string nameEndTerminatorSeq = "seqEndTerminator";
            const string nameEndEscapeSeq = "seqEndEscape";

            bool hasTerminator = (token.Value.End?.Terminator.Length > 0);
            bool hasEscape = (token.Value.End?.Escape?.Length > 0);

            ImmutableArray<string>? startSeq = token.Value.Start.Select(Common.Get_TokenId_Constant).ToImmutableArray();
            ImmutableArray<string>? consumeSeq = null;
            ImmutableArray<string>? endTerminatorSeq = null;
            ImmutableArray<string>? endEscapeSeq = null;

            if (token.Value.Consume.Length > 0)
            {
                consumeSeq = token.Value.Consume.Select(Common.Get_TokenId_Constant).ToImmutableArray();
            }

            if (hasTerminator)
            {
                endTerminatorSeq = token.Value.End?.Terminator.Select(Common.Get_TokenId_Constant).ToImmutableArray();

                if (hasEscape)
                {
                    var escSeq = token.Value.End?.Escape?.Select(Common.Get_TokenId_Constant).ToImmutableArray();
                    endEscapeSeq = escSeq!.Value.AddRange(endTerminatorSeq!);
                }
            }

            wr.WriteLine($"static bool {Get_Token_Consumer_Function_Name(token.Name)} (System.Memory.ReadOnlySpan<{context.IdValueTypeName}> start, out int consumed)");
            wr.WriteLine("{");
            wr.Indent++;

            wr.WriteLine("#if DEBUG");
            wr.Indent++;
            wr.WriteLine($"Debug.Assert(buffer.StartsWith(stackalloc[] {{ {string.Join(", ", startSeq)} }}));");
            wr.Indent--;
            wr.WriteLine("#endif");
            wr.WriteLine($"buffer = source.Slice({token.Value.Start!.Length})");// Skip ahead of the token start

            if (endTerminatorSeq is not null)
            {
                wr.WriteLine($"var {nameEndTerminatorSeq} = stackalloc[] {{ {string.Join(", ", endTerminatorSeq)} }};");
            }

            if (endEscapeSeq is not null)
            {
                wr.WriteLine($"var {nameEndEscapeSeq} = stackalloc[] {{ {string.Join(", ", endEscapeSeq)} }};");
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
                wr.WriteLine($"buffer = buffer.Slice({endEscapeSeq.Value.Length});");
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
                wr.WriteLine($"switch (buffer)");
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
                wr.WriteLine($"if (!buffer.StartsWith({nameEndTerminatorSeq}))");
                wr.WriteLine("{");
                wr.Indent++;
                wr.WriteLine("consumed = default;");
                wr.WriteLine("return false;");
                wr.Indent--;
                wr.WriteLine("}");
                wr.WriteLine();
            }

            wr.WriteLine("consumed = source.Length - buffer.Length;");
            wr.WriteLine("return true;// end consumption");

            wr.Indent--;
            wr.WriteLine("}");// end function
        }

        private static Dictionary<string, DependencyNode<TokenDefComplex>> Build_Dependency_Graph(ImmutableArray<TokenDefComplex> Tokens)
        {
            var dependencies = Tokens.ToDictionary(tok => tok.Name!, tok => new DependencyNode<TokenDefComplex>(tok.Name!, tok));

            // Foreach complex token, add all of the other tokens which it references to its dependency node
            foreach (var token in Tokens)
            {
                if (dependencies.TryGetValue(token.Name!, out var tokDep))
                {
                    // Add all pattern-start tokens
                    foreach (var subTok in token.Start)
                    {
                        // find this tokens dependency node so we can link it
                        if (dependencies.TryGetValue(subTok, out var subTokDep))
                        {
                            tokDep.Add(subTokDep);
                        }
                    }

                    // Add all consumed tokens
                    foreach (var subTok in token.Consume)
                    {
                        // find this tokens dependency node so we can link it
                        if (dependencies.TryGetValue(subTok, out var subTokDep))
                        {
                            tokDep.Add(subTokDep);
                        }
                    }

                    if (token.End is not null)
                    {
                        // Add all pattern-terminator tokens
                        foreach (var subTok in token.End.Terminator)
                        {
                            // find this tokens dependency node so we can link it
                            if (dependencies.TryGetValue(subTok, out var subTokDep))
                            {
                                tokDep.Add(subTokDep);
                            }
                        }

                        if (token.End.Escape is not null)
                        {
                            // Add all pattern-terminator-escape tokens
                            foreach (var subTok in token.End.Escape)
                            {
                                // find this tokens dependency node so we can link it
                                if (dependencies.TryGetValue(subTok, out var subTokDep))
                                {
                                    tokDep.Add(subTokDep);
                                }
                            }
                        }

                    }
                }
            }
            return dependencies;
        }

    }
}
