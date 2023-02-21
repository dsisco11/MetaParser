using MetaParser.Schemas.Structs;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace MetaParser.DependencyGraph
{
    internal static class DepsGraph
    {
        public static Dictionary<string, DependencyNode<TokenDefComplex>> Build(IEnumerable<TokenDefComplex> Tokens)
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
                        foreach (var subTok in token.End)
                        {
                            // find this tokens dependency node so we can link it
                            if (dependencies.TryGetValue(subTok, out var subTokDep))
                            {
                                tokDep.Add(subTokDep);
                            }
                        }

                        if (token.Escape is not null)
                        {
                            // Add all pattern-terminator-escape tokens
                            foreach (var subTok in token.Escape)
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
