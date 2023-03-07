using System.Reflection;

namespace MetaParser.Core;

internal static class CodeCommon
{
    #region Statics
    private static AssemblyName _assemblyName = typeof(Common).Assembly.GetName();
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

}
