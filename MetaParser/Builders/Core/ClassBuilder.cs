using MetaParser.Builders.Interfaces;
using MetaParser.Core;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using System.Collections.Generic;

namespace MetaParser.Builders.Core;
internal class ClassBuilder : MetaCodeBuilder
{
    #region Properties
    public SyntaxTokenList? Modifiers { get; }
    public string Name { get; }
    #endregion

    #region Constructors
    public ClassBuilder(SyntaxTokenList modifiers, string name)
    {
        Modifiers = modifiers;
        Name = name;
    }

    public ClassBuilder(IEnumerable<SyntaxToken> modifiers, string name)
    {
        Modifiers = SyntaxFactory.TokenList(modifiers);
        Name = name;
    }

    public ClassBuilder(string name)
    {
        Name = name;
    }
    #endregion

    protected override void Write(MetaParserContext context)
    {
        var writer = context.writer;
        writer.WriteLine($"namespace {context.Config.Namespace}");
        writer.WriteLine("{");
        writer.Indent++;
#if !DEBUG
        writer.WriteLine(CodeCommon.s_generatedCodeAttributeSource);
#endif

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

        base.WriteContent(context);

        writer.Indent--;
        writer.WriteLine("}");// end class

        writer.Indent--;
        writer.WriteLine("}");// end namespace
    }
}
