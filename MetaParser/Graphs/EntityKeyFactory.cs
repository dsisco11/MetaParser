using System.Collections.Generic;

namespace MetaParser.Graphs;

internal class EntityKeyFactory
{
    #region Fields
    Dictionary<NodeType, int> idTracker = new();
    #endregion

    #region Constructors
    public EntityKeyFactory()
    {
    }
    #endregion

    #region Methods
    public EntityKey Next(NodeType type)
    {
        if (!idTracker.ContainsKey(type))
        {
            idTracker[type] = 0;
        }
        else
        {
            idTracker[type] += 1;
        }

        return new EntityKey(type, idTracker[type]);
    }
    #endregion

}
