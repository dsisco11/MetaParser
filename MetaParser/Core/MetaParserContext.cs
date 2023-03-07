using MetaParser.Builders.Core;
using MetaParser.Builders.Interfaces;
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

        #region Builders
        public TypeSyntax Get_Consumer_Data_Type(EConsumerType type) => (type == EConsumerType.Data? InputType : IdType);
        public TypeSyntax Get_Token_Buffer_Type(EConsumerType type) => SyntaxFactory.ParseTypeName($"{CodeCommon.ReadOnlySpan}<{Get_Consumer_Data_Type(type)}>");
        public FunctionDefinition Get_Token_Processor_Function_Definition(EConsumerType type, string name, IMetaCodeBuilder body) => new(CodeCommon.SyntaxPrivateStatic, SyntaxFactory.ParseTypeName("bool"), name, SyntaxFactory.ParseArgumentList($"{Get_Token_Buffer_Type(type)} {CodeCommon.VarNameBufferMajor}, out {IdType} id, out int length"), body);
        public FunctionDefinition Get_Local_Token_Consumer_Function_Definition(EConsumerType type, string name, IMetaCodeBuilder body) => new(SyntaxFactory.ParseTypeName("bool"), name, SyntaxFactory.ParseArgumentList($"{Get_Token_Buffer_Type(type)} {CodeCommon.VarNameBufferMajor}, out int length"), body);
        #endregion

    }
}
