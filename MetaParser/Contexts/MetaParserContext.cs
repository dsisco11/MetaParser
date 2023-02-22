using Microsoft.CodeAnalysis;

using System;

namespace MetaParser.Contexts
{
    internal record MetaParserContext
    {
        public string BaseFileName { get; set; } = string.Empty;
        public string Namespace { get; set; } = string.Empty;
        public string? ClassName { get; set; } = "Parser";
        public string? ParserType { get; set; }

        public SpecialType IdType { get; set; } = SpecialType.System_Int32;
        public SpecialType InputType { get; set; } = SpecialType.System_Char;

        #region Accessors
        public string IdTypeName => CodeGen.Format(IdType);
        public string InputTypeName => CodeGen.Format(InputType);
        public string ParserClassDeclaration => $"public sealed partial class {ClassName}";
        #endregion

        #region Constants
        public readonly string TokenEnum = "ETokenType";
        public readonly string TokenConsts = "TokenId";

        public readonly string ConstantTokenConsumerFunctionName = "consume_constant_token";
        public readonly string CompoundTokenConsumerFunctionName = "consume_compound_token";
        public readonly string ComplexTokenConsumerFunctionName = "consume_complex_token";
        #endregion

        #region Utility Functions
        public string Format_TokenId(string? name) => name is null ? throw new ArgumentNullException(nameof(name)) : System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(name.ToLowerInvariant());
        public string Get_TokenId_Ref(string? name) => name is null ? throw new ArgumentNullException(nameof(name)) : $"{TokenConsts}.{Format_TokenId(name)}";
        public string Get_Token_Consumer_Function_Name(string? name) => name is null ? throw new ArgumentNullException(nameof(name)) : $"consume_{name.ToLowerInvariant()}";
        #endregion

    }
}
