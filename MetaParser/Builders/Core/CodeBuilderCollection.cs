using MetaParser.Core;

using System.Collections.Generic;

namespace MetaParser.Builders.Core;
internal class CodeBuilderCollection : CodeBuilder
{
    private readonly List<CodeBuilder> _codeBuilders;

    public CodeBuilderCollection()
    {
        _codeBuilders = new List<CodeBuilder>();
    }

    public CodeBuilderCollection(IEnumerable<CodeBuilder> codeBuilders)
    {
        _codeBuilders = new List<CodeBuilder>(codeBuilders);
    }

    public void Add(CodeBuilder codeBuilder)
    {
        _codeBuilders.Add(codeBuilder);
    }

    public bool Remove(CodeBuilder codeBuilder)
    {
        return _codeBuilders.Remove(codeBuilder);
    }

    public void Clear()
    {
        _codeBuilders.Clear();
    }

    protected override void WriteTo(CodeGenContext context)
    {
        foreach (var builder in _codeBuilders)
        {
            builder.GenerateCode(context);
        }
    }
}
