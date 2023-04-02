using MetaParser.Parsing.Constructs.Stages;

namespace MetaParser.Core;

internal record struct CodeGenState
{
    #region Fields
    public int ActiveBuffer;
    public WorkingSet Targets;
    public ParsingStageContext Stage;
    #endregion

    #region Accessors
    public string LastBufferName => FormatBufferName(ActiveBuffer - 1);
    public string ActiveBufferName => FormatBufferName(ActiveBuffer);
    public string NextBufferName => FormatBufferName(ActiveBuffer + 1);
    #endregion

    #region Constructors
    public CodeGenState(ParsingStageContext stage)
    {
        Stage = stage;
        Targets = new WorkingSet(stage.Consumers);
    }
    #endregion

    #region Methods
    private static string FormatBufferName(int buffer) => $"buffer{buffer}";
    #endregion
}
