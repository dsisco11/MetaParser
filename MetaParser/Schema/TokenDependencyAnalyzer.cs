using System;
using System.Collections.Generic;
using MetaParser.Graphs;

namespace MetaParser.Schema;

/// <summary>
/// Analyzes token dependencies in a schema definition.
/// 
/// Builds a directed graph where:
/// - Nodes are token names
/// - Edges represent dependencies (A → B means A references B via $token pattern)
/// 
/// This is used for:
/// - Detecting circular dependencies (error)
/// - Finding unreachable tokens (warning)
/// - Computing optimal generation order via topological sort
/// </summary>
internal sealed class TokenDependencyAnalyzer
{
    private readonly SchemaDefinition _schema;
    private readonly DirectedGraph<string> _graph;
    private bool _analyzed;
    
    /// <summary>
    /// Gets the dependency graph after analysis.
    /// </summary>
    public DirectedGraph<string> Graph => _graph;
    
    /// <summary>
    /// Creates a new analyzer for the given schema.
    /// </summary>
    public TokenDependencyAnalyzer(SchemaDefinition schema)
    {
        _schema = schema ?? throw new ArgumentNullException(nameof(schema));
        _graph = new DirectedGraph<string>();
    }
    
    /// <summary>
    /// Analyzes the schema and builds the dependency graph.
    /// </summary>
    public void Analyze()
    {
        if (_analyzed) return;
        
        // Add all tokens as nodes
        foreach (var tokenName in _schema.Tokens.Keys)
        {
            _graph.AddNode(tokenName);
        }
        
        // Analyze each token's patterns for references
        foreach (var kvp in _schema.Tokens)
        {
            CollectDependencies(kvp.Key, kvp.Value);
        }
        
        _analyzed = true;
    }
    
    /// <summary>
    /// Gets whether the schema has circular dependencies.
    /// </summary>
    public bool HasCircularDependencies()
    {
        EnsureAnalyzed();
        return CycleDetection.HasCycles(_graph);
    }
    
    /// <summary>
    /// Gets the first cycle found in the schema, if any.
    /// </summary>
    /// <returns>A list of token names forming a cycle, or null if none.</returns>
    public IReadOnlyList<string>? FindFirstCycle()
    {
        EnsureAnalyzed();
        return CycleDetection.FindFirstCycle(_graph);
    }
    
    /// <summary>
    /// Gets all tokens that are part of circular dependencies.
    /// </summary>
    public IReadOnlyCollection<string> GetCyclicTokens()
    {
        EnsureAnalyzed();
        return CycleDetection.GetCyclicNodes(_graph);
    }
    
    /// <summary>
    /// Gets tokens that are defined but never referenced by other tokens.
    /// These may be entry points (good) or dead code (warning).
    /// A token is unreferenced if no other token uses $tokenName to reference it.
    /// </summary>
    public IReadOnlyList<string> GetUnreferencedTokens()
    {
        EnsureAnalyzed();
        
        var unreferenced = new List<string>();
        foreach (var token in _schema.Tokens.Keys)
        {
            // A token is unreferenced if nothing depends on it (no outgoing edges)
            // Edge direction: dependency → dependent, so outgoing = "is referenced by"
            if (_graph.GetDependents(token).Count == 0)
            {
                unreferenced.Add(token);
            }
        }
        return unreferenced;
    }
    
    /// <summary>
    /// Gets tokens that reference undefined tokens.
    /// </summary>
    public IReadOnlyDictionary<string, IReadOnlyList<string>> GetUndefinedReferences()
    {
        EnsureAnalyzed();
        
        var undefined = new Dictionary<string, List<string>>();
        
        foreach (var kvp in _schema.Tokens)
        {
            var tokenName = kvp.Key;
            var tokenDef = kvp.Value;
            var refs = new List<string>();
            CollectTokenReferences(tokenDef.Start, refs);
            CollectTokenReferences(tokenDef.Consume, refs);
            CollectTokenReferences(tokenDef.Stop, refs);
            CollectTokenReferences(tokenDef.Escape, refs);
            
            var undefinedRefs = new List<string>();
            foreach (var refName in refs)
            {
                if (!_schema.Tokens.ContainsKey(refName))
                {
                    undefinedRefs.Add(refName);
                }
            }
            
            if (undefinedRefs.Count > 0)
            {
                undefined[tokenName] = undefinedRefs;
            }
        }
        
        return undefined as IReadOnlyDictionary<string, IReadOnlyList<string>> 
            ?? new Dictionary<string, IReadOnlyList<string>>();
    }
    
    /// <summary>
    /// Gets tokens in dependency order (dependencies before dependents).
    /// Returns null if there are circular dependencies.
    /// </summary>
    public IReadOnlyList<string>? GetGenerationOrder()
    {
        EnsureAnalyzed();
        
        var result = TopologicalSort.KahnSort(_graph);
        return result.IsSuccess ? result.SortedNodes : null;
    }
    
    /// <summary>
    /// Gets the dependency level of each token (0 = no dependencies, higher = more dependencies).
    /// Useful for organizing tokens into consumer levels.
    /// </summary>
    public IReadOnlyDictionary<string, int> GetDependencyLevels()
    {
        EnsureAnalyzed();
        
        var levels = new Dictionary<string, int>();
        var sortResult = TopologicalSort.KahnSort(_graph);
        
        if (!sortResult.IsSuccess)
        {
            // Can't compute levels with cycles - return 0 for all
            foreach (var token in _schema.Tokens.Keys)
            {
                levels[token] = 0;
            }
            return levels;
        }
        
        // Compute levels by traversing in topological order
        foreach (var token in sortResult.SortedNodes)
        {
            var maxDepLevel = -1;
            foreach (var dep in _graph.GetDependents(token))
            {
                if (levels.TryGetValue(dep, out var depLevel))
                {
                    maxDepLevel = Math.Max(maxDepLevel, depLevel);
                }
            }
            levels[token] = maxDepLevel + 1;
        }
        
        return levels;
    }
    
    private void EnsureAnalyzed()
    {
        if (!_analyzed)
            Analyze();
    }
    
    private void CollectDependencies(string tokenName, TokenDefinition tokenDef)
    {
        var references = new List<string>();
        
        CollectTokenReferences(tokenDef.Start, references);
        CollectTokenReferences(tokenDef.Consume, references);
        CollectTokenReferences(tokenDef.Stop, references);
        CollectTokenReferences(tokenDef.Escape, references);
        
        foreach (var refName in references)
        {
            // Edge: tokenName depends on refName (tokenName → refName)
            // In our graph, we model it as refName → tokenName (dependency to dependent)
            _graph.AddEdge(refName, tokenName);
        }
    }
    
    private static void CollectTokenReferences(PatternDefinition? pattern, List<string> references)
    {
        if (pattern == null) return;
        
        switch (pattern)
        {
            case TokenReferencePattern tokenRef:
                if (!references.Contains(tokenRef.TokenName))
                {
                    references.Add(tokenRef.TokenName);
                }
                break;
                
            case OneOfPattern oneOf:
                foreach (var subPattern in oneOf.Patterns)
                {
                    CollectTokenReferences(subPattern, references);
                }
                break;
                
            // LiteralPattern and RangePattern have no token references
        }
    }
}
