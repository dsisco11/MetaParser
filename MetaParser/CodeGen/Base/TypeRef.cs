using MetaParser.CodeGen.Core;
using MetaParser.Contexts;

using Microsoft.CodeAnalysis;

namespace MetaParser.CodeGen.Base
{
    internal class TypeRef : IMetaCodeBuilder
    {
        #region Properties
        public TypeInfo Info { get; }
        #endregion

        #region Constructors
        public TypeRef(TypeInfo info)
        {
            Info = info;
        }
        #endregion


        public void WriteTo(MetaParserContext context)
        {
            context.writer.Write(Info.Type?.ToDisplayString(null));
        }
    }
}
