namespace MetaParser.Core;

internal record struct CodeGenState
{
    #region Fields
    public WorkingSet Targets;
    public int ActiveBuffer;
    #endregion

    #region Accessors
    public string LastBufferName => FormatBufferName(ActiveBuffer - 1);
    public string ActiveBufferName => FormatBufferName(ActiveBuffer);
    public string NextBufferName => FormatBufferName(ActiveBuffer + 1);
    #endregion

    #region Constructors
    public CodeGenState(WorkingSet targets)
    {
        Targets = targets;
    }
    #endregion

    #region Methods
    private static string FormatBufferName(int buffer) => $"buffer{buffer}";
    #endregion
}
