using Microsoft.CodeAnalysis;

using System;
using System.Reflection;

namespace MetaParser.CodeGen
{
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
        #endregion

    }
}
