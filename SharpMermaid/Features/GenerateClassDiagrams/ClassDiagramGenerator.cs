using SharpMermaid.Models;
using System.Text;

namespace SharpMermaid.Features.GenerateClassDiagrams;
public class ClassDiagramGenerator(string solutionFullPath)
{
    private readonly SlnModel _solution = new(solutionFullPath);
    public Dictionary<string, string> GenerateClassDiagrams()
    {
        return _solution.Csprojs.ToDictionary(
            csproj => csproj.Name,
            csproj =>
            {
                var diagram = new StringBuilder();

                Rules.AddMermaidBlockStart(diagram);
                Rules.AddCsProjectAsTitle(csproj.Name, diagram);
                Rules.AddClassDeclaration(diagram);
                Rules.AddCsFileNames(csproj.CsFiles, diagram);
                //Rules.AddClickableLinks(csproj, diagram);
                Rules.AddDiagramFooter(diagram);

                return diagram.ToString();
            }
        );
    }
}
