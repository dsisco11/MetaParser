using MetaParser.CodeGen;
using MetaParser.CodeGen.Base;
using MetaParser.CodeGen.Core;
using MetaParser.Schemas.Structs;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using System;
using System.CodeDom.Compiler;
using System.Collections.Immutable;

namespace MetaParser.Contexts
{
    internal record MetaParserContext : ICodeBuilderContext
    {
        public IndentedTextWriter writer { get; set; }
        public string BaseFileName { get; set; } = string.Empty;
        public string Namespace { get; set; } = string.Empty;
        public string? ClassName { get; set; } = "Parser";
        public string? ParserType { get; set; }

        public SpecialType IdType { get; set; } = SpecialType.System_Int32;
        public SpecialType InputType { get; set; } = SpecialType.System_Char;

        #region Accessors
        public string IdTypeName => CodeCommon.Format(IdType);
        public string InputTypeName => CodeCommon.Format(InputType);
        #endregion

        #region Tokens
        /// <summary>
        /// All of the tokens defined for this parser
        /// </summary>
        public ImmutableArray<TokenDef> DefinedTokens { get; set; }
        public ImmutableArray<TokenDefConstant> ConstantTokens { get; set; }
        public ImmutableArray<TokenDefCompound> CompoundTokens { get; set; }
        public ImmutableArray<TokenDefComplex> ComplexTokens { get; set; }
        #endregion

        #region Constants
        public readonly string ClassAccessKeywords = "public sealed partial";

        public readonly string TokenEnum = "ETokenType";
        public readonly string TokenConsts = "TokenId";
        public readonly string UnknownToken = "Unknown";

        public readonly string ConstantTokenConsumerFunctionName = "consume_constant_token";
        public readonly string CompoundTokenConsumerFunctionName = "consume_compound_token";
        public readonly string ComplexTokenConsumerFunctionName = "consume_complex_token";
        #endregion

        #region Utility Functions
        public string Format_TokenId(string? name) => name is null ? throw new ArgumentNullException(nameof(name)) : System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(name.ToLowerInvariant());
        public string Get_TokenId_Ref(string? name) => name is null ? throw new ArgumentNullException(nameof(name)) : $"{TokenConsts}.{Format_TokenId(name)}";
        public string Get_Token_Consumer_Function_Name(string? name) => name is null ? throw new ArgumentNullException(nameof(name)) : $"consume_{name.ToLowerInvariant()}";
        #endregion

        #region Builders
        public FunctionDefinition Get_ValueToken_Consumer(string name, IMetaCodeBuilder body) => new(SyntaxFactory.TokenList(SyntaxFactory.ParseTokens("private static")), SyntaxFactory.ParseTypeName("bool"), name, SyntaxFactory.ParseArgumentList($"{CodeCommon.ReadOnlySpan}<{InputTypeName}> source, out {IdTypeName} id, out int length"), body);
        public FunctionDefinition Get_Token_Consumer(string name, IMetaCodeBuilder body) => new(SyntaxFactory.TokenList(SyntaxFactory.ParseTokens("private static")), SyntaxFactory.ParseTypeName("bool"), name, SyntaxFactory.ParseArgumentList($"{CodeCommon.ReadOnlySpan}<{IdTypeName}> source, out {IdTypeName} id, out int length"), body);
        #endregion

    }
}
