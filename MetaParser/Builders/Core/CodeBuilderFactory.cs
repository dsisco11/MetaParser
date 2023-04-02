using MetaParser.Builders.Interfaces;
using MetaParser.Builders.Parser.Functions;
using MetaParser.Builders.TokenLogic.Consumer;
using MetaParser.Core;

namespace MetaParser.Builders.Core;

internal class CodeBuilderFactory : ICodeBuilderFactory
{
    #region Fields
    protected readonly ParserConfiguration Config;
    private readonly IMetaCodeBuilder _parsing_logic;
    private readonly IMetaCodeBuilder _parsing_struct_builder;
    private readonly IMetaCodeBuilder _parsing_table_executor;
    private readonly IMetaCodeBuilder _parsing_table_function;
    private readonly IMetaCodeBuilder _token_id_constants_builder;
    private readonly IMetaCodeBuilder _token_id_enum_builder;
    private readonly IMetaCodeBuilder _token_struct_builder;
    private readonly IMetaCodeBuilder _token_pattern_matcher;
    private readonly IMetaCodeBuilder _token_pattern_expression_writer;
    #endregion

    public CodeBuilderFactory(ParserConfiguration config)
    {
        Config = config;
        _parsing_logic = new ParsingLogic();
        _parsing_struct_builder = new ResultStructBuilder();
        _parsing_table_executor = new FuncParsingTableExecutor();
        _parsing_table_function = new FuncProcessParserTable();
        _token_id_constants_builder = new TokenIDConstBuilder();
        _token_id_enum_builder = new TokenIDEnumBuilder();
        _token_struct_builder = new TokenStructBuilder();
        _token_pattern_matcher = new LogicPatternMatcher();
        _token_pattern_expression_writer = new WritePatternAsExpression();
    }

    public IMetaCodeBuilder Get_Parsing_Logic() => _parsing_logic;
    public IMetaCodeBuilder Get_Parsing_Struct_Builder() => _parsing_struct_builder;
    public IMetaCodeBuilder Get_Parsing_Table_Executor() => _parsing_table_executor;
    public IMetaCodeBuilder Get_Parsing_Table_Function() => _parsing_table_function;

    public IMetaCodeBuilder Get_Logic_Consumer_Match() => new LogicSingleConsumer();

    public IMetaCodeBuilder Get_Switch_Block_For_Consumers() => new LogicConsumerSwitchBlock();
    public IMetaCodeBuilder Get_Logic_Detect_Recursive_Token() => new DetectRecursiveTokensAndThen();
    public IMetaCodeBuilder Get_Logic_Single_Token_Detector() => new LogicSingleTokenDetector();
    public IMetaCodeBuilder Get_Logic_Single_Token_Consume() => new LogicSingleTokenDetector();
    public IMetaCodeBuilder Get_Logic_Pattern_Match() => _token_pattern_matcher;
    public IMetaCodeBuilder Get_Pattern_Writer() => _token_pattern_expression_writer;

    public IMetaCodeBuilder Get_Token_ID_Constants_Builder() => _token_id_constants_builder;
    public IMetaCodeBuilder Get_Token_ID_Enum_Builder() => _token_id_enum_builder;
    public IMetaCodeBuilder Get_Token_Struct_Builder() => _token_struct_builder;
}
