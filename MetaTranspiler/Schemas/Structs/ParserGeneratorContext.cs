using MetaTranspiler.Schemas.Structs;

using System;
using System.Collections.Immutable;

namespace MetaTranspiler
{
    public record ParserGeneratorContext
    {
        public string BaseFileName { get; set; }
        public string Namespace { get; set; }
        /// <summary>
        /// All of the tokens defined for this parser
        /// </summary>
        public ImmutableArray<TokenDef> DefinedTokens { get; set; }

        public ImmutableArray<TokenDefConst> ConstantTokens { get; set; }
        public ImmutableArray<TokenDefCompound> CompoundTokens { get; set; }
        public ImmutableArray<TokenDefComplex> ComplexTokens { get; set; }

        public Type IdValueType => Common.Get_Integer_Type(DefinedTokens.Length);
        public string IdValueTypeName => Common.Get_Integer_TypeName(DefinedTokens.Length);
    }
}
