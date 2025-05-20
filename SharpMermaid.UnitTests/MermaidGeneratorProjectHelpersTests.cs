using SharpMermaid.MermaidGeneratorHelpers;
using SharpMermaid.TestHelpers;
using System.Text;
using Xunit.Abstractions;

namespace SharpMermaid.UnitTests;
public class MermaidGeneratorProjectHelpersTests(ITestOutputHelper output)
{
    private readonly ITestOutputHelper _output = output;

    [Fact]
    public void ShouldAddGraphDeclaration()
    {
        // Arrange:
        var diagram = new StringBuilder();

        // Act:
        MermaidGeneratorProjectHelpers.AddGraphDeclaration(diagram);

        // Assert:
        string expected =
        $"""
        graph

        """;

        string actual = diagram.ToString();
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.Equal(expected, diagram.ToString());
    }

    [Fact]
    public void ShouldAppendProjectNamesToDiagram()
    {
        // Arrange:
        using var solution = new TemporarySolutionBuilder();

        var projectA = solution.AddProject("ProjectA");
        var projectB = solution.AddProject("ProjectB");
        var projectC = solution.AddProject("ProjectC");

        // Load projects into CsprojModel instances
        List<CsprojModel> projectFiles = [
            new ("ProjectA", Path.GetFullPath(projectA), Path.GetRelativePath(solution.Directory, projectA)),
            new ("ProjectB", Path.GetFullPath(projectB), Path.GetRelativePath(solution.Directory, projectB)),
            new ("ProjectC", Path.GetFullPath(projectC), Path.GetRelativePath(solution.Directory, projectC))
            ];

        var diagram = new StringBuilder();

        // Act:
        MermaidGeneratorProjectHelpers.AddProjectNames(projectFiles, diagram);

        // Assert: Verify that the output matches the expected format
        string expected =
        """
            ProjectA
            ProjectB
            ProjectC

        """;

        string actual = diagram.ToString();
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.Equal(expected, diagram.ToString());
    }

    [Fact]
    public void ShouldAppendProjectDependenciesToDiagram()
    {
        // Arrange:
        using var solution = new TemporarySolutionBuilder();

        var projectA = solution.AddProject("ProjectA");
        var projectB = solution.AddProject("ProjectB");
        var projectC = solution.AddProject("ProjectC");
        var projectD = solution.AddProject("ProjectD");

        solution.AddProjectReference(projectA, projectB);
        solution.AddProjectReference(projectA, projectC);
        solution.AddProjectReference(projectB, projectD);

        List<CsprojModel> projectFiles = [
            new ("ProjectA", Path.GetFullPath(projectA), Path.GetRelativePath(solution.Directory, projectA)),
            new ("ProjectB", Path.GetFullPath(projectB), Path.GetRelativePath(solution.Directory, projectB)),
            new ("ProjectC", Path.GetFullPath(projectC), Path.GetRelativePath(solution.Directory, projectC)),
            new ("ProjectD", Path.GetFullPath(projectD), Path.GetRelativePath(solution.Directory, projectD))
            ];

        var diagram = new StringBuilder();

        // Act:
        MermaidGeneratorProjectHelpers.AddProjectDependencies(projectFiles, diagram);

        // Assert:
        string expected =
        """
            ProjectA --> ProjectB
            ProjectA --> ProjectC
            ProjectB --> ProjectD

        """;

        string actual = diagram.ToString();
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.Equal(expected, diagram.ToString());
    }

    [Fact]
    public void ShouldAppendClickableLinksToDiagram()
    {
        // Arrange:
        using var solution = new TemporarySolutionBuilder();

        var projectA = solution.AddProjectWithFiles("ProjectA", new Dictionary<string, string> { ["X.cs"] = "public class MyClass" });
        var projectB = solution.AddProjectWithFiles("ProjectB", new Dictionary<string, string> { ["Y.cs"] = "public class YourClass" });

        List<CsprojModel> projectFiles = [
            new ("ProjectA", Path.GetFullPath(projectA), Path.GetRelativePath(solution.Directory, projectA)),
            new ("ProjectB", Path.GetFullPath(projectB), Path.GetRelativePath(solution.Directory, projectB))
            ];

        Settings.BaseUrl = "https://example.com/";

        var diagram = new StringBuilder();

        // Act:
        MermaidGeneratorProjectHelpers.AddClickableLinks(projectFiles, diagram);

        // Assert:
        string expected =
        """
            click ProjectA "https://example.com/ProjectA/ProjectA.csproj"
            click ProjectB "https://example.com/ProjectB/ProjectB.csproj"

        """;

        string actual = diagram.ToString();
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.Equal(expected, diagram.ToString());
    }

