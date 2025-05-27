using SharpMermaid.Models;
using System.Text;

namespace SharpMermaid.Features.GenerateLogicalProjectDiagram;
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
    /// Appends the graph declaration for a Mermaid diagram.
    /// </summary>
    /// <param name="diagramBuilder">The <see cref="StringBuilder"/> to which the graph declaration is appended.</param>
    public static void AddGraphDeclaration(StringBuilder diagramBuilder)
    {
        diagramBuilder.AppendLine("graph");
    }

    /// <summary>
    /// Constructs a hierarchical representation of projects and folders
    /// within a solution and appends it to the Mermaid diagram.
    /// </summary>
    /// <param name="csprojModels">The list of projects in the solution.</param>
    /// <param name="diagramBuilder">The <see cref="StringBuilder"/> used to construct the diagram.</param>
    public static void AddProjectHierarchy(IEnumerable<CsprojModel> csprojModels, StringBuilder diagramBuilder)
    {
        // Build a tree where the root represents projects with no folder,
        // and each FolderNode represents a folder with possible subfolders.
        var root = new FolderNode { FolderName = "" };

        foreach (var proj in csprojModels)
        {
            // Split the relative path by both kinds of directory separators
            // (so it handles both '\' and '/').
            var tokens = proj.RelativePathFromSln
                             .Split([Path.DirectorySeparatorChar,
                                     Path.AltDirectorySeparatorChar],
                                     StringSplitOptions.RemoveEmptyEntries);

            // two tokens(folder and csproj file)
            if (tokens.Length == 2)
            {
                // No folder info – top-level project
                root.Projects.Add(proj.Name);
            }
            else
            {
                // The last token is the actual project name,
                // while the earlier tokens define the folder structure.
                var currentNode = root;
                for (int i = 0; i < tokens.Length - 2; i++) // Note: tokens.Length - 2
                {
                    string folder = tokens[i];
                    var child = currentNode.SubFolders.FirstOrDefault(f => f.FolderName == folder);
                    if (child is null)
                    {
                        child = new FolderNode { FolderName = folder };
                        currentNode.SubFolders.Add(child);
                    }
                    currentNode = child;
                }

                string folderName = tokens[^2];
                string projectName = Path.GetFileNameWithoutExtension(tokens[^1]);

                if (folderName == projectName)
                {
                    // project is in a folder named like the project – treat as "no folder"
                    currentNode.Projects.Add(projectName);
                }
                else
                {
                    // add it to a folder node (create if needed)
                    var child = currentNode.SubFolders.FirstOrDefault(f => f.FolderName == folderName);
                    if (child == null)
                    {
                        child = new FolderNode { FolderName = folderName };
                        currentNode.SubFolders.Add(child);
                    }
                    child.Projects.Add(projectName);
                }

            }
        }

        // Output top-level projects
        foreach (var proj in root.Projects)
        {
            diagramBuilder.AppendLine($"    {proj}");
        }

        // Recursively output subgraph information from the tree.
        foreach (var folder in root.SubFolders.OrderBy(f => f.FolderName))
        {
            AppendFolderNode(folder, diagramBuilder, indentLevel: 1);
        }
    }

    /// <summary>
    /// Recursively appends a folder node and its subfolders/projects
    /// to the Mermaid diagram.
    /// </summary>
    /// <param name="node">The folder node to append.</param>
    /// <param name="diagramBuilder">The <see cref="StringBuilder"/> used to build the diagram.</param>
    /// <param name="indentLevel">The indentation level for nested folders.</param>
    private static void AppendFolderNode(FolderNode node, StringBuilder diagramBuilder, int indentLevel)
    {
        string indent = new string(' ', indentLevel * 4);
        diagramBuilder.AppendLine($"{indent}subgraph {node.FolderName}");

        // Output projects in this folder
        foreach (var project in node.Projects)
        {
            diagramBuilder.AppendLine($"{indent}    {project}");
        }

        // Recursively output subfolders if any
        foreach (var subfolder in node.SubFolders.OrderBy(f => f.FolderName))
        {
            AppendFolderNode(subfolder, diagramBuilder, indentLevel + 1);
        }

        diagramBuilder.AppendLine($"{indent}end");
    }

    /// <summary>
    /// Represents a folder node containing subfolders and projects
    /// in a hierarchical project structure.
    /// </summary>
    private sealed class FolderNode
    {
        public string FolderName { get; set; }
        public List<FolderNode> SubFolders { get; set; } = new List<FolderNode>();
        public List<string> Projects { get; set; } = new List<string>();
    }

    /// <summary>
    /// Adds clickable Mermaid diagram links for each project that contains source files.
    /// Projects without source files are ignored.
    /// </summary>
    /// <param name="projectFiles">The list of <see cref="CsprojModel"/> to process.</param>
    /// <param name="diagramBuilder">The <see cref="StringBuilder"/> used to build the diagram.</param>
    public static void AddClickableLinks(IEnumerable<CsprojModel> projectFiles, StringBuilder diagramBuilder)
    {
        foreach (var project in projectFiles.Where(p => p.HasCsFiles))
        {
            var baseUrl = Settings.BaseUrl.TrimEnd('/');
            var relativePath = project.RelativePathFromSln.Replace('\\', '/');
            var fullUrl = $"{baseUrl}/{relativePath}";

            diagramBuilder.AppendLine($"    click {project.Name} \"{fullUrl}\"");
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
