using MetaParser.Core;
using MetaParser.Graphs;

using System.Collections.Generic;

namespace MetaParser.Parsing.Constructs
{
    internal interface IGraphableEntity
    {
        public EntityKey NodeID { get; }
        public IEnumerable<EntityLink> ResolveLinks(MetaParserRegistry Registry);
    }
}
