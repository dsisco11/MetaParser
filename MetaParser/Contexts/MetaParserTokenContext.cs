using MetaParser.Generators;
using MetaParser.Schemas.Structs;

using System.Collections.Immutable;

namespace MetaParser.Contexts
{
    internal sealed record MetaParserTokenContext : MetaParserContext
    {
        /// <summary>
        /// All of the tokens defined for this parser
        /// </summary>
        public ImmutableArray<TokenDef> DefinedTokens { get; set; }
        public ImmutableArray<TokenDefConstant> ConstantTokens { get; set; }
        public ImmutableArray<TokenDefCompound> CompoundTokens { get; set; }
        public ImmutableArray<TokenDefComplex> ComplexTokens { get; set; }

        public ITokenCodeGenerator ConstantTokenGen { get; set; } = new Generators.PatternGenerators.ConstantTokenGenerator();
        public ITokenCodeGenerator CompoundTokenGen { get; set; } = new Generators.PatternGenerators.CompoundTokenGenerator();
        public ITokenCodeGenerator ComplexTokenGen { get; set; } = new Generators.PatternGenerators.ComplexTokenGenerator();

        public MetaParserTokenContext(MetaParserContext other) : base(other)
        {
        }
    }
}
