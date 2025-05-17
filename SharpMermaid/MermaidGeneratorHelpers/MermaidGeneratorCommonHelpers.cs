using System.Text;

namespace SharpMermaid.MermaidGeneratorHelpers;

/// <summary>
/// Helper methods for generating Mermaid diagrams from .NET project and solution files.
/// </summary>
static class MermaidGeneratorCommonHelpers
{
    /// <summary>
    /// Appends the opening Mermaid code block.
    /// </summary>
    /// <param name="diagramBuilder">The <see cref="StringBuilder"/> to which the Mermaid block start is appended.</param>
    public static void AddMermaidBlockStart(StringBuilder diagramBuilder)
    {
        diagramBuilder.AppendLine("```mermaid");
    }

    /// <summary>
    /// Adds the solution name as a title in the Mermaid diagram.
    /// </summary>
    /// <param name="solutionName">The name of the solution to be used as the diagram title.</param>
    /// <param name="diagramBuilder">The <see cref="StringBuilder"/> to which the title is appended.</param>

    public static void AddSolutionNameAsTitle(string solutionName, StringBuilder diagramBuilder)
    {
        string formattedTitle =

           $"""
            ---
            title: {solutionName}
            ---
            """;

        diagramBuilder.AppendLine(formattedTitle);
    }

    /// <summary>
    /// Appends the closing fence for a Mermaid diagram.
    /// </summary>
    /// <param name="diagramBuilder">The <see cref="StringBuilder"/> to which the footer is appended.</param>
    public static void AddDiagramFooter(StringBuilder diagramBuilder)
    {
        diagramBuilder.Append("```");
    }
}