using System.CodeDom.Compiler;

namespace MetaParser.Graphs;

internal static class MermaidFormatter
{
    public static bool TryFormat(IndentedTextWriter writer, TokenGraph graph)
    {
        writer.WriteLine("erDiagram");
        var resolved = graph.Resolve();
        writer.WriteLine("graph LR");        
        foreach (var node in graph.Nodes)
        {
            writer.WriteLine($"{node.Key}({node.Key})");
        }
        // output all resolved nodes in mermaid entity diagram format
        foreach (var entry in resolved)
        {
            foreach (var dep in entry.Value.Outgoing)
            {
                writer.WriteLine($"{entry.Key} --> {dep}");
            }
        }

        return true;
    }
}
