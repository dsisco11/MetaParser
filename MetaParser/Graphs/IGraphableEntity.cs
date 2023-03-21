using MetaParser.Core;
using MetaParser.Graphs;

using System.Collections.Generic;

namespace MetaParser.Parsing.Constructs
{
    internal interface IGraphableEntity
    {
        public EntityKey Key { get; }
        public IEnumerable<EntityLink> ResolveLinks(TokenRegistry Registry);
    }
}
