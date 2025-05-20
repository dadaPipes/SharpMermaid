using System.Text;
using Xunit.Abstractions;
using SharpMermaid.TestHelpers;
using SharpMermaid.MermaidGeneratorHelpers;

namespace SharpMermaid.UnitTests;
public class MermaidGeneratorClassHelpersTests(ITestOutputHelper output)
{
    private readonly ITestOutputHelper _output = output;

    [Fact]
    public void Should_Add_ClassDiagram_Declaration()
    {
        // Arrange:
        var diagram = new StringBuilder();

        // Act:
        MermaidGeneratorClassHelpers.AddClassDeclaration(diagram);

        // Assert:
        string expected =
        $"""
        classDiagram

        """;

        string actual = diagram.ToString();
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.Equal(expected, diagram.ToString());
    }

    [Fact]
    public void Should_Add_csProjName_As_Title()
    {
        // Arrange:
        using var solution = new TemporarySolutionBuilder();
        solution.AddProject("ProjectA");

        var diagram = new StringBuilder();

        // Act:
        MermaidGeneratorClassHelpers.AddCsProjectAsTitle("ProjectA", diagram);

        // Assert:
        string expected =
        $"""
        ---
        title: ProjectA
        ---

        """;

        string actual = diagram.ToString();
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.Equal(expected, diagram.ToString());
    }

    [Fact]
    public void Should_Add_CsFileNames()
    {
        // Arrange:
        using var solution = new TemporarySolutionBuilder();
        var projectA = solution.AddProjectWithFiles("ProjectA", new Dictionary<string, string> { ["X.cs"] = "public class X" });

        var projectFiles = new List<CsprojModel>
        {
            new ("ProjectA", Path.GetFullPath(projectA), Path.GetRelativePath(solution.Directory, projectA))
        };

        var csFiles = projectFiles.SelectMany(p => p.CsFiles).ToList();

        var diagram = new StringBuilder();

        // Act:
        MermaidGeneratorClassHelpers.AddCsFileNames(csFiles, diagram);

        // Assert:
        string expected =
        $"""
            class X

        """;

        string actual = diagram.ToString();
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.Equal(expected, diagram.ToString());
    }

    [Fact]
    public void ShouldAddClickableLinks()
    {
        // Arrange:
        using var solution = new TemporarySolutionBuilder();

        var projectA = solution.AddProjectWithFiles("ProjectA", new Dictionary<string, string>
        {
            ["X.cs"] = "public class X",
            ["FolderOne/Y.cs"] = "public class Y",
            ["FolderOne/FolderTwo/Z.cs"] = "public class Z"
        });

        CsprojModel csProj = new (
        "ProjectA",
        Path.GetFullPath(projectA),
        Path.GetRelativePath(solution.Directory, projectA)
        );

        Settings.BaseUrl = "https://example.com/";

        var diagram = new StringBuilder();

        // Act: 
        MermaidGeneratorClassHelpers.AddClickableLinks(csProj, diagram);

        string expected =
        """ 
            click X href "https://example.com/ProjectA/X.cs"
            click Y href "https://example.com/ProjectA/FolderOne/Y.cs"
            click Z href "https://example.com/ProjectA/FolderOne/FolderTwo/Z.cs"

        """;

        string actual = diagram.ToString();
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.Equal(expected, diagram.ToString());
    }
}
