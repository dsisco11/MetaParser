using MetaParser.Graphs;
using MetaParser.Parsing.Constructs;
using MetaParser.Trees;

using System;
using System.Collections.Generic;

namespace MetaParser.Core;

/// <summary>
/// Holds a registry list of all Tokens, Consumers, and Patterns that are used in the MetaParser.
/// </summary>
internal class MetaParserRegistry
{
    #region Fields
    private readonly KeyTree<EntityKey> _tree = new KeyTree<EntityKey>(new KeyTreeNode<EntityKey>(EntityKey.Default));
    private readonly Dictionary<EntityKey, TokenInfo> _tokens = new();
    private readonly Dictionary<EntityKey, Consumer> _consumers = new();
    private readonly Dictionary<EntityKey, Pattern> _patterns = new();
    #endregion

    #region Accessors
    public KeyTree<EntityKey> Tree => _tree;
    public IReadOnlyDictionary<EntityKey, TokenInfo> Tokens => _tokens;
    public IReadOnlyDictionary<EntityKey, Consumer> Consumers => _consumers;
    public IReadOnlyDictionary<EntityKey, Pattern> Patterns => _patterns;
    #endregion

    #region Item Management
    public void AddToken(TokenInfo token)
    {
        if (_tokens.ContainsKey(token.NodeID))
        {
            throw new ArgumentException($"Token '{token.Name}' already exists in the registry.");
        }
        _tokens.Add(token.NodeID, token);
        Tree.Add(token.NodeID);
    }

    public void AddConsumer(Consumer consumer, EntityKey parentKey)
    {
        if (_consumers.ContainsKey(consumer.NodeID))
        {
            throw new ArgumentException($"Consumer '{consumer.NodeID}' already exists in the registry.");
        }
        _consumers.Add(consumer.NodeID, consumer);
        Tree.AddEdge(consumer.NodeID, parentKey);
    }

    public void AddPattern(Pattern pattern, EntityKey parentKey)
    {
        if (_patterns.ContainsKey(pattern.NodeID))
        {
            throw new ArgumentException($"Pattern '{pattern.NodeID}' already exists in the registry.");
        }
        _patterns.Add(pattern.NodeID, pattern);
        Tree.AddEdge(pattern.NodeID, parentKey);
    }
    #endregion

    #region Index Trackers
    public int GetNextTokenIndex()
    {
        return _tokens.Count + 1;
    }

    public int GetNextConsumerIndex()
    {
        return _consumers.Count + 1;
    }

    public int GetNextPatternIndex()
    {
        return _patterns.Count + 1;
    }
    #endregion

    public bool TryGetToken(string name, out TokenInfo outToken)
    {
        foreach (var token in _tokens.Values)
        {
            if (token.Name == name)
            {
                outToken = token;
                return true;
            }
        }

        outToken = default;
        return false;
    }

    public IEnumerable<EntityKey> GetNodeIDs()
    {
        foreach (var token in _tokens)
        {
            yield return token.Key;
        }
        foreach (var consumer in _consumers)
        {
            yield return consumer.Key;
        }
        foreach (var pattern in _patterns)
        {
            yield return pattern.Key;
        }
    }

    public IEnumerable<IGraphableEntity> GetGraphEntities()
    {
        foreach (var token in _tokens)
        {
            yield return token.Value;
        }
        foreach (var consumer in _consumers)
        {
            yield return consumer.Value;
        }
        foreach (var pattern in _patterns)
        {
            yield return pattern.Value;
        }
    }

}
