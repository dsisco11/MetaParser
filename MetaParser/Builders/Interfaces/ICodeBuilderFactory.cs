namespace MetaParser.Builders.Interfaces;

internal interface ICodeBuilderFactory
{
    IMetaCodeBuilder Get_Parsing_Logic();
    IMetaCodeBuilder Get_Token_Consumer_Logic();
    IMetaCodeBuilder Get_Token_Detection_Logic();
    IMetaCodeBuilder Get_Token_ID_Constants_Builder();
    IMetaCodeBuilder Get_Token_ID_Enum_Builder();
    IMetaCodeBuilder Get_Token_Processing_Logic();
    IMetaCodeBuilder Get_Token_Struct_Builder();
}