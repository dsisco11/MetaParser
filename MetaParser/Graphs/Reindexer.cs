using System;

namespace MetaParser.Graphs;

/// <summary> Maps indices into lower space </summary>
internal class Reindexer
{
    #region Fields
    private readonly int[] newIndex;
    private readonly int[] oldIndex;
    #endregion

    public Reindexer(int[] indices)
    {
        Array.Sort(indices);
        var min = indices[0];
        var max = indices[indices.Length-1];

        newIndex = new int[max+1];
        oldIndex = new int[indices.Length];

        for (int nidx = 0; nidx < indices.Length; nidx++)
        {
            var oidx = indices[nidx];
            oldIndex[nidx] = oidx;
            newIndex[oidx] = nidx;
        }
    }

    public int GetNew(int oldIndice) => newIndex[oldIndice];
    public int GetOld(int newIndice) => oldIndex[newIndice];
}
