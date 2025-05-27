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

    /// <summary>
    /// Adds clickable Mermaid diagram links for each project that contains source files.
    /// If a project lacks source files, no link is added, and a warning message is returned.
    /// </summary>
    /// <param name="projectFiles">The list of <see cref="CsprojModel"/> representing projects to process.</param>
    /// <param name="diagramBuilder">The <see cref="StringBuilder"/> used to build the diagram.</param>
    /// <returns>
    /// A list of warning messages for projects that do not contain source files.
    /// </returns>
    public static List<string> AddClickableLinks(IEnumerable<CsprojModel> projectFiles, StringBuilder diagramBuilder)
    {
        foreach (var project in projectFiles.Where(p => p.HasCsFiles))
        {
            diagramBuilder.AppendLine($"    click {project.Name} \"{project.ClassDiagramUrl}\"");
        }

        List<string> warningMessages = [];

        foreach (var project in projectFiles.Where(p => !p.HasCsFiles))
        {
            var warning = $"Warning: Project '{project.Name}' does not contain any source files and will not include a class diagram url.";
            warningMessages.Add(warning);
        }

        return warningMessages;
    }

    /// <summary>
    /// Constructs Mermaid diagram arrows to represent project dependencies.
    /// Bi-directional dependencies are detected and represented with `<-->`, ensuring each relationship appears only once.
    /// </summary>
    /// <param name="projectFiles">The list of <see cref="CsprojModel"/> to process.</param>
    /// <param name="diagramBuilder">The <see cref="StringBuilder"/> used to construct the diagram.</param>
    public static string? AddProjectDependencies(IEnumerable<CsprojModel> projectFiles, StringBuilder diagramBuilder)
    {
        var dependencyMap = projectFiles.ToDictionary(p => p.Name, p => p.CsprojDependencies.ToHashSet());
        var processedPairs = new HashSet<string>();
        string? warningMessage = null;

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

                        // Assign warning message instead of appending to StringBuilder
                        warningMessage = $"Warning: Bi-directional dependency detected between {projectFile.Name} and {dependency}.";
                    }
                }
                else
                {
                    diagramBuilder.AppendLine($"    {projectFile.Name} --> {dependency}");
                }
            }
        }
        return warningMessage;
    }
}
