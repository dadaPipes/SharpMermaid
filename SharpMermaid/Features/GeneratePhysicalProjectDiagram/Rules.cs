using SharpMermaid.Models;
using System.Text;
using Spectre.Console;

namespace SharpMermaid.Features.GeneratePhysicalProjectDiagram;
public static class Rules
{
    /// <summary>
    /// Appends the opening Mermaid code block.
    /// </summary>
    /// <param name="diagramBuilder">The <see cref="StringBuilder"/> to which the Mermaid block start is appended.</param>
    public static void AddMermaidCodeBlockStart(StringBuilder diagramBuilder)
    {
        diagramBuilder.AppendLine("```mermaid");
    }

    /// <summary>
    /// Appends the closing fence for a Mermaid diagram.
    /// </summary>
    /// <param name="diagramBuilder">The <see cref="StringBuilder"/> to which the footer is appended.</param>
    public static void AddCodeBlockEnd(StringBuilder diagramBuilder)
    {
        diagramBuilder.Append("```");
    }

    /// <summary>
    /// Appends the graph declaration for a Mermaid diagram.
    /// </summary>
    /// <param name="diagramBuilder">The <see cref="StringBuilder"/> to which the graph declaration is appended.</param>
    public static void AddGraphDeclaration(StringBuilder diagramBuilder)
    {
        diagramBuilder.AppendLine("graph");
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
    /// Appends project names as Mermaid nodes to the diagram.
    /// If no projects are found, a warning message is displayed in the console.
    /// </summary>
    public static void AddProjectNames(IEnumerable<CsprojModel> projectFiles, StringBuilder diagramBuilder)
    {
        foreach (var projectFile in projectFiles)
        {
            diagramBuilder.AppendLine($"    {projectFile.Name}");
        }
    }

    public static void AddClickableLinks(IEnumerable<CsprojModel> projectFiles, StringBuilder diagram, Settings settings)
    {
        if (!settings.IncludeUrls || string.IsNullOrEmpty(settings.BaseUrl))
            return;

        foreach (var project in projectFiles)
        {
            string filePath = Path.GetRelativePath(Directory.GetCurrentDirectory(), project.FullPath)
                                 .Replace("\\", "/");
            string formattedUrl =
                settings.UrlPattern
                    .Replace("{baseUrl}", settings.BaseUrl)
                    .Replace("{FilePath}", filePath)
                    .Replace("{ProjectName}", project.Name);

            diagram.AppendLine($"    click {project.Name} \"{formattedUrl}\"");
        }
    }

    /// <summary>
    /// Constructs Mermaid diagram arrows to represent project dependencies.
    /// Bi-directional dependencies are detected and represented with `<-->`, ensuring each relationship appears only once.
    /// </summary>
    /// <param name="projectFiles">The list of <see cref="CsprojModel"/> to process.</param>
    /// <param name="diagramBuilder">The <see cref="StringBuilder"/> used to construct the diagram.</param>
    public static void AddProjectDependencies(IEnumerable<CsprojModel> projectFiles, StringBuilder diagramBuilder)
    {
        var dependencyMap = projectFiles.ToDictionary(p => p.Name, p => p.CsprojDependencies.ToHashSet());
        var processedPairs = new HashSet<string>();

        foreach (var projectFile in projectFiles)
        {
            foreach (var dependency in projectFile.CsprojDependencies)
            {
                string pairKey = $"{projectFile.Name}-{dependency}";
                string reversePairKey = $"{dependency}-{projectFile.Name}";

                if (dependencyMap.TryGetValue(dependency, out HashSet<string>? value) && value.Contains(projectFile.Name))
                {
                    if (!processedPairs.Contains(reversePairKey))
                    {
                        diagramBuilder.AppendLine($"    {projectFile.Name} <--> {dependency}");
                        processedPairs.Add(pairKey);
                    }
                }
                else
                {
                    diagramBuilder.AppendLine($"    {projectFile.Name} --> {dependency}");
                }
            }
        }
    }
}
