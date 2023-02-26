using MetaParser.CodeGen;
using MetaParser.CodeGen.Base;
using MetaParser.CodeGen.Core;
using MetaParser.Json.Definitions;
using MetaParser.Structs;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using System;
using System.CodeDom.Compiler;
using System.Collections.Immutable;
using System.IO;

namespace MetaParser.Contexts
{
    internal record MetaParserContext : ICodeBuilderContext
    {
        public IndentedTextWriter writer { get; set; } = new IndentedTextWriter(new StringWriter());
        public string BaseFileName { get; set; } = string.Empty;
        public string Namespace { get; set; } = string.Empty;
        public string? ClassName { get; set; } = "Parser";
        public string? ParserType { get; set; }
        public ImmutableDictionary<string, ImmutableArray<PatternDefinition>> Patterns { get; set; } = ImmutableDictionary<string, ImmutableArray<PatternDefinition>>.Empty;
        public TokenDeclarationsList Tokens { get; set; } = new();

        public SpecialType IdType { get; set; } = SpecialType.System_Int32;
        public SpecialType InputType { get; set; } = SpecialType.System_Char;

        #region Accessors
        public string IdTypeName => CodeCommon.Format(IdType);
        public string InputTypeName => CodeCommon.Format(InputType);
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
        public static string Format_TokenId(string? name) => name is null ? throw new ArgumentNullException(nameof(name)) : System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(name.ToLowerInvariant());
        public string Get_TokenId_Ref(string? name) => name is null ? throw new ArgumentNullException(nameof(name)) : $"{TokenConsts}.{Format_TokenId(name)}";
        public static string Get_Token_Consumer_Function_Name(int consumerIndex) => $"consume_{consumerIndex}";
        #endregion

        #region Builders
        public FunctionDefinition Get_ValueToken_Consumer(string name, IMetaCodeBuilder body) => new(SyntaxFactory.TokenList(SyntaxFactory.ParseTokens("private static")), SyntaxFactory.ParseTypeName("bool"), name, SyntaxFactory.ParseArgumentList($"{CodeCommon.ReadOnlySpan}<{InputTypeName}> source, out {IdTypeName} id, out int length"), body);
        public FunctionDefinition Get_Token_Consumer(string name, IMetaCodeBuilder body) => new(SyntaxFactory.TokenList(SyntaxFactory.ParseTokens("private static")), SyntaxFactory.ParseTypeName("bool"), name, SyntaxFactory.ParseArgumentList($"{CodeCommon.ReadOnlySpan}<{IdTypeName}> source, out {IdTypeName} id, out int length"), body);
        #endregion

    }

    internal sealed record TokenDeclarationsList
    {
        /// <summary>
        /// Complete list of all tokens defined
        /// </summary>
        public ImmutableArray<PatternConsumer> CompleteSet { get; set; } = ImmutableArray<PatternConsumer>.Empty;

        /// <summary>
        /// Set of tokens being targeted by the current action
        /// </summary>
        public PatternConsumer[] WorkingSet { get; set; } = Array.Empty<PatternConsumer>();
    }
}
