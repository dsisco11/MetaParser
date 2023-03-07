using MetaParser.CodeGen.Core;
using MetaParser.CodeGen.Interfaces;
using MetaParser.Consumers;
using MetaParser.Graphs;
using MetaParser.Tokens;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System;
using System.CodeDom.Compiler;
using System.Collections.Immutable;
using System.IO;

namespace MetaParser.Core
{
    internal record MetaParserContext : MetaParserConfig, ICodeBuilderContext
    {
        public IndentedTextWriter writer { get; set; } = new IndentedTextWriter(new StringWriter());
        public VertexGraph TokenGraph { get; set; }
        public ImmutableDictionary<string, TokenInfo> Tokens = ImmutableDictionary<string, TokenInfo>.Empty;
        public ConsumerList Consumers { get; set; } = new();

        #region Constants
        public const string TokenEnum = "ETokenType";
        public const string TokenConsts = "TokenId";
        public const string UnknownToken = "unknown";

        public const string TokenDataClassName = "TokenData";
        public const string TokenValueStructName = "ValueToken";
        public const string TokenRecordTypeName = "Token";

        /// <summary>Name of first buffer used in any method</summary>
        public const string VarNameBufferMajor = "input";
        /// <summary>Name of second buffer used in any method</summary>
        public const string VarNameBufferMinor = "buffer";
        /// <summary>Name of third buffer used in any method</summary>
        public const string VarNameBufferLocal = "reader";

        public const string ConstantTokenProcessorFunctionName = "TryProcessConstant";
        public const string CompoundTokenProcessorFunctionName = "TryProcessCompound";
        public const string ComplexTokenProcessorFunctionName = "TryProcessComplex";
        #endregion

        #region Statics
        public static SyntaxTokenList ParserClassModifiers = SyntaxFactory.TokenList(SyntaxFactory.ParseTokens("public sealed partial"));
        public static SyntaxTokenList SyntaxPrivateStatic = SyntaxFactory.TokenList(SyntaxFactory.ParseTokens("private static"));
        #endregion

        #region Utility Functions
        public static string Format_Token_Key(string? name) => name is null ? throw new ArgumentNullException(nameof(name)) : name.ToLowerInvariant();
        public static string Format_Token_Id(string? name) => name is null ? throw new ArgumentNullException(nameof(name)) : System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(name);
        public static string Get_TokenId_Ref(string? name) => $"{TokenConsts}.{Format_Token_Id(name)}";
        public static string Format_Pattern_Consumer_Function_Name(int consumerIndex) => $"consume_pattern_{consumerIndex}";

        public static string Get_Token_Key(TokenInfo? token) => token is null ? throw new ArgumentNullException(nameof(token)) : Format_Token_Key(token.Name);
        public static string Get_TokenId_Ref(TokenInfo? token) => token is null ? throw new ArgumentNullException(nameof(token)) : $"{TokenConsts}.{Format_Token_Id(token.Name)}";
        #endregion

        #region Builders
        public TypeSyntax Get_Consumer_Data_Type(EConsumerType type) => (type == EConsumerType.Data? InputType : IdType);
        public TypeSyntax Get_Token_Buffer_Type(EConsumerType type) => SyntaxFactory.ParseTypeName($"{CodeCommon.ReadOnlySpan}<{Get_Consumer_Data_Type(type)}>");
        public FunctionDefinition Get_Token_Processor_Function_Definition(EConsumerType type, string name, IMetaCodeBuilder body) => new(SyntaxPrivateStatic, SyntaxFactory.ParseTypeName("bool"), name, SyntaxFactory.ParseArgumentList($"{Get_Token_Buffer_Type(type)} {VarNameBufferMajor}, out {IdType} id, out int length"), body);
        public FunctionDefinition Get_Local_Token_Consumer_Function_Definition(EConsumerType type, string name, IMetaCodeBuilder body) => new(SyntaxFactory.ParseTypeName("bool"), name, SyntaxFactory.ParseArgumentList($"{Get_Token_Buffer_Type(type)} {VarNameBufferMajor}, out int length"), body);
        #endregion

    }
}
