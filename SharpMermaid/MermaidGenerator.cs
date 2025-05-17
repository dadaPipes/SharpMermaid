using SharpMermaid.DiagramGeneratorHelpers;
using System.Text;

namespace SharpMermaid;
public class MermaidGenerator(string solutionFullPath)
{
    internal readonly SlnModel _solution = new(solutionFullPath);

    public string PhysicalProjectDiagram()
    {
        var diagramBuilder = new StringBuilder();

        CommonDiagramGeneratorHelpers.AddMermaidBlockStart(diagramBuilder);

        CommonDiagramGeneratorHelpers.AddSolutionNameAsTitle(_solution.Name, diagramBuilder);

        ProjectDiagramGeneratorHelpers.AddGraphDeclaration(diagramBuilder);

        if (!_solution.HasProjects)
        {
            CommonDiagramGeneratorHelpers.AddDiagramFooter(diagramBuilder);
            return diagramBuilder.ToString();
        }

        ProjectDiagramGeneratorHelpers.AddProjectNames(_solution.Csprojs, diagramBuilder);

        ProjectDiagramGeneratorHelpers.AddClickableLinks(_solution.Csprojs, diagramBuilder);

        ProjectDiagramGeneratorHelpers.AddProjectDependencies(_solution.Csprojs, diagramBuilder);

        CommonDiagramGeneratorHelpers.AddDiagramFooter(diagramBuilder);

        return diagramBuilder.ToString();
    }

    public string LogicalProjectDiagram()
    {
        var diagramBuilder = new StringBuilder();

        CommonDiagramGeneratorHelpers.AddMermaidBlockStart(diagramBuilder);

        CommonDiagramGeneratorHelpers.AddSolutionNameAsTitle(_solution.Name, diagramBuilder);

        ProjectDiagramGeneratorHelpers.AddGraphDeclaration(diagramBuilder);

        if (!_solution.HasProjects) // should write to the console
        {
            CommonDiagramGeneratorHelpers.AddDiagramFooter(diagramBuilder);
            return diagramBuilder.ToString();
        }

        ProjectDiagramGeneratorHelpers.AddProjectHierarchy(_solution, _solution.Csprojs, diagramBuilder);

        ProjectDiagramGeneratorHelpers.AddClickableLinks(_solution.Csprojs, diagramBuilder);

        ProjectDiagramGeneratorHelpers.AddProjectDependencies(_solution.Csprojs, diagramBuilder);

        CommonDiagramGeneratorHelpers.AddDiagramFooter(diagramBuilder);

        return diagramBuilder.ToString();
    }

    public string ClassDiagram()
    {
        var diagramBuilder = new StringBuilder();

        CommonDiagramGeneratorHelpers.AddMermaidBlockStart(diagramBuilder);
        ClassDiagramGeneratorHelpers.AddClassDeclaration(diagramBuilder);

        // MermaidGeneratorHelpers.GenerateClassHierarchy();

        return diagramBuilder.ToString();
    }
}