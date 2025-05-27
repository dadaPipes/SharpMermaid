using SharpMermaid.Models;
using System.Text;

namespace SharpMermaid.Features.GeneratePhysicalProjectDiagram;
public class PhysicalProjectDiagramGenerator(string solutionFullPath)
{
    private readonly SlnModel _solution = new(solutionFullPath);

    public string GeneratePhysicalProjectDiagram()
    {
        var diagramBuilder = new StringBuilder();

        Rules.AddMermaidBlockStart(diagramBuilder);
        Rules.AddSolutionNameAsTitle(_solution.Name, diagramBuilder);
        Rules.AddGraphDeclaration(diagramBuilder);

        if (!_solution.HasProjects)
        {
            Rules.AddDiagramFooter(diagramBuilder);
            return diagramBuilder.ToString();
        }

        Rules.AddProjectNames(_solution.Csprojs, diagramBuilder);
        Rules.AddClickableLinks(_solution.Csprojs, diagramBuilder);
        Rules.AddProjectDependencies(_solution.Csprojs, diagramBuilder);
        Rules.AddDiagramFooter(diagramBuilder);

        return diagramBuilder.ToString();
    }
}
