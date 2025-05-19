using System.Text;
using System.Xml.Linq;

namespace SharpMermaid.MermaidGeneratorHelpers;

/// <summary>
/// Provides helper methods for generating Mermaid class diagrams
/// from .NET types, including classes, interfaces, and relationships.
/// </summary>
static class MermaidGeneratorClassHelpers
{
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
    /*
    public static void AddClickableLinks(CsprojModel csproj , StringBuilder diagramBuilder)
    {
        var baseUrl = Settings.BaseUrl.TrimEnd('/');
        var relativePath = csproj.RelativePathFromSlnWithoutExtension.Replace('\\', '/');
        var fullUrl = $"{baseUrl}/{relativePath}";

        foreach(var csFile in csproj.CsFiles)
        {
            diagramBuilder.AppendLine($"    click {csFile.Name} href \"{fullUrl}/{csFile.NameWithFileExtension}\"");
        }
    }
    */
    public static void AddClickableLinks(List<CsprojModel> csprojList, StringBuilder diagramBuilder)
    {
        var baseUrl = Settings.BaseUrl.TrimEnd('/');

        foreach (var csproj in csprojList)
        {
            var relativePath = csproj.RelativePathFromSlnWithoutExtension.Replace('\\', '/');
            var fullUrl = $"{baseUrl}/{relativePath}";

            foreach (var csFile in csproj.CsFiles)
            {
                diagramBuilder.AppendLine($"    click {csFile.Name} href \"{fullUrl}/{csFile.NameWithFileExtension}\"");
            }
        }
    }
}