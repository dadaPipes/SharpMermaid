using SharpMermaid.MermaidGeneratorHelpers;
using System.Text;

namespace SharpMermaid;
public class MermaidGenerator(string solutionFullPath)
{
    internal readonly SlnModel _solution = new(solutionFullPath);

    public string PhysicalProjectDiagram()
    {
        var diagramBuilder = new StringBuilder();

        MermaidGeneratorCommonHelpers.AddMermaidBlockStart(diagramBuilder);

        MermaidGeneratorCommonHelpers.AddSolutionNameAsTitle(_solution.Name, diagramBuilder);

        MermaidGeneratorProjectHelpers.AddGraphDeclaration(diagramBuilder);

        if (!_solution.HasProjects)
        {
            MermaidGeneratorCommonHelpers.AddDiagramFooter(diagramBuilder);
            return diagramBuilder.ToString();
        }

        MermaidGeneratorProjectHelpers.AddProjectNames(_solution.Csprojs, diagramBuilder);

        MermaidGeneratorProjectHelpers.AddClickableLinks(_solution.Csprojs, diagramBuilder);

        MermaidGeneratorProjectHelpers.AddProjectDependencies(_solution.Csprojs, diagramBuilder);

        MermaidGeneratorCommonHelpers.AddDiagramFooter(diagramBuilder);

        return diagramBuilder.ToString();
    }

    public string LogicalProjectDiagram()
    {
        var diagramBuilder = new StringBuilder();

        MermaidGeneratorCommonHelpers.AddMermaidBlockStart(diagramBuilder);

        MermaidGeneratorCommonHelpers.AddSolutionNameAsTitle(_solution.Name, diagramBuilder);

        MermaidGeneratorProjectHelpers.AddGraphDeclaration(diagramBuilder);

        if (!_solution.HasProjects) // should write to the console
        {
            MermaidGeneratorCommonHelpers.AddDiagramFooter(diagramBuilder);
            return diagramBuilder.ToString();
        }

        MermaidGeneratorProjectHelpers.AddProjectHierarchy(_solution, _solution.Csprojs, diagramBuilder);

        MermaidGeneratorProjectHelpers.AddClickableLinks(_solution.Csprojs, diagramBuilder);

        MermaidGeneratorProjectHelpers.AddProjectDependencies(_solution.Csprojs, diagramBuilder);

        MermaidGeneratorCommonHelpers.AddDiagramFooter(diagramBuilder);

        return diagramBuilder.ToString();
    }

    public List<string> ClassDiagrams()
    {
        return _solution.Csprojs
            .ConvertAll(csproj =>
            {
                var diagram = new StringBuilder();
                MermaidGeneratorCommonHelpers.AddMermaidBlockStart(diagram);
                MermaidGeneratorClassHelpers.AddCsProjectAsTitle(csproj.Name, diagram);
                MermaidGeneratorClassHelpers.AddClassDeclaration(diagram);
                MermaidGeneratorClassHelpers.AddCsFileNames(csproj.CsFiles, diagram);
                MermaidGeneratorClassHelpers.AddClickableLinks(_solution.Csprojs, diagram);

                return diagram.ToString();
            });
    }
}


// MermaidGeneratorHelpers.GenerateClassHierarchy();