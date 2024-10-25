using MetaParser.Builders.Interfaces;
using MetaParser.Core;

using System;
using System.Collections.Generic;

namespace MetaParser.Builders.Core;
internal abstract class CodeBuilder
{
    #region Fields
    private List<CodeBuilder> _before;
    private List<CodeBuilder> _after;
    protected List<CodeBuilder> _within;
    #endregion

    protected abstract void WriteTo(CodeGenContext context);

    #region Call Chain Builders

    #region Adding
    public CodeBuilder AddBefore(CodeBuilder codeBuilder)
    {
        _before ??= new List<CodeBuilder>();
        _before.Add(codeBuilder);
        return this;
    }

    public CodeBuilder AddAfter(CodeBuilder codeBuilder)
    {
        _after ??= new List<CodeBuilder>();
        _after.Add(codeBuilder);
        return this;
    }

    public CodeBuilder AddWithin(CodeBuilder codeBuilder)
    {
        _within ??= new List<CodeBuilder>();
        _within.Add(codeBuilder);
        return this;
    }
    #endregion

    #region Removing
    public CodeBuilder RemoveBefore(CodeBuilder codeBuilder)
    {
        _before?.Remove(codeBuilder);
        return this;
    }

    public CodeBuilder RemoveAfter(CodeBuilder codeBuilder)
    {
        _after?.Remove(codeBuilder);
        return this;
    }

    public CodeBuilder RemoveWithin(CodeBuilder codeBuilder)
    {
        _within?.Remove(codeBuilder);
        return this;
    }
    #endregion

    #region Replacing
    public CodeBuilder ReplaceBefore(CodeBuilder oldBuilder, CodeBuilder newBuilder)
    {
        int index = _before?.IndexOf(oldBuilder) ?? -1;
        if (index >= 0)
        {
            _before[index] = newBuilder;
        }
        return this;
    }

    public CodeBuilder ReplaceAfter(CodeBuilder oldBuilder, CodeBuilder newBuilder)
    {
        int index = _after?.IndexOf(oldBuilder) ?? -1;
        if (index >= 0)
        {
            _after[index] = newBuilder;
        }
        return this;
    }

    public CodeBuilder ReplaceWithin(CodeBuilder oldBuilder, CodeBuilder newBuilder)
    {
        int index = _within?.IndexOf(oldBuilder) ?? -1;
        if (index >= 0)
        {
            _within[index] = newBuilder;
        }
        return this;
    }
    #endregion

    #region Clearing
    public CodeBuilder ClearBefore()
    {
        _before?.Clear();
        return this;
    }

    public CodeBuilder ClearAfter()
    {
        _after?.Clear();
        return this;
    }

    public CodeBuilder ClearWithin()
    {
        _within?.Clear();
        return this;
    }
    #endregion

    #region Conditional Adding
    public CodeBuilder AddBeforeIf(Predicate<CodeGenContext> predicate, CodeBuilder codeBuilder)
    {
        return AddBefore(new ConditionalCodeBuilder(predicate, codeBuilder));
    }

    public CodeBuilder AddAfterIf(Predicate<CodeGenContext> condition, CodeBuilder codeBuilder)
    {
        return AddAfter(new ConditionalCodeBuilder(condition, codeBuilder));
    }

    public CodeBuilder AddWithinIf(Predicate<CodeGenContext> condition, CodeBuilder codeBuilder)
    {
        return AddWithin(new ConditionalCodeBuilder(condition, codeBuilder));
    }
    #endregion

    public CodeBuilder ForEach<TItem>(IEnumerable<TItem> items, Func<TItem, CodeBuilder> codeBuilderFunc)
    {
        return AddWithin(new ForEachCodeBuilder<TItem>(items, codeBuilderFunc));
    }
    #endregion

    public void GenerateCode(CodeGenContext context)
    {
        if (_before is not null)
        {
            foreach (var builder in _before)
            {
                builder.GenerateCode(context);
            }
        }

        WriteTo(context);

        if (_after is not null)
        {
            foreach (var builder in _after)
            {
                builder.GenerateCode(context);
            }
        }
    }

    protected void GenerateInnerItems(CodeGenContext context)
    {
        if (_within is not null)
        {
            foreach (var builder in _within)
            {
                builder.GenerateCode(context);
            }
        }
    }
}
