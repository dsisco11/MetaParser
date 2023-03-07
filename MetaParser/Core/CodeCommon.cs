using MetaParser.Tokens;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using System;
using System.Reflection;

namespace MetaParser.Core;

internal static class CodeCommon
{
    #region Statics
    private static AssemblyName _assemblyName = typeof(Common).Assembly.GetName();

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

    #region Formatting
    public static string Format_Token_Key(string? name) => name is null ? throw new ArgumentNullException(nameof(name)) : name.ToLowerInvariant();
    public static string Format_Token_Id(string? name) => name is null ? throw new ArgumentNullException(nameof(name)) : System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(name);
    public static string Get_TokenId_Ref(string? name) => $"{CodeCommon.TokenConsts}.{Format_Token_Id(name)}";
    public static string Format_Pattern_Consumer_Function_Name(int consumerIndex) => $"consume_pattern_{consumerIndex}";

    public static string Get_Token_Key(TokenInfo? token) => token is null ? throw new ArgumentNullException(nameof(token)) : Format_Token_Key(token.Name);
    public static string Get_TokenId_Ref(TokenInfo? token) => token is null ? throw new ArgumentNullException(nameof(token)) : $"{CodeCommon.TokenConsts}.{Format_Token_Id(token.Name)}";
    #endregion
}
