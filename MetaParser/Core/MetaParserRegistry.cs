using MetaParser.Graphs;
using MetaParser.Parsing.Constructs;
using MetaParser.Trees;

using System;
using System.Collections.Generic;
using System.Linq;

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
    public void RegisterToken(TokenInfo token)
    {
        if (_tokens.ContainsKey(token.Key))
        {
            throw new ArgumentException($"Token '{token.Name}' already exists in the registry.");
        }
        _tokens.Add(token.Key, token);
        Tree.Add(token.Key);
    }

    public void RegisterConsumer(Consumer consumer, EntityKey parentKey)
    {
        if (_consumers.ContainsKey(consumer.Key))
        {
            throw new ArgumentException($"Consumer '{consumer.Key}' already exists in the registry.");
        }
        _consumers.Add(consumer.Key, consumer);
        Tree.AddEdge(consumer.Key, parentKey);
    }

    public Pattern RegisterPattern(Pattern pattern, EntityKey parentKey)
    {
        if (_patterns.ContainsKey(pattern.Key))
        {
            throw new ArgumentException($"Pattern '{pattern.Key}' already exists in the registry.");
        }

        if (_patterns.ContainsValue(pattern))
        {
            pattern = _patterns.Values.First(p => p == pattern);
        }

        _patterns.Add(pattern.Key, pattern);
        Tree.AddEdge(pattern.Key, parentKey);

        return pattern;
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
