using SharpMermaid.MermaidGeneratorHelpers;
using System.Text;
using Xunit.Abstractions;

namespace SharpMermaid.Test.MermaidGeneratorHelpersTests;
public class MermaidGeneratorClassHelpersTests(ITestOutputHelper output)
{
    private readonly ITestOutputHelper _output = output;

    [Fact(DisplayName = "AddClassDeclaration(diagram)")]
    public void Should_Add_ClassDiagram_Declaration()
    {
        // Arrange: create an empty StringBuilder
        var diagram = new StringBuilder();

        // Act: add the classDiagram declaration
        MermaidGeneratorClassHelpers.AddClassDeclaration(diagram);

        // Assert: the output matches the expected Mermaid classDiagram format
        string expected =
        $"""
        classDiagram

        """;
        Assert.Equal(expected, diagram.ToString());
    }

    [Fact(DisplayName = "AddCsProjectAsTitle(\"projectA\", diagram)")]
    public void Should_Add_csProjName_As_Title()
    {
        // Arrange: create an empty StringBuilder
        using var solution = new TemporarySolutionBuilder();

        // Arrange: Create a temporary solution with projects
        var projectA = solution.AddProject("ProjectA");

        // Load projects into CsprojModel instances
        var projectFiles = new List<CsprojModel>
        {
            new ("ProjectA", Path.GetFullPath(projectA), Path.GetRelativePath(solution.Directory, projectA))
        };

        // Create an empty StringBuilder to store the diagram content
        var diagram = new StringBuilder();

        // Act: add the csProject name as title
        MermaidGeneratorClassHelpers.AddCsProjectAsTitle("ProjectA", diagram);

        // Assert: the output matches the expected Mermaid title block format and adds a new line
        string expected =
        $"""
        ---
        title: ProjectA
        ---

        """;

        Assert.Equal(expected, diagram.ToString());
    }

    [Fact(DisplayName = "AddCsFileNames(csFiles, diagram)")]
    public void Should_Add_CsFileNames()
    {
        // Arrange: Create a Solution with Projects
        using var builder = new TemporarySolutionBuilder();

        var projectA = builder.AddProjectWithFiles("ProjectA", new Dictionary<string, string> { ["X.cs"] = "public class X" });
        
        // Load projects with source files into CsprojModel instances
        var projectFiles = new List<CsprojModel>
        {
            new ("ProjectA", Path.GetFullPath(projectA), Path.GetRelativePath(builder.Directory, projectA))
        };

        var csFiles = projectFiles.SelectMany(p => p.CsFiles).ToList();

        // Create an empty StringBuilder to store the diagram content
        var diagram = new StringBuilder();

        // Act: add the csFile names to the diagram
        MermaidGeneratorClassHelpers.AddCsFileNames(csFiles, diagram);

        // Assert: the output matches the expected Mermaid class format for each file
        string expected =
        $"""
        class X

        """;

        Assert.Equal(expected, diagram.ToString());
    }

    [Fact(DisplayName = "AddClickableLinks(csproj ,diagram)")]
    public void ShouldAddClickableLinks()
    {
        // Arrange: Create a Solution with Projects
        using var builder = new TemporarySolutionBuilder();

        var projectA = builder.AddProjectWithFiles("ProjectA", new Dictionary<string, string>
        {
            ["X.cs"] = "public class X",
            ["Y.cs"] = "public class Y",
            ["Z.cs"] = "public class Z"
        });
        var projectB = builder.AddProjectWithFiles("ProjectB", new Dictionary<string, string>
        {
            ["S.cs"] = "public class S",
            ["D.cs"] = "public class D"
        });
        var projectC = builder.AddProjectWithFiles("ProjectC", new Dictionary<string, string>
        {
            ["J.cs"] = "public class J"
        });

        // Load projects with source files into CsprojModel instances
        var projectFiles = new List<CsprojModel>
        {
            new ("ProjectA", Path.GetFullPath(projectA), Path.GetRelativePath(builder.Directory, projectA)),
            new ("ProjectB", Path.GetFullPath(projectB), Path.GetRelativePath(builder.Directory, projectB)),
            new ("ProjectC", Path.GetFullPath(projectC), Path.GetRelativePath(builder.Directory, projectC))
        };

        var projA = projectFiles.First(projectFiles => projectFiles.Name == "ProjectA");

        // Mock the Settings.BaseUrl
        Settings.BaseUrl = "https://example.com/repo";

        // Create an empty StringBuilder to store the diagram content
        var diagram = new StringBuilder();

        // Act: 
        MermaidGeneratorClassHelpers.AddClickableLinks(projA, diagram);

        string expected =
        """ 
            click X href "https://example.com/repo/ProjectA/X.cs"
            click Y href "https://example.com/repo/ProjectB/Y.cs"
            click Z href "https://example.com/repo/ProjectB/Z.cs"

        """;

        // Log expected and actual values for debugging
        string actual = diagram.ToString();
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.Equal(expected, diagram.ToString());
    }
}
