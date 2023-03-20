using MetaParser.Builders.Interfaces;
using MetaParser.Builders.Parser.Functions;
using MetaParser.Builders.TokenLogic.Consumer;
using MetaParser.Core;

namespace MetaParser.Builders.Core;

internal class CodeBuilderFactory : ICodeBuilderFactory
{
    #region Fields
    protected readonly MetaParserConfig Config;
    private readonly IMetaCodeBuilder _parsing_logic;
    private readonly IMetaCodeBuilder _token_processing_logic;
    private readonly IMetaCodeBuilder _token_id_constants_builder;
    private readonly IMetaCodeBuilder _token_id_enum_builder;
    private readonly IMetaCodeBuilder _token_struct_builder;
    private readonly IMetaCodeBuilder _token_pattern_matcher;
    private readonly IMetaCodeBuilder _token_pattern_expression_writer;
    #endregion

    public CodeBuilderFactory(MetaParserConfig config)
    {
        Config = config;
        _parsing_logic = new ParsingLogic();
        _token_processing_logic = new TokenProcessor();
        _token_id_constants_builder = new TokenIDConstBuilder();
        _token_id_enum_builder = new TokenIDEnumBuilder();
        _token_struct_builder = new TokenStructBuilder();
        _token_pattern_matcher = new LogicPatternMatcher();
        _token_pattern_expression_writer = new WritePatternAsExpression();
    }

    public IMetaCodeBuilder Get_Parsing_Logic() => _parsing_logic;

    public IMetaCodeBuilder Get_Logic_Consumer_Match() => new LogicSingleConsumer();

    public IMetaCodeBuilder Get_Switch_Block_For_Consumers() => new SwitchBlockForConsumers();
    public IMetaCodeBuilder Get_Logic_Detect_Recursive_Token() => new DetectRecursiveTokensAndThen();
    public IMetaCodeBuilder Get_Logic_Single_Token_Detector() => new LogicSingleTokenDetector();
    public IMetaCodeBuilder Get_Logic_Single_Token_Consume() => new LogicSingleTokenDetector();
    public IMetaCodeBuilder Get_Logic_Pattern_Match() => _token_pattern_matcher;
    public IMetaCodeBuilder Get_Pattern_Writer() => _token_pattern_expression_writer;


    public IMetaCodeBuilder Get_Token_ID_Constants_Builder() => _token_id_constants_builder;

    public IMetaCodeBuilder Get_Token_ID_Enum_Builder() => _token_id_enum_builder;

    public IMetaCodeBuilder Get_Token_Processing_Logic() => _token_processing_logic;

    public IMetaCodeBuilder Get_Token_Struct_Builder() => _token_struct_builder;
}
