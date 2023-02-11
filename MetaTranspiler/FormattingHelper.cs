using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace MetaTranspiler
{
    //internal class Indenter
    //{
    //    public string PrettyPrint(ReadOnlyMemory<char> buf)
    //    {
    //        int indentLevel = 0;
    //        ReadOnlySequence<char> seq = new(buf);
    //        SequenceReader<char> rd = new(seq);
    //        SequencePosition pos = seq.Start;
    //        var sb = new StringBuilder();

    //        static string consume()
    //        {

    //        }


    //        while (!rd.End)
    //        {
    //            if (!rd.TryAdvanceToAny("\n{}"))
    //            {
    //                break;
    //            }
    //        }

    //        sb.Append(new String("\t", indentLevel));

    //        return sb.ToString();
    //    }
    //}
}
