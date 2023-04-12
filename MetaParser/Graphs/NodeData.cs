namespace MetaParser.Graphs;

public sealed record NodeData()
{
    #region Static
    public static readonly NodeData Default = new();
    #endregion

    #region Properties
    public int Order { get; set; } = -1;
    public int Depth { get; set; } = 0;
    #endregion

    #region Accessors
    public bool IsRecursive => Order == -1;
    #endregion

    #region Methods
    public void Update_Depth(NodeData ancestor)
    {
        if (ancestor.Depth + 1 > Depth)
        {
            Depth = ancestor.Depth + 1;
        }
    }
    #endregion
}