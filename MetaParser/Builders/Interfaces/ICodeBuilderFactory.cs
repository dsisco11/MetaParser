namespace MetaParser.Builders.Interfaces;

internal interface ICodeBuilderFactory
{
    IMetaCodeBuilder Get_Parsing_Logic();
    IMetaCodeBuilder Get_Logic_Consumer_Match();
    IMetaCodeBuilder Get_Switch_Block_For_Consumers();
    IMetaCodeBuilder Get_Logic_Detect_Recursive_Token();
    IMetaCodeBuilder Get_Token_ID_Constants_Builder();
    IMetaCodeBuilder Get_Token_ID_Enum_Builder();
    IMetaCodeBuilder Get_Token_Processing_Logic();
    IMetaCodeBuilder Get_Token_Struct_Builder();
    IMetaCodeBuilder Get_Logic_Single_Token_Detector();
    IMetaCodeBuilder Get_Logic_Single_Token_Consume();
    IMetaCodeBuilder Get_Logic_Pattern_Match();
    IMetaCodeBuilder Get_Pattern_Writer();
}