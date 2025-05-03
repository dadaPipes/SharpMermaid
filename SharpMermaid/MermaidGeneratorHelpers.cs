using System.Text;
using System.Xml.Linq;

namespace SharpMermaid;
static class MermaidGeneratorHelpers
{
    public static void AddProjectDiagramHeader(StringBuilder diagramBuilder)
    {
        diagramBuilder.AppendLine("```mermaid");
        diagramBuilder.AppendLine("graph TD");
    }

    public static void AddDiagramFooter(StringBuilder diagramBuilder)
    {
        diagramBuilder.Append("```");
    }
    public static void AddProjectNames(List<string> projectFiles, StringBuilder diagramBuilder)
    {
        foreach (var project in projectFiles.ConvertAll(p => Path.GetFileNameWithoutExtension(p)))
        {
            diagramBuilder.AppendLine($"    {project}");
        }
    }

    public static List<string> GetProjectFiles(string solutionPath)
    {
        return Directory.GetFiles(solutionPath, "*.csproj", SearchOption.AllDirectories).ToList();
    }
    
    public static void GenerateProjectNodes(List<string> projectNames, StringBuilder diagramBuilder)
    {
        foreach (var project in projectNames)
        {
            diagramBuilder.AppendLine($"    {project}");
        }
    }
    public static void AddProjectDependencies(Dictionary<string, List<string>> dependencies, StringBuilder diagramBuilder)
    {
        foreach (var project in dependencies.Keys)
        {
            foreach (var dependency in dependencies[project])
            {
                diagramBuilder.AppendLine($"    {project} --> {dependency}");
            }
        }
    }
    public static string GenerateDiagramFooter()
    {
        return "```";
    }

    public static Dictionary<string, List<string>> ExtractProjectDependencies(List<string> projectFiles)
    {
        var dependencies = new Dictionary<string, List<string>>();

        foreach (var projectFile in projectFiles)
        {
            var projectName = Path.GetFileNameWithoutExtension(projectFile);
            var referencedProjects = new List<string>();

            try
            {
                var xdoc = XDocument.Load(projectFile);

                var projectReferences = xdoc
                    .Descendants("ProjectReference")
                    .Select(elem => elem.Attribute("Include")?.Value)
                    .Where(path => !string.IsNullOrWhiteSpace(path))
                    .Select(path => Path.GetFileNameWithoutExtension(path!))
                    .ToList();

                referencedProjects.AddRange(projectReferences);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to parse {projectFile}: {ex.Message}");
            }

            dependencies[projectName] = referencedProjects;
        }

        return dependencies;
    }

    public static bool ProjectHasSourceFiles(string projectFilePath)
    {
        string projectDir = Path.GetDirectoryName(projectFilePath)!;
        return Directory.EnumerateFiles(projectDir, "*.cs", SearchOption.AllDirectories)
                        .Any();
    }

    public static string GetRelativePath(string projectFilePath)
    {
        string projectDir = Path.GetDirectoryName(projectFilePath);
        string solutionDir = Path.GetDirectoryName(projectDir);
        return Path.GetRelativePath(solutionDir, projectFilePath);
    }
    public static void AddClickableLinks(List<string> projectFiles, StringBuilder diagramBuilder, string relativePath)
    {
        foreach (var file in projectFiles)
        {
            var name = Path.GetFileNameWithoutExtension(file);

            diagramBuilder.AppendLine($"    click {name} \"{relativePath}\"");
        }
    }
}
