using MetaParser.CodeGen;
using MetaParser.CodeGen.Base;
using MetaParser.CodeGen.Core;
using MetaParser.Json.Definitions;
using MetaParser.Structs;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
        public const string TokenEnum = "ETokenType";
        public const string TokenConsts = "TokenId";
        public const string UnknownToken = "Unknown";

        /// <summary>Name of first buffer used in any method</summary>
        public const string VarNameBufferMajor = "stream";
        /// <summary>Name of second buffer used in any method</summary>
        public const string VarNameBufferMinor = "buffer";
        /// <summary>Name of third buffer used in any method</summary>
        public const string VarNameBufferLocal = "reader";

        public const string ConstantTokenProcessorFunctionName = "process_constant_tokens";
        public const string CompoundTokenProcessorFunctionName = "process_compound_tokens";
        public const string ComplexTokenProcessorFunctionName = "process_complex_tokens";
        #endregion

        #region Statics
        public static SyntaxTokenList ParserClassModifiers = SyntaxFactory.TokenList(SyntaxFactory.ParseTokens("public sealed partial"));
        public static SyntaxTokenList SyntaxPrivateStatic = SyntaxFactory.TokenList(SyntaxFactory.ParseTokens("private static"));
        #endregion

        #region Utility Functions
        public static string Format_TokenId(string? name) => name is null ? throw new ArgumentNullException(nameof(name)) : System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(name.ToLowerInvariant());
        public string Get_TokenId_Ref(string? name) => name is null ? throw new ArgumentNullException(nameof(name)) : $"{TokenConsts}.{Format_TokenId(name)}";
        public static string Format_Pattern_Consumer_Function_Name(int consumerIndex) => $"consume_pattern_{consumerIndex}";
        #endregion

        #region Builders
        public SyntaxToken Get_Consumer_Data_Type(ETokenType type) => SyntaxFactory.ParseToken(type == ETokenType.Constant ? InputTypeName : IdTypeName);
        public TypeSyntax Get_Token_Buffer_Type(ETokenType type) => SyntaxFactory.ParseTypeName($"{CodeCommon.ReadOnlySpan}<{Get_Consumer_Data_Type(type)}>");
        public FunctionDefinition Get_Token_Processor_Function_Definition(ETokenType type, string name, IMetaCodeBuilder body) => new(SyntaxPrivateStatic, SyntaxFactory.ParseTypeName("bool"), name, SyntaxFactory.ParseArgumentList($"{Get_Token_Buffer_Type(type)} {VarNameBufferMajor}, out {IdTypeName} id, out int length"), body);
        public FunctionDefinition Get_Local_Token_Consumer_Function_Definition(ETokenType type, string name, IMetaCodeBuilder body) => new(null, SyntaxFactory.ParseTypeName("bool"), name, SyntaxFactory.ParseArgumentList($"{Get_Token_Buffer_Type(type)} {VarNameBufferMajor}, out int length"), body);
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
