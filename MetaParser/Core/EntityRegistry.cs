using MetaParser.Graphs;
using MetaParser.Parsing.Constructs;
using MetaParser.Trees;

using System;
using System.Collections.Generic;

namespace MetaParser.Core;

/// <summary>
/// Holds a registry list of all Tokens, Consumers, and Patterns that are used in the MetaParser.
/// </summary>
internal class EntityRegistry
{
    #region Fields
    private readonly KeyTree<EntityKey> _tree = new(new KeyTreeNode<EntityKey>(EntityKey.Default));
    /// <summary> Keeps track of entity population count according to node type </summary>
    private readonly Dictionary<NodeType, int> _tracker = new();
    private readonly Dictionary<EntityKey, GraphEntity> _entities = new();
    private readonly Dictionary<string, EntityKey> _entity_ids = new();

    private readonly DirectedNodeGraph<EntityKey> _tokenGraph = new();
    private readonly DirectedNodeGraph<EntityKey> _consumerGraph = new();
    private readonly DirectedNodeGraph<EntityKey> _patternGraph = new();
    #endregion

    #region Accessors
    public KeyTree<EntityKey> Tree => _tree;
    public IReadOnlyDictionary<EntityKey, GraphEntity> Entities => _entities;
    public IEnumerable<TokenEntity> Tokens => GetEntitiesOfType<TokenEntity>(NodeType.Token);
    public IEnumerable<ConsumerEntity> Consumers => GetEntitiesOfType<ConsumerEntity>(NodeType.Consumer);
    public IEnumerable<PatternEntity> Patterns => GetEntitiesOfType<PatternEntity>(NodeType.Pattern);

    internal DirectedNodeGraph<EntityKey> TokenGraph => _tokenGraph;
    internal DirectedNodeGraph<EntityKey> ConsumerGraph => _consumerGraph;
    internal DirectedNodeGraph<EntityKey> PatternGraph => _patternGraph;
    #endregion

    #region Item Management
    private void Mutate_Population_Count(int delta, NodeType type)
    {
        if (!_tracker.ContainsKey(type))
        {
            _tracker.Add(type, 0);
        }
        _tracker[type] += delta;
    }

    public void Add(GraphEntity entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        if (_entities.ContainsKey(entity.Key))
        {
            throw new ArgumentException($"Entity '{entity.Key}' already exists in the registry.");
        }

        _entities.Add(entity.Key, entity);
        if (entity.Key.Type == NodeType.Token)
        {
            var tokenEnt = (TokenEntity)entity;
            _entity_ids[tokenEnt.ID] = tokenEnt.Key;
            _entity_ids[tokenEnt.Name] = tokenEnt.Key;
        }

        Mutate_Population_Count(1, entity.Key.Type);
    }
    #endregion

    #region Index Trackers
    public int GetNextIndex(NodeType type)
    {
        if (_tracker.ContainsKey(type))
        {
            return _tracker[type];
        }
        return 0;
    }

    [Obsolete]
    public int GetNextTokenIndex()
    {
        return GetNextIndex(NodeType.Token);
    }

    [Obsolete]
    public int GetNextConsumerIndex()
    {
        return GetNextIndex(NodeType.Consumer);
    }

    [Obsolete]
    public int GetNextPatternIndex()
    {
        return GetNextIndex(NodeType.Pattern);
    }
    #endregion

    #region Entity Lookups
    public bool TryGetEntity(EntityKey key, out GraphEntity entity)
    {
        return _entities.TryGetValue(key, out entity);
    }

    public bool TryGetEntity<T>(EntityKey key, out T entity) where T : GraphEntity
    {
        bool result = _entities.TryGetValue(key, out var outEntity);
        entity = (T)outEntity;
        return result;
    }

    public bool TryGetEntityByName<T>(string name, out T outEntity) where T : GraphEntity
    {
        if (_entity_ids.TryGetValue(name, out var key))
        {
            outEntity = (T)_entities[key];
            return true;
        }
        outEntity = default;
        return false;
    }
    #endregion

    #region Typed Entity Accessors
    public IEnumerable<T> GetEntitiesOfType<T>(NodeType nodeType) where T : GraphEntity
    {
        foreach (var entry in _entities)
        {
            if (entry.Key.Type == nodeType)
            {
                yield return (T)entry.Value;
            }
        }
    }
    #endregion

    #region Graph Building
    public void BuildGraphs()
    {
        _tokenGraph.Clear();
        _consumerGraph.Clear();
        _patternGraph.Clear();

        foreach (var entry in _entities)
        {
            switch (entry.Key.Type)
            {
                case NodeType.Token:
                    _tokenGraph.TryAdd(entry.Key);
                    break;
                case NodeType.Consumer:
                    _consumerGraph.TryAdd(entry.Key);
                    break;
                case NodeType.Pattern:
                    _patternGraph.TryAdd(entry.Key);
                    break;
            }
        }

        foreach (var entity in _entities.Values)
        {
            var resolvedLinks = entity.ResolveLinks(this);
            foreach (var link in resolvedLinks)
            {
                if (link.Source.Type != link.Target.Type)
                {
                    continue;
                }

                switch (link.Source.Type)
                {
                    case NodeType.Token:
                        _tokenGraph.TryLink(link.Source, link.Target);
                        break;
                    case NodeType.Consumer:
                        _consumerGraph.TryLink(link.Source, link.Target);
                        break;
                    case NodeType.Pattern:
                        _patternGraph.TryLink(link.Source, link.Target);
                        break;
                }
            }
        }
        var resolved = _tokenGraph.Data;
        foreach (var entry in resolved)
        {
            _entities[entry.Key].GraphInfo = entry.Value;
        }
        resolved = _consumerGraph.Data;
        foreach (var entry in resolved)
        {
            _entities[entry.Key].GraphInfo = entry.Value;
        }
        resolved = _patternGraph.Data;
        foreach (var entry in resolved)
        {
            _entities[entry.Key].GraphInfo = entry.Value;
        }
    }
    #endregion
}
