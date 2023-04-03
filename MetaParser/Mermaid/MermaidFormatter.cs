using MetaParser.Graphs;

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace MetaParser.Mermaid;

internal static class MermaidFormatter
{
    public static void Write(IndentedTextWriter writer, MermaidChartType chartType, IEnumerable<EntityKey> Nodes, IReadOnlyDictionary<EntityKey, IEnumerable<EntityKey>> Hierarchy, Func<EntityKey, string>? nodeTitleFormatter = null)
    {
        writer.WriteLine("/*");
        writer.WriteLine("```mermaid");
        writer.WriteLine(chartType switch
        {
            MermaidChartType.Graph => "graph LR",
            MermaidChartType.EntityRelationship => "erDiagram",
            MermaidChartType.Class => "classDiagram",
            _ => throw new System.NotImplementedException(),
        });

        // First, write out all of the defined nodes
        foreach (var node in Nodes)
        {
            if (nodeTitleFormatter is not null)
            {
                string nodeTitle = nodeTitleFormatter(node);
                if (!string.IsNullOrEmpty(nodeTitle))
                {
                    writer.Write(node);
                    writer.Write(@"[""");
                    writer.Write(nodeTitle);
                    writer.Write(@"""]");
                    writer.WriteLine();
                    continue;
                }
            }

            writer.WriteLine(node);
        }

        // Last, write out all of the node hierarchal relationships
        foreach (var entry in Hierarchy)
        {
            foreach (var child in entry.Value)
            {
                writer.Write(entry.Key);
                writer.Write(" --> ");
                writer.Write(child);
                writer.WriteLine();
            }
        }

        writer.WriteLine("```");
        writer.WriteLine("*/");
    }
}
