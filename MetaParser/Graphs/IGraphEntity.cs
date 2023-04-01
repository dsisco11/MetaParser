using MetaParser.Core;
using MetaParser.Graphs;

using System.Collections.Generic;

namespace MetaParser.Parsing.Constructs
{
    internal interface IGraphEntity
    {
        public EntityKey Key { get; }
        public IEnumerable<EntityLink> ResolveLinks(EntityRegistry Registry);
    }
}
