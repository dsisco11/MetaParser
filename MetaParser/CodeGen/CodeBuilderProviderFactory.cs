using MetaParser.CodeGen.Interfaces;
using MetaParser.Core;

namespace MetaParser.CodeGen;

internal abstract class CodeBuilderProviderFactory
{
    #region Properties
    protected readonly MetaParserContext Context;
    #endregion

    #region Constructors
    public CodeBuilderProviderFactory(MetaParserContext context)
    {
        Context = context;
    }
    #endregion

    public abstract IMetaCodeBuilder Get_Parsing_Logic();
    public abstract IMetaCodeBuilder Get_Token_Struct_Builder();
    public abstract IMetaCodeBuilder Get_Token_ID_Enum_Builder();
    public abstract IMetaCodeBuilder Get_Token_ID_Constants_Builder();
    public abstract IMetaCodeBuilder Get_Token_Detection_Logic();
    public abstract IMetaCodeBuilder Get_Token_Processing_Logic();
    public abstract IMetaCodeBuilder Get_Token_Consumer_Logic();
}
