using Microsoft.CodeAnalysis.CSharp;

using System;

namespace MetaParser.Contexts
{
    internal record MetaParserContext
    {
        public string BaseFileName { get; set; } = string.Empty;
        public string Namespace { get; set; } = string.Empty;

        public Type IdType { get; set; } = typeof(int);
        public Type InputType{ get; set; } = typeof(char);

        public string IdTypeName => IdType.FullName;
        public string InputTypeName => InputType.FullName;

        public readonly string TokenEnum = "EToken";
        public readonly string TokenConsts = "TokenId";

        public string Format_TokenId(string? name) => name is null ? throw new ArgumentNullException(nameof(name)) : System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(name.ToLowerInvariant());
        public string Get_TokenId_Ref(string? name) => name is null ? throw new ArgumentNullException(nameof(name)) : $"{TokenConsts}.{Format_TokenId(name)}";

    }
}
