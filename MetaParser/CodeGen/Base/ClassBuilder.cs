using MetaParser.CodeGen;
using MetaParser.CodeGen.Core;
using MetaParser.Contexts;

namespace MetaParser.CodeGen.Base;

internal class ClassBuilder : IMetaCodeBuilder
{
    #region Properties
    public string Accessors { get; }
    public string Name { get; }
    public IMetaCodeBuilder[] Contents { get; }
    #endregion

    public ClassBuilder(string accessors, string name, params IMetaCodeBuilder[] contents)
    {
        Accessors = accessors;
        Name = name;
        Contents = contents;
    }

    public void WriteTo(MetaParserContext context)
    {
        var wr = context.writer;
        wr.WriteLine($"namespace {context.Namespace};");
        //wr.WriteLine(Common.s_generatedCodeAttributeSource);
        wr.WriteLine($"{Accessors} class {Name}");
        wr.WriteLine("{");
        wr.Indent++;

        foreach (var builder in Contents)
        {
            builder.WriteTo(context);
        }

        wr.Indent--;
        wr.WriteLine("}");// end class
    }
}
