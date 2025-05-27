using SharpMermaid.Features.GenerateLogicalProjectDiagram;
using SharpMermaid.Models;
using SharpMermaid.TestHelpers;
using System.Text;
using Xunit.Abstractions;

namespace SharpMermaid.RulesTests;
public class GenerateLogicalProjectDiagramTests(ITestOutputHelper output)
{
    private readonly ITestOutputHelper _output = output;

    // General Structure

    [Fact(DisplayName = "Diagram **must** have a title same as the solution name")]
    public void Should_Add_Solution_Name_As_Title()
    {
        // Arrange:
        var diagram = new StringBuilder();

        // Act:
        Rules.AddSolutionNameAsTitle("TestSolution", diagram);

        // Assert:
        string expected =
        $"""
        ---
        title: TestSolution
        ---

        """;

        string actual = diagram.ToString();
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.Equal(expected, diagram.ToString());
    }

    [Fact(DisplayName = "The diagram **must** include a node for each project in the solution")]
    public void Should_Append_Project_Names_To_Diagram()
    {
        // Arrange:
        var diagram = new StringBuilder();
        using var solution = new TemporarySolutionBuilder();

        var projectA = solution.AddProject("ProjectA");
        var projectB = solution.AddProject("ProjectB");
        var projectC = solution.AddProject("ProjectC");

        List<CsprojModel> projectFiles = [
            new ("ProjectA", Path.GetFullPath(projectA), Path.GetRelativePath(solution.Directory, projectA)),
            new ("ProjectB", Path.GetFullPath(projectB), Path.GetRelativePath(solution.Directory, projectB)),
            new ("ProjectC", Path.GetFullPath(projectC), Path.GetRelativePath(solution.Directory, projectC))
            ];

        // Act:
        Rules.AddProjectHierarchy(projectFiles, diagram);

        // Assert:
        string actual = diagram.ToString();
        Assert.Contains("ProjectA", actual);
        Assert.Contains("ProjectB", actual);
        Assert.Contains("ProjectC", actual);
    }

    [Fact(DisplayName = "Relationships must be indicated with usage arrows when dependencies exist between projects,  \r\n  including bi-directional dependencies")]
    public void Should_Add_Arrows_For_Project_Dependencies()
    {
        // Arrange:
        using var solution = new TemporarySolutionBuilder();

        // One-way dependencies
        var projectA = solution.AddProject("ProjectA");
        var projectB = solution.AddProject("ProjectB");
        var projectC = solution.AddProject("ProjectC");
        var projectD = solution.AddProject("ProjectD");

        solution.AddProjectReference(projectA, projectB);
        solution.AddProjectReference(projectA, projectC);
        solution.AddProjectReference(projectB, projectD);

        // Bi-directional dependencies
        var projectX = solution.AddProject("ProjectX");
        var projectY = solution.AddProject("ProjectY");

        solution.AddProjectReference(projectX, projectY);
        solution.AddProjectReference(projectY, projectX);

        // No dependencies
        var projectL = solution.AddProject("ProjectL");
        var projectM = solution.AddProject("ProjectM");


        List<CsprojModel> projectFiles = [
            new ("ProjectA", Path.GetFullPath(projectA), Path.GetRelativePath(solution.Directory, projectA)),
            new ("ProjectB", Path.GetFullPath(projectB), Path.GetRelativePath(solution.Directory, projectB)),
            new ("ProjectC", Path.GetFullPath(projectC), Path.GetRelativePath(solution.Directory, projectC)),
            new ("ProjectD", Path.GetFullPath(projectD), Path.GetRelativePath(solution.Directory, projectD)),
            new ("ProjectX", Path.GetFullPath(projectX), Path.GetRelativePath(solution.Directory, projectX)),
            new ("ProjectY", Path.GetFullPath(projectY), Path.GetRelativePath(solution.Directory, projectY)),
            new ("ProjectL", Path.GetFullPath(projectL), Path.GetRelativePath(solution.Directory, projectL)),
            new ("ProjectM", Path.GetFullPath(projectM), Path.GetRelativePath(solution.Directory, projectM))
            ];

        var diagram = new StringBuilder();

        // Act:
        Rules.AddProjectDependencies(projectFiles, diagram);

        // Assert:
        string expected =
        """
            ProjectA --> ProjectB
            ProjectA --> ProjectC
            ProjectB --> ProjectD
            ProjectX <--> ProjectY

        """;

        string actual = diagram.ToString();
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.Equal(expected, diagram.ToString());
    }

    [Fact(DisplayName = "A warning must be issued in the console **when** a bi-directional dependency is detected")]
    public void Should_Log_BiDirectional_Dependency_Warning()
    {
        // Arrange:
        using var solution = new TemporarySolutionBuilder();
        var projectX = solution.AddProject("ProjectX");
        var projectY = solution.AddProject("ProjectY");
        solution.AddProjectReference(projectX, projectY);
        solution.AddProjectReference(projectY, projectX);

        // Act:
        throw new NotImplementedException();

        // Assert: Verify that the warning is logged
        //string expectedWarning = "Bi-directional dependency detected between ProjectX and ProjectY";
        //_output.WriteLine("Expected Warning:\n" + expectedWarning);
    }

    [Fact(DisplayName = "Project nodes **must** be grouped into `subgraphs` based on their folder structure **when** multiple projects exist inside grouping folders")]
    public void ShouldCreateSubgraph_WhenSingleProjectInRoot()
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
        Rules.AddProjectHierarchy(projectFiles, diagram);

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

    [Fact(DisplayName = "Project nodes **must** be grouped into `subgraphs` based on their folder structure **when** multiple projects exist inside grouping folders")]
    public void ShouldCreateSubgraph_WhenMultipleProjectsInRoot()
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
        Rules.AddProjectHierarchy(projectFiles, diagram);

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

    // Configuration

    [Fact(DisplayName = "Project nodes that has source files, **may** include a url to the projects class diagram")]
    public void Should_Add_Url_To_Project_Class_Diagram()
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
        Rules.AddClickableLinks(projectFiles, diagram);

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
}
