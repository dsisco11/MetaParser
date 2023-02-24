using MetaParser.CodeGen;
using MetaParser.CodeGen.Core;
using MetaParser.Contexts;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MetaParser.CodeGen.Base
{
    internal class FunctionDefinition : IMetaCodeBuilder
    {
        public FunctionDefinition(SyntaxTokenList modifiers, TypeSyntax returnType, string name, ArgumentListSyntax arguments, IMetaCodeBuilder body)
        {
            Modifiers = modifiers;
            ReturnType = returnType;
            Name = SyntaxFactory.ParseName(name);
            Arguments = arguments;
            Body = body;
        }

        public SyntaxTokenList Modifiers { get; }
        public TypeSyntax ReturnType { get; }
        public NameSyntax Name { get; }
        public ArgumentListSyntax Arguments { get; }
        public IMetaCodeBuilder Body { get; }

        public void WriteTo(MetaParserContext context)
        {
            var writer = context.writer;
            foreach (var mod in Modifiers)
            {
                mod.WriteTo(writer);
            }

            if (Modifiers.Count > 0)
            {
                writer.Write(" ");
            }

            ReturnType.WriteTo(writer);
            writer.Write(" ");
            Name.WriteTo(writer);
            writer.Write("(");
            Arguments.WriteTo(writer);
            writer.Write(")");
            writer.WriteLine();
            writer.WriteLine("{");
            writer.Indent++;

            Body.WriteTo(context);

            writer.Indent--;
            writer.WriteLine("}");
        }
    }
}
