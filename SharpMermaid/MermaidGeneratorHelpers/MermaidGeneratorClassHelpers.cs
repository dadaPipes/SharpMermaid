using System.Text;

namespace SharpMermaid.MermaidGeneratorHelpers;

/// <summary>
/// Provides helper methods for generating Mermaid class diagrams
/// from .NET types, including classes, interfaces, and relationships.
/// </summary>
public class MermaidGeneratorClassHelpers
{
    /// <summary>
    /// Appends the classDiagram declaration for a Mermaid diagram.
    /// </summary>
    public static void AddClassDeclaration(StringBuilder diagramBuilder)
    {
        diagramBuilder.AppendLine("classDiagram");
    }


}
