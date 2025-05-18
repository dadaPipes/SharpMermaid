using SharpMermaid.MermaidGeneratorHelpers;
using System.Text;
using Xunit.Abstractions;

namespace SharpMermaid.Test.MermaidGeneratorHelpersTests;
public class MermaidGeneratorProjectHelpersTests(ITestOutputHelper output)
{
    private readonly ITestOutputHelper _output = output;

    [Fact(DisplayName = "AddGraphDeclaration(diagramBuilder)")]
    public void ShouldAddGraphDeclaration()
    {
        // Arrange: Create an empty StringBuilder to hold the Mermaid diagram
        var diagramBuilder = new StringBuilder();

        // Act: Call the helper method to add the graph declaration
        MermaidGeneratorProjectHelpers.AddGraphDeclaration(diagramBuilder);

        // Assert: Verify that the StringBuilder contains the expected graph declaration and adds a new line
        string expected =
        $"""
        graph

        """;

        Assert.Equal(expected, diagramBuilder.ToString());
    }

    [Fact(DisplayName = "AddProjectNames(projectFiles, diagramBuilder)")]
    public void ShouldAppendProjectNamesToDiagram()
    {
        using var solution = new TemporarySolutionBuilder();

        // Arrange: Create a temporary solution with projects
        var projectA = solution.AddProject("ProjectA");
        var projectB = solution.AddProject("ProjectB");
        var projectC = solution.AddProject("ProjectC");

        // Load projects into CsprojModel instances
        var projectFiles = new List<CsprojModel>
        {
            new ("ProjectA", Path.GetFullPath(projectA), Path.GetRelativePath(solution.Directory, projectA)),
            new ("ProjectB", Path.GetFullPath(projectB), Path.GetRelativePath(solution.Directory, projectB)),
            new ("ProjectC", Path.GetFullPath(projectC), Path.GetRelativePath(solution.Directory, projectC))
        };

        // Create an empty StringBuilder to store the diagram content
        var diagramBuilder = new StringBuilder();

        // Act: Add project names to the diagram
        MermaidGeneratorProjectHelpers.AddProjectNames(projectFiles, diagramBuilder);

        // Assert: Verify that the output matches the expected format and adds a new line
        string expected =
        """
            ProjectA
            ProjectB
            ProjectC

        """;

        Assert.Equal(expected, diagramBuilder.ToString());
    }

    [Fact(DisplayName = "AddProjectDependencies(projectFiles, diagramBuilder")]
    public void ShouldAppendProjectDependenciesToDiagram()
    {
        // Arrange: Create a temporary solution with projects
        using var builder = new TemporarySolutionBuilder();

        var projectA = builder.AddProject("ProjectA");
        var projectB = builder.AddProject("ProjectB");
        var projectC = builder.AddProject("ProjectC");
        var projectD = builder.AddProject("ProjectD");

        // Add project dependencies
        builder.AddProjectReference(projectA, projectB);
        builder.AddProjectReference(projectA, projectC);
        builder.AddProjectReference(projectB, projectD);

        // Load projects into CsprojModel instances
        var projectFiles = new List<CsprojModel>
        {
            new ("ProjectA", Path.GetFullPath(projectA), Path.GetRelativePath(builder.Directory, projectA)),
            new ("ProjectB", Path.GetFullPath(projectB), Path.GetRelativePath(builder.Directory, projectB)),
            new ("ProjectC", Path.GetFullPath(projectC), Path.GetRelativePath(builder.Directory, projectC)),
            new ("ProjectD", Path.GetFullPath(projectD), Path.GetRelativePath(builder.Directory, projectD))
        };

        // Create an empty StringBuilder to store the diagram content
        var diagramBuilder = new StringBuilder();

        // Act: Add project dependencies to the diagram
        MermaidGeneratorProjectHelpers.AddProjectDependencies(projectFiles, diagramBuilder);

        // Assert: Verify that the output matches the expected format and adds a new line
        string expected =
        """
            ProjectA --> ProjectB
            ProjectA --> ProjectC
            ProjectB --> ProjectD

        """;

        Assert.Equal(expected, diagramBuilder.ToString());
    }

