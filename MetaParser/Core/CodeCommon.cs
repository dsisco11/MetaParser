using MetaParser.Builders.Core;
using MetaParser.Parsing.Constructs;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System;
using System.Reflection;

namespace MetaParser.Core;

internal static class CodeCommon
{
    #region Statics
    private static readonly AssemblyName _assemblyName = typeof(Common).Assembly.GetName();

    public static SyntaxTokenList ParserClassModifiers = SyntaxFactory.TokenList(SyntaxFactory.ParseTokens("public sealed partial"));
    public static SyntaxTokenList SyntaxPrivateStatic = SyntaxFactory.TokenList(SyntaxFactory.ParseTokens("private static"));
    internal static readonly string s_generatedCodeAttributeSource = $@"
[global::System.CodeDom.Compiler.GeneratedCodeAttribute(""{_assemblyName.Name}"", ""{_assemblyName.Version}"")]
";
    #endregion

    #region Constants
    public const string List = "global::System.Collections.Generic.List";
    public const string Span = "global::System.Span";
    public const string Memory = "global::System.Memory";

    public const string ReadOnlySpan = "global::System.ReadOnlySpan";
    public const string ReadOnlyMemory = "global::System.ReadOnlyMemory";

    public const string TokenEnum = "ETokenType";
    public const string TokenConsts = "TokenId";
    public const string UnknownToken = "unknown";

    public const string TokenDataClassName = "TokenData";
    public const string TokenValueStructName = "ValueToken";
    public const string TokenRecordTypeName = "Token";

    public const string ConstantTokenProcessorFunctionName = "TryProcessConstant";
    public const string CompoundTokenProcessorFunctionName = "TryProcessCompound";
    public const string ComplexTokenProcessorFunctionName = "TryProcessComplex";
    #endregion

    #region Formatting
    public static string Format_Pattern_Consumer_Function_Name(int consumerIndex) => $"consume_pattern_{consumerIndex}";
    public static string Format_Token_Start_Detection_Function_Name(string tokenName) => $"is_{tokenName.ToLowerInvariant()}_token_start";
    public static string Format_Pattern_Start_Detection_Function_Name(int consumerIndex) => $"is_consumer_start_{consumerIndex}";

    public static string Format_Token_Id(string? name) => name is null ? throw new ArgumentNullException(nameof(name)) : System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(name);
    public static string Format_Token_Key(string? name) => name is null ? throw new ArgumentNullException(nameof(name)) : name.ToLowerInvariant();
    public static string Format_Token_Key(TokenInfo? token) => token is null ? throw new ArgumentNullException(nameof(token)) : Format_Token_Key(token.Name);
    public static string Format_Token_Id_Const_Ref(string? name) => $"{TokenConsts}.{Format_Token_Id(name)}";
    public static string Format_Token_Id_Const_Ref(TokenInfo? token) => token is null ? throw new ArgumentNullException(nameof(token)) : $"{TokenConsts}.{Format_Token_Id(token.Name)}";
    #endregion


    #region Builders
    public static TypeSyntax Get_Consumer_Data_Type(MetaParserConfig config, EConsumerType type) => (type == EConsumerType.Lexer ? config.InputType : config.IdType);
    public static TypeSyntax Get_Token_Buffer_Type(MetaParserConfig config, EConsumerType type) => SyntaxFactory.ParseTypeName($"{ReadOnlySpan}<{Get_Consumer_Data_Type(config, type)}>");
    public static FunctionDefinition Get_Token_Processor_Function_Definition(MetaParserContext context, EConsumerType type, string name) => (FunctionDefinition)new FunctionDefinition(SyntaxPrivateStatic, SyntaxFactory.ParseTypeName("bool"), name, SyntaxFactory.ParseArgumentList($"{Get_Token_Buffer_Type(context.Config, type)} {context.ActiveBufferName}, out {context.Config.IdType} id, out int length")).And(context.Config.CodeFactory.Get_Token_Processing_Logic());
    public static FunctionDefinition Get_Local_Token_Consumer_Function_Definition(MetaParserContext context, EConsumerType type, string name) => (FunctionDefinition)new FunctionDefinition(SyntaxFactory.ParseTypeName("bool"), name, SyntaxFactory.ParseArgumentList($"{Get_Token_Buffer_Type(context.Config, type)} {context.ActiveBufferName}, out int length")).And(context.Config.CodeFactory.Get_Logic_Consumer_Match());
    public static FunctionDefinition Get_Local_Token_Detection_Function_Definition(MetaParserContext context, EConsumerType type, string name) => (FunctionDefinition)new FunctionDefinition(SyntaxFactory.ParseTypeName("bool"), name, SyntaxFactory.ParseArgumentList($"{Get_Token_Buffer_Type(context.Config, type)} {context.ActiveBufferName}")).And(context.Config.CodeFactory.Get_Logic_Single_Token_Detector());
    #endregion
}
