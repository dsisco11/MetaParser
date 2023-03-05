using MetaParser.CodeGen;
using MetaParser.CodeGen.Base;
using MetaParser.CodeGen.Core;
using MetaParser.Consumers;

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
        public PatternConsumerList Consumers { get; set; } = new();

        public TypeSyntax IdType { get; set; } = SyntaxFactory.ParseTypeName("int");
        public TypeSyntax InputType { get; set; } = SyntaxFactory.ParseTypeName("char");

        #region Accessors
        #endregion

        #region Constants
        public const string TokenEnum = "ETokenType";
        public const string TokenConsts = "TokenId";
        public const string UnknownToken = "unknown";

        public const string TokenDataClassName = "TokenData";
        public const string TokenValueClassName = "ValueToken";
        public const string TokenClassName = "Token";

        /// <summary>Name of first buffer used in any method</summary>
        public const string VarNameBufferMajor = "stream";
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
        public static string Format_TokenId(string? name) => name is null ? throw new ArgumentNullException(nameof(name)) : System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(name.ToLowerInvariant());
        public static string Get_TokenId_Ref(string? name) => name is null ? throw new ArgumentNullException(nameof(name)) : $"{TokenConsts}.{Format_TokenId(name)}";
        public static string Format_Pattern_Consumer_Function_Name(int consumerIndex) => $"consume_pattern_{consumerIndex}";
        #endregion

        #region Builders
        public TypeSyntax Get_Consumer_Data_Type(ETokenType type) => (type == ETokenType.Constant ? InputType : IdType);
        public TypeSyntax Get_Token_Buffer_Type(ETokenType type) => SyntaxFactory.ParseTypeName($"{CodeCommon.ReadOnlySpan}<{Get_Consumer_Data_Type(type)}>");
        public FunctionDefinition Get_Token_Processor_Function_Definition(ETokenType type, string name, IMetaCodeBuilder body) => new(SyntaxPrivateStatic, SyntaxFactory.ParseTypeName("bool"), name, SyntaxFactory.ParseArgumentList($"{Get_Token_Buffer_Type(type)} {VarNameBufferMajor}, out {IdType} id, out int length"), body);
        public FunctionDefinition Get_Local_Token_Consumer_Function_Definition(ETokenType type, string name, IMetaCodeBuilder body) => new(SyntaxFactory.ParseTypeName("bool"), name, SyntaxFactory.ParseArgumentList($"{Get_Token_Buffer_Type(type)} {VarNameBufferMajor}, out int length"), body);
        #endregion

    }

    internal sealed record PatternConsumerList
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
