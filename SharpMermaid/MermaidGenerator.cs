using System.Text;

namespace SharpMermaid;
public static class MermaidGenerator
{
    public static string PhysicalProjectDiagram(string solutionPath)
    {
        var diagramBuilder = new StringBuilder();
        
        MermaidGeneratorHelpers.AddProjectDiagramHeader(diagramBuilder);
        
        var projectFiles = MermaidGeneratorHelpers.GetProjectFiles(solutionPath);
        if (projectFiles.Count is 0)
        {
            MermaidGeneratorHelpers.AddDiagramFooter(diagramBuilder);
            return diagramBuilder.ToString();
        }

        MermaidGeneratorHelpers.AddProjectNames(projectFiles, diagramBuilder);

        string relativePath = "dummy";
        if (projectFiles.Any(MermaidGeneratorHelpers.ProjectHasSourceFiles))
        {
            MermaidGeneratorHelpers.AddClickableLinks(projectFiles, diagramBuilder, relativePath);
        }

        var dependencies = MermaidGeneratorHelpers.ExtractProjectDependencies(projectFiles);
        MermaidGeneratorHelpers.AddProjectDependencies(dependencies, diagramBuilder);

        MermaidGeneratorHelpers.AddDiagramFooter(diagramBuilder);

        return diagramBuilder.ToString();
    }
}