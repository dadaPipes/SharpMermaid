using SharpMermaid.Models;
using System.Text;

namespace SharpMermaid.Features.GenerateClassDiagrams;
public static class Rules
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
    /// Appends the closing fence for a Mermaid diagram.
    /// </summary>
    /// <param name="diagramBuilder">The <see cref="StringBuilder"/> to which the footer is appended.</param>
    public static void AddDiagramFooter(StringBuilder diagramBuilder)
    {
        diagramBuilder.Append("```");
    }

    /// <summary>
    /// Appends the classDiagram declaration for a Mermaid diagram.
    /// </summary>
    public static void AddClassDeclaration(StringBuilder diagramBuilder)
    {
        diagramBuilder.AppendLine("classDiagram");
    }

    /// <summary>
    /// Adds the csproj name as a title in the Mermaid diagram.
    /// </summary>
    /// <param name="csProjName">The name of the csproj to be used as the diagram title.</param>
    /// <param name="diagramBuilder">The <see cref="StringBuilder"/> to which the title is appended.</param>

    public static void AddCsProjectAsTitle(string csProjName, StringBuilder diagramBuilder)
    {
        string formattedTitle =

           $"""
            ---
            title: {csProjName}
            ---
            """;

        diagramBuilder.AppendLine(formattedTitle);
    }

    public static void AddCsFileNames(IEnumerable<CsModel> csFiles, StringBuilder diagramBuilder)
    {
        foreach (var csFile in csFiles)
        {
            diagramBuilder.AppendLine($"    class {csFile.Name}");
        }
    }

    public static void AddClickableLinks(CsprojModel csproj, StringBuilder diagramBuilder)
    {
        var baseUrl = Settings.BaseUrl.TrimEnd('/');

        foreach (var csFile in csproj.CsFiles)
        {
            var filePath = csFile.RelativePathFromCsProjDirectory.Replace('\\', '/');
            var fullUrl = $"{baseUrl}/{csproj.RelativePathFromSlnWithoutExtension}/{filePath}";

            diagramBuilder.AppendLine($"    click {csFile.Name} href \"{fullUrl}\"");
        }
    }
}
