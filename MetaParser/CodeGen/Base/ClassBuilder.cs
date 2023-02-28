using MetaParser.CodeGen;
using MetaParser.CodeGen.Core;
using MetaParser.Contexts;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using System.Collections.Generic;

namespace MetaParser.CodeGen.Base;

internal class ClassBuilder : IMetaCodeBuilder
{
    #region Properties
    public SyntaxTokenList? Modifiers { get; } = null;
    public string Name { get; }
    public IMetaCodeBuilder[] Contents { get; }
    #endregion

    #region Constructors
    public ClassBuilder(SyntaxTokenList modifiers, string name, params IMetaCodeBuilder[] contents)
    {
        Modifiers = modifiers;
        Name = name;
        Contents = contents;
    }

    public ClassBuilder(IEnumerable<SyntaxToken> modifiers, string name, params IMetaCodeBuilder[] contents)
    {
        Modifiers = SyntaxFactory.TokenList(modifiers);
        Name = name;
        Contents = contents;
    }

    public ClassBuilder(string name, params IMetaCodeBuilder[] contents)
    {
        Name = name;
        Contents = contents;
    }
    #endregion

    public void WriteTo(MetaParserContext context)
    {
        var writer = context.writer;
        writer.WriteLine($"namespace {context.Namespace};");
        //wr.WriteLine(Common.s_generatedCodeAttributeSource);

        if (Modifiers is not null)
        {
            foreach (var mod in Modifiers)
            {
                mod.WriteTo(writer);
            }

            if (Modifiers?.Count > 0)
            {
                writer.Write(" ");
            }
        }

        writer.WriteLine($"class {Name}");
        writer.WriteLine("{");
        writer.Indent++;

        foreach (var builder in Contents)
        {
            builder.WriteTo(context);
        }

        writer.Indent--;
        writer.WriteLine("}");// end class
    }
}
