using SharpMermaid.Models;
using System.Text;

namespace SharpMermaid.Features.GenerateLogicalProjectDiagram;
public class LogicalProjectDiagramGenerator(string solutionFullPath)
{
    private readonly SlnModel _solution = new(solutionFullPath);
    public string GenerateLogicalProjectDiagram()
    {
        var diagramBuilder = new StringBuilder();

        Rules.AddMermaidBlockStart(diagramBuilder);
        Rules.AddSolutionNameAsTitle(_solution.Name, diagramBuilder);
        Rules.AddGraphDeclaration(diagramBuilder);

        if (!_solution.HasProjects) // should write to the console
        {
            Rules.AddDiagramFooter(diagramBuilder);
            return diagramBuilder.ToString();
        }

        Rules.AddProjectHierarchy(_solution.Csprojs, diagramBuilder);
        //Rules.AddClickableLinks(_solution.Csprojs, diagramBuilder);
        Rules.AddProjectDependencies(_solution.Csprojs, diagramBuilder);
        Rules.AddDiagramFooter(diagramBuilder);

        return diagramBuilder.ToString();
    }
}
