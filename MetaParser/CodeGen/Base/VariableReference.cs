using MetaParser.CodeGen.Core;
using MetaParser.Contexts;

namespace MetaParser.CodeGen.Base
{
    internal class VariableReference : IMetaCodeBuilder
    {
        public IMetaCodeBuilder Type { get; }
        public string Name { get; }

        public VariableReference(IMetaCodeBuilder type, string name)
        {
            Type = type;
            Name = name;
        }

        public void WriteTo(MetaParserContext context)
        {
            Type.WriteTo(context);
            context.writer.Write(" ");
            context.writer.Write(Name);
        }
    }
}
