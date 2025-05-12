using System.Text;

namespace SharpMermaid;
public class MermaidGenerator(string solutionFullPath)
{
    public readonly SlnModel _solution = new(solutionFullPath);

    public string PhysicalProjectDiagram()
    {
        var diagramBuilder = new StringBuilder();

        MermaidGeneratorHelpers.AddMermaidBlockStart(diagramBuilder);

        MermaidGeneratorHelpers.AddSolutionNameAsTitle(_solution.Name, diagramBuilder);

        MermaidGeneratorHelpers.AddGraphDeclaration(diagramBuilder);

        if (!_solution.HasProjects)
        {
            MermaidGeneratorHelpers.AddDiagramFooter(diagramBuilder);
            return diagramBuilder.ToString();
        }

        MermaidGeneratorHelpers.AddProjectNames(_solution.Csprojs, diagramBuilder);

        MermaidGeneratorHelpers.AddClickableLinks(_solution.Csprojs, diagramBuilder);

        MermaidGeneratorHelpers.AddProjectDependencies(_solution.Csprojs, diagramBuilder);

        MermaidGeneratorHelpers.AddDiagramFooter(diagramBuilder);

        return diagramBuilder.ToString();
    }

    public string LogicalProjectDiagram()
    {
        var diagramBuilder = new StringBuilder();

        MermaidGeneratorHelpers.AddMermaidBlockStart(diagramBuilder);

        MermaidGeneratorHelpers.AddSolutionNameAsTitle(_solution.Name, diagramBuilder);

        MermaidGeneratorHelpers.AddGraphDeclaration(diagramBuilder);

        if (!_solution.HasProjects) // should write to the console
        {
            MermaidGeneratorHelpers.AddDiagramFooter(diagramBuilder);
            return diagramBuilder.ToString();
        }

        MermaidGeneratorHelpers.AddProjectHierarchy(_solution, _solution.Csprojs, diagramBuilder);

        MermaidGeneratorHelpers.AddClickableLinks(_solution.Csprojs, diagramBuilder);

        MermaidGeneratorHelpers.AddProjectDependencies(_solution.Csprojs, diagramBuilder);

        MermaidGeneratorHelpers.AddDiagramFooter(diagramBuilder);

        return diagramBuilder.ToString();
    }
}