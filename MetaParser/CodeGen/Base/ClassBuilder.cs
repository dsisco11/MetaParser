using MetaParser.CodeGen;
using MetaParser.CodeGen.Core;
using MetaParser.Contexts;

using Microsoft.CodeAnalysis;

namespace MetaParser.CodeGen.Base;

internal class ClassBuilder : IMetaCodeBuilder
{
    #region Properties
    public SyntaxTokenList? Modifiers { get; }
    public string Name { get; }
    public IMetaCodeBuilder[] Contents { get; }
    #endregion

    public ClassBuilder(SyntaxTokenList? modifiers, string name, params IMetaCodeBuilder[] contents)
    {
        Modifiers = modifiers;
        Name = name;
        Contents = contents;
    }

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
