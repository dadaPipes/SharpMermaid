using SharpMermaid.Models;
using System.Text;

namespace SharpMermaid.Features.GeneratePhysicalProjectDiagram;
public class PhysicalProjectDiagramGenerator(string solutionFullPath, Settings settings)
{
    private readonly SlnModel _solution = new(solutionFullPath);
    private readonly Settings _settings = settings;

    public string GeneratePhysicalProjectDiagram()
    {
        var diagramBuilder = new StringBuilder();

        Rules.AddMermaidCodeBlockStart(diagramBuilder);
        Rules.AddSolutionNameAsTitle(_solution.Name, diagramBuilder);
        Rules.AddGraphDeclaration(diagramBuilder);

        if (!_solution.HasProjects)
        {
            Rules.AddCodeBlockEnd(diagramBuilder);
            return diagramBuilder.ToString();
        }

        Rules.AddProjectNames(_solution.Csprojs, diagramBuilder);
        Rules.AddClickableLinks(_solution.Csprojs, diagramBuilder, _settings);
        Rules.AddProjectDependencies(_solution.Csprojs, diagramBuilder);
        Rules.AddCodeBlockEnd(diagramBuilder);

        return diagramBuilder.ToString();
    }
}