    [Fact]
    public void AddProjectHierarchy_ShouldAppendProjectHierarchyToDiagramWithOneProjectInTheSolutionRoot()
    {
        // Arrange:
        using var builder = new TemporarySolutionBuilder();
        var projectA = builder.AddProject("ProjectA");
        var projectB = builder.AddProject("Subfolder1", "ProjectB");
        var projectC = builder.AddProject("Subfolder1", "ProjectC");
        var projectD = builder.AddProject("Subfolder1/Subfolder2", "ProjectD");
        var projectE = builder.AddProject("Subfolder1/Subfolder2", "ProjectE");
        var projectX = builder.AddProject("Subfolder9", "ProjectX");
        var projectY = builder.AddProject("Subfolder9", "ProjectY");
        var projectZ = builder.AddProject("Subfolder9", "ProjectZ");

        var solution = new SlnModel(builder.FullPath);

        List<CsprojModel> projectFiles = [
            new ("ProjectA", Path.GetFullPath(projectA), Path.GetRelativePath(builder.Directory, projectA)),
            new ("ProjectB", Path.GetFullPath(projectB), Path.GetRelativePath(builder.Directory, projectB)),
            new ("ProjectC", Path.GetFullPath(projectC), Path.GetRelativePath(builder.Directory, projectC)),
            new ("ProjectD", Path.GetFullPath(projectD), Path.GetRelativePath(builder.Directory, projectD)),
            new ("ProjectE", Path.GetFullPath(projectE), Path.GetRelativePath(builder.Directory, projectE)),
            new ("ProjectX", Path.GetFullPath(projectX), Path.GetRelativePath(builder.Directory, projectX)),
            new ("ProjectY", Path.GetFullPath(projectY), Path.GetRelativePath(builder.Directory, projectY)),
            new ("ProjectZ", Path.GetFullPath(projectZ), Path.GetRelativePath(builder.Directory, projectZ))
            ];

        var diagram = new StringBuilder();

        // Act:
        MermaidGeneratorProjectHelpers.AddProjectHierarchy(solution, projectFiles, diagram);

        // Assert:
        string expected =
        $"""
            ProjectA
            subgraph Subfolder1
                ProjectB
                ProjectC
                subgraph Subfolder2
                    ProjectD
                    ProjectE
                end
            end
            subgraph Subfolder9
                ProjectX
                ProjectY
                ProjectZ
            end

        """;

        string actual = diagram.ToString();
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.Equal(expected, diagram.ToString());
    }

    [Fact]
    public void AddProjectHierarchy_ShouldAppendProjectHierarchyToDiagramWithMultipleProjectsInTheSolutionRoot()
    {
        // Arrange:
        using var builder = new TemporarySolutionBuilder();
        var projectA = builder.AddProject("ProjectA");
        var projectAb = builder.AddProject("ProjectAb");
        var projectB = builder.AddProject("Subfolder1", "ProjectB");
        var projectC = builder.AddProject("Subfolder1", "ProjectC");
        var projectD = builder.AddProject("Subfolder1/Subfolder2", "ProjectD");
        var projectE = builder.AddProject("Subfolder1/Subfolder2", "ProjectE");
        var projectX = builder.AddProject("Subfolder9", "ProjectX");
        var projectY = builder.AddProject("Subfolder9", "ProjectY");
        var projectZ = builder.AddProject("Subfolder9", "ProjectZ");

        var solution = new SlnModel(builder.FullPath);

        List<CsprojModel> projectFiles = [
            new ("ProjectA", Path.GetFullPath(projectA), Path.GetRelativePath(builder.Directory, projectA)),
            new ("ProjectAb", Path.GetFullPath(projectAb), Path.GetRelativePath(builder.Directory, projectAb)),
            new ("ProjectB", Path.GetFullPath(projectB), Path.GetRelativePath(builder.Directory, projectB)),
            new ("ProjectC", Path.GetFullPath(projectC), Path.GetRelativePath(builder.Directory, projectC)),
            new ("ProjectD", Path.GetFullPath(projectD), Path.GetRelativePath(builder.Directory, projectD)),
            new ("ProjectE", Path.GetFullPath(projectE), Path.GetRelativePath(builder.Directory, projectE)),
            new ("ProjectX", Path.GetFullPath(projectX), Path.GetRelativePath(builder.Directory, projectX)),
            new ("ProjectY", Path.GetFullPath(projectY), Path.GetRelativePath(builder.Directory, projectY)),
            new ("ProjectZ", Path.GetFullPath(projectZ), Path.GetRelativePath(builder.Directory, projectZ))
            ];

        var diagram = new StringBuilder();

        // Act:
        MermaidGeneratorProjectHelpers.AddProjectHierarchy(solution, projectFiles, diagram);

        // Assert:
        string expected =
        $"""
            ProjectA
            ProjectAb
            subgraph Subfolder1
                ProjectB
                ProjectC
                subgraph Subfolder2
                    ProjectD
                    ProjectE
                end
            end
            subgraph Subfolder9
                ProjectX
                ProjectY
                ProjectZ
            end

        """;

        string actual = diagram.ToString();
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.Equal(expected, diagram.ToString());
    }
}