    [Fact(DisplayName = "AddClickableLinks(projectFiles, diagramBuilder)")]
    public void ShouldAppendClickableLinksToDiagram()
    {
        // Arrange: Create a Solution with Projects
        using var builder = new TemporarySolutionBuilder();

        var projectA = builder.AddProjectWithFiles("ProjectA", new Dictionary<string, string> { ["X.cs"] = "public class MyClass" });
        var projectB = builder.AddProjectWithFiles("ProjectB", new Dictionary<string, string> { ["Y.cs"] = "public class YourClass" });

        // Load projects with source files into CsprojModel instances
        var projectFiles = new List<CsprojModel>
        {
            new ("ProjectA", Path.GetFullPath(projectA), Path.GetRelativePath(builder.Directory, projectA)),
            new ("ProjectB", Path.GetFullPath(projectB), Path.GetRelativePath(builder.Directory, projectB))
        };

        // Mock the Settings.BaseUrl
        Settings.BaseUrl = "https://example.com/repo";

        // Create an empty StringBuilder to store the diagram content
        var diagramBuilder = new StringBuilder();

        // Act: Add clickable links to the diagram
        MermaidGeneratorProjectHelpers.AddClickableLinks(projectFiles, diagramBuilder);

        // Assert: Verify that the output matches the expected format and adds a new line
        string expected =
        """
            click ProjectA "https://example.com/repo/ProjectA/ProjectA.csproj"
            click ProjectB "https://example.com/repo/ProjectB/ProjectB.csproj"

        """;

        Assert.Equal(expected, diagramBuilder.ToString());
    }

    [Fact(DisplayName = "AddProjectHierarchy(solution, projectFiles, diagram)")]
    public void AddProjectHierarchy_ShouldAppendProjectHierarchyToDiagramWithOneProjectInTheSolutionRoot()
    {
        // Arrange: Create a temporary solution with projects
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

        // Load projects into CsprojModel instances
        var projectFiles = new List<CsprojModel>
        {
            new ("ProjectA", Path.GetFullPath(projectA), Path.GetRelativePath(builder.Directory, projectA)),
            new ("ProjectB", Path.GetFullPath(projectB), Path.GetRelativePath(builder.Directory, projectB)),
            new ("ProjectC", Path.GetFullPath(projectC), Path.GetRelativePath(builder.Directory, projectC)),
            new ("ProjectD", Path.GetFullPath(projectD), Path.GetRelativePath(builder.Directory, projectD)),
            new ("ProjectE", Path.GetFullPath(projectE), Path.GetRelativePath(builder.Directory, projectE)),
            new ("ProjectX", Path.GetFullPath(projectX), Path.GetRelativePath(builder.Directory, projectX)),
            new ("ProjectY", Path.GetFullPath(projectY), Path.GetRelativePath(builder.Directory, projectY)),
            new ("ProjectZ", Path.GetFullPath(projectZ), Path.GetRelativePath(builder.Directory, projectZ))
        };

        // Create an empty StringBuilder to store the diagram content
        var diagram = new StringBuilder();

        // Act: Add project hierarchy to the diagram
        MermaidGeneratorProjectHelpers.AddProjectHierarchy(solution, projectFiles, diagram);

        // Assert: Verify that the output matches the expected format and adds a new line
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

        // Log expected and actual values for debugging
        string actual = diagram.ToString();
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.Equal(expected, diagram.ToString());
    }

    [Fact(DisplayName = "AddProjectHierarchy(solution, projectFiles, diagram)")]
    public void AddProjectHierarchy_ShouldAppendProjectHierarchyToDiagramWithMultipleProjectsInTheSolutionRoot()
    {
        // Arrange: Create a temporary solution with projects
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

        // Load projects into CsprojModel instances
        var projectFiles = new List<CsprojModel>
        {
            new ("ProjectA", Path.GetFullPath(projectA), Path.GetRelativePath(builder.Directory, projectA)),
            new ("ProjectAb", Path.GetFullPath(projectAb), Path.GetRelativePath(builder.Directory, projectAb)),
            new ("ProjectB", Path.GetFullPath(projectB), Path.GetRelativePath(builder.Directory, projectB)),
            new ("ProjectC", Path.GetFullPath(projectC), Path.GetRelativePath(builder.Directory, projectC)),
            new ("ProjectD", Path.GetFullPath(projectD), Path.GetRelativePath(builder.Directory, projectD)),
            new ("ProjectE", Path.GetFullPath(projectE), Path.GetRelativePath(builder.Directory, projectE)),
            new ("ProjectX", Path.GetFullPath(projectX), Path.GetRelativePath(builder.Directory, projectX)),
            new ("ProjectY", Path.GetFullPath(projectY), Path.GetRelativePath(builder.Directory, projectY)),
            new ("ProjectZ", Path.GetFullPath(projectZ), Path.GetRelativePath(builder.Directory, projectZ))
        };

        // Create an empty StringBuilder to store the diagram content
        var diagram = new StringBuilder();

        // Act: Add project hierarchy to the diagram
        MermaidGeneratorProjectHelpers.AddProjectHierarchy(solution, projectFiles, diagram);

        // Assert: Verify that the output matches the expected format and adds a new line
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

        // Log expected and actual values for debugging
        string actual = diagram.ToString();
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.Equal(expected, diagram.ToString());
    }
}
