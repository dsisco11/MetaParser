using MetaParser.Core;
using MetaParser.Graphs;

using System.Collections.Generic;

namespace MetaParser.Parsing.Constructs
{
    internal interface IGraphableEntity
    {
        public NodeKey NodeID { get; }
        public IEnumerable<EntityLink> ResolveLinks(MetaParserContext context);
    }
}
