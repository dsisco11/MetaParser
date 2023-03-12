using MetaParser.Core;
using MetaParser.Graphs;
using MetaParser.Parsing.Constructs;

using Microsoft.CodeAnalysis.CSharp;

using System.CodeDom.Compiler;
using System.Linq;

namespace MetaParser.Mermaid;

internal class MermaidFormatter
{
    private TokenGraph Graph;
    private MetaParserRegistry Registry;

    public MermaidFormatter(MetaParserRegistry registry, TokenGraph graph)
    {
        Graph = graph;
        Registry = registry;
    }

    public void Write(IndentedTextWriter writer, MermaidChartType chartType)
    {
        writer.WriteLine(chartType switch
        {
            MermaidChartType.Graph => "graph LR",
            MermaidChartType.EntityRelationship => "erDiagram",
            MermaidChartType.Class => "classDiagram",
            _ => throw new System.NotImplementedException(),
        });

        Write_Node_Definitions(writer, chartType);
        Write_Node_Links(writer, chartType);
    }

    void Write_Node_Definitions(IndentedTextWriter writer, MermaidChartType chartType)
    {
        foreach (var entry in Graph.Nodes)
        {
            Write_Definition(writer, chartType, entry.Key, entry.Value);
        }
    }

    void Write_Definition(IndentedTextWriter writer, MermaidChartType chartType, NodeKey Key, TokenGraph.Node node)
    {
        if (!node.Incoming.Any() && !node.Outgoing.Any())
        {
            return;
        }

        switch (Key.Type)
        {
            case NodeType.Data:
                break;
            case NodeType.Pattern:
                {
                    if (!node.Incoming.Any())
                    {
                        break;
                    }
                    var content = Get_Pattern_Contents(chartType, Registry.Patterns[Key]);
                    writer.WriteLine($"{Key}({content})");
                }
                break;
            case NodeType.Consumer:
                {
                    if (node.Incoming.Any())
                    {
                        writer.WriteLine($@"{Key}{{""{Key.Index}""}}");
                    }
                }
                break;
            case NodeType.Token:
                {
                    var token = Registry.Tokens[Key];
                    writer.WriteLine($@"{Key}[""{token.Name}""]");
                }
                break;
        }
    }

    string Get_Pattern_Contents(MermaidChartType chartType, Pattern pattern)
    {
        return pattern switch
        {
            PatternConst c => $@"{SymbolDisplay.FormatLiteral(c.Value, true)}",
            PatternRange r => $"[{r.Begin}, {r.End}]",
            PatternGroup g => $@"""{{{g.NodeID.Index}}}""",
            PatternTokenRef t => $@"""#{t.TokenName}""",
            _ => throw new System.NotImplementedException(),
        };
    }

    void Write_Node_Links(IndentedTextWriter writer, MermaidChartType chartType)
    {
        var resolved = Graph.Resolve();

        // output all resolved nodes in mermaid entity diagram format
        foreach (var entry in resolved)
        {
            foreach (var dep in entry.Value.Outgoing)
            {
                writer.Write(entry.Key);
                writer.Write(" --> ");
                writer.Write(dep.Key);
                writer.WriteLine();
            }
        }
    }
}
