using SharpMermaid.Features.GeneratePhysicalProjectDiagram;
using SharpMermaid.Models;
using SharpMermaid.TestHelpers;
using System.Text;
using Xunit.Abstractions;

namespace SharpMermaid.RulesTests;
public class GeneratePhysicalProjectDiagramTests(ITestOutputHelper output)
{
    private readonly ITestOutputHelper _output = output;

    [Fact(DisplayName = "Rule 1: The code block **must** start with` ```mermaid`")]
    public void ShouldAddMermaidBlockStart()
    {
        // Arrange:
        var diagram = new StringBuilder();

        // Act:
        Rules.AddMermaidBlockStart(diagram);

        // Assert:
        string expected =
        $"""
        ```mermaid

        """;

        string actual = diagram.ToString();
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.Equal(expected, diagram.ToString());
    }

    [Fact(DisplayName = "Rule 2: The code block **must** end with ` ``` `")]
    public void ShouldAddDiagramFooter()
    {
        // Arrange:
        var diagram = new StringBuilder();

        // Act:
        Rules.AddDiagramFooter(diagram);

        // Assert:
        string expected =
        $"""
        ```
        """;

        string actual = diagram.ToString();
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.Equal(expected, diagram.ToString());
    }

    [Fact(DisplayName = "Rule 3: The diagram **must** begin with a `graph` declaration")]
    public void ShouldAddGraphDeclaration()
    {
        // Arrange:
        var diagram = new StringBuilder();

        // Act:
        Rules.AddGraphDeclaration(diagram);

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

    [Fact(DisplayName = "Rule 5: Diagram **must** have a title same as the solution name")]
    public void ShouldAddSolutionNameAsTitle()
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

    [Fact(DisplayName = "Rule 6.a: If a single project is selected by the developer, a node **must** be included in the diagram")]
    public void ShouldIncludeSingleNode_WhenSingleProjectIsSelected()
    {
        // Arrange:
        var diagram        = new StringBuilder();
        using var solution = new TemporarySolutionBuilder();

        var projectA = solution.AddProject("Project1");
                   _ = solution.AddProject("Project2");
                   _ = solution.AddProject("Project3");

        List<CsprojModel> projectFiles = [
            new ("Project1", Path.GetFullPath(projectA), Path.GetRelativePath(solution.Directory, projectA))
            ];

        // Act:
        Rules.AddProjectNames(projectFiles, diagram);

        // Assert:
        string expected =
        """
            Project1

        """;

        string actual = diagram.ToString();
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.Equal(expected, diagram.ToString());
    }

    [Fact(DisplayName = "Rule 6.b: If multiple projects are selected by the developer, nodes **must** be included in the diagram")]
    public void ShouldIncludeMultipleNodes_WhenMultipleProjectsAreSelected()
    {
        // Arrange:
        var diagram = new StringBuilder();

        using var solution = new TemporarySolutionBuilder();

        var projectA = solution.AddProject("Project1");
        var projectB = solution.AddProject("Project2");
                   _ = solution.AddProject("Project3");

        List<CsprojModel> projectFiles = [
            new ("Project1", Path.GetFullPath(projectA), Path.GetRelativePath(solution.Directory, projectA)),
            new ("Project2", Path.GetFullPath(projectB), Path.GetRelativePath(solution.Directory, projectB))
            ];

        // Act:
        Rules.AddProjectNames(projectFiles, diagram);

        // Assert:
        string expected =
        """
            Project1
            Project2

        """;
        string actual = diagram.ToString();
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);
        Assert.Equal(expected, diagram.ToString());
    }

    [Fact(DisplayName = "Rule 7.a: If a dependency exists, the relationship **must** be indicated with an arrow")]
    public void ShouldShowRelationshipWithArrow_WhenDependencyExists()
    {
        // Arrange:
        using var solution = new TemporarySolutionBuilder();

        var project1 = solution.AddProject("Project1");
        var project2 = solution.AddProject("Project2");
        var project3 = solution.AddProject("Project3");
        var project4 = solution.AddProject("Project4");

        solution.AddProjectReference(project1, project2);
        solution.AddProjectReference(project1, project3);
        solution.AddProjectReference(project2, project4);

        List<CsprojModel> projectFiles = [
            new ("Project1", Path.GetFullPath(project1), Path.GetRelativePath(solution.Directory, project1)),
            new ("Project2", Path.GetFullPath(project2), Path.GetRelativePath(solution.Directory, project2)),
            new ("Project3", Path.GetFullPath(project3), Path.GetRelativePath(solution.Directory, project3)),
            new ("Project4", Path.GetFullPath(project4), Path.GetRelativePath(solution.Directory, project4))
            ];

        var diagram = new StringBuilder();

        // Act:
        var warningMesssage = Rules.AddProjectDependencies(projectFiles, diagram);

        // Assert:
        string expected =
        """
            Project1 --> Project2
            Project1 --> Project3
            Project2 --> Project4

        """;

        string actual = diagram.ToString();
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.Equal(expected, diagram.ToString());
        Assert.Null(warningMesssage);
    }

    [Fact(DisplayName = "Rule 7.b: If a dependency is bi-directional, the arrow **must** be <--> **and** a warning **must** be issued in the console")]
    public void ShouldShowBidirectionalDependencyWithArrow_AndWarning()
    {
        // Arrange:
        using var solution = new TemporarySolutionBuilder();

        var project1 = solution.AddProject("Project1");
        var project2 = solution.AddProject("Project2");

        solution.AddProjectReference(project1, project2);
        solution.AddProjectReference(project2, project1);

        List<CsprojModel> projectFiles = [
            new ("Project1", Path.GetFullPath(project1), Path.GetRelativePath(solution.Directory, project1)),
            new ("Project2", Path.GetFullPath(project2), Path.GetRelativePath(solution.Directory, project2)),
            ];

        var diagram = new StringBuilder();

        // Act:
        var warningMesssage = Rules.AddProjectDependencies(projectFiles, diagram);

        // Assert:
        string expected =
        """
            Project1 <--> Project2

        """;

        string actual = diagram.ToString();
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.Equal(expected, diagram.ToString());
        Assert.NotNull(warningMesssage);
    }

    [Fact(DisplayName = "Rule 7.c: If a project has no dependencies, it **must not** display an arrow")]
    public void ShouldNotDisplayArrow_WhenNoDependencyExists()
    {
        // Arrange:
        using var solution = new TemporarySolutionBuilder();

        var project1 = solution.AddProject("Project1");
        var project2 = solution.AddProject("Project2");
        var project3 = solution.AddProject("Project3");

        List<CsprojModel> projectFiles = [
            new ("Project1", Path.GetFullPath(project1), Path.GetRelativePath(solution.Directory, project1)),
            new ("Project2", Path.GetFullPath(project2), Path.GetRelativePath(solution.Directory, project2)),
            new ("Project3", Path.GetFullPath(project3), Path.GetRelativePath(solution.Directory, project3)),
            ];

        var diagram = new StringBuilder();

        // Act:
        var warningMesssage = Rules.AddProjectDependencies(projectFiles, diagram);

        // Assert:
        string expected =
        """

        """;

        string actual = diagram.ToString();
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.Equal(expected, diagram.ToString());
        Assert.Null(warningMesssage);
    }

    [Fact(DisplayName = "Rule 8.a: Projects without `.cs` files **must** display a warning message \"No .cs files found in {Name}\", in the console")]
    public void ShouldIncludeClickableUrl_WhenClassDiagramUrlsAreEnabled()
    {
        // Arrange:
        using var solution = new TemporarySolutionBuilder();

        var project1 = solution.AddProjectWithFiles("Project1", new Dictionary<string, string> { ["A.cs"] = "public class A" });
        var project2 = solution.AddProjectWithFiles("Project2", new Dictionary<string, string> { ["B.cs"] = "public class B" });
        var project3 = solution.AddProject("Project3");
        var project4 = solution.AddProject("Project4");


        List<CsprojModel> projectFiles = [
            new ("Project1", Path.GetFullPath(project1), Path.GetRelativePath(solution.Directory, project1), true, "https://example.com/Project1/Project1.csproj"),
            new ("Project2", Path.GetFullPath(project2), Path.GetRelativePath(solution.Directory, project2), true, "https://example.com/Project2/Project2.csproj"),
            new ("Project3", Path.GetFullPath(project3), Path.GetRelativePath(solution.Directory, project3), true, "https://example.com/Project3/Project3.csproj"),
            new ("Project4", Path.GetFullPath(project4), Path.GetRelativePath(solution.Directory, project4), true, "https://example.com/Project4/Project4.csproj")
            ];

        var diagram = new StringBuilder();

        var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);

        // Act:
        var warnings = Rules.AddClickableLinks(projectFiles, diagram);

        // Assert:
        string expected =
        """
            click Project1 "https://example.com/Project1/Project1.csproj"
            click Project2 "https://example.com/Project2/Project2.csproj"
        
        """;



        string actual = diagram.ToString();
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);
        Assert.Equal(expected, diagram.ToString());
    }

    [Fact(DisplayName = "Rule 8.b: If the developer has enabled class diagram URLs, the diagram **must** include a clickable URL for each project")]
    public void ShouldDisplayTopLevelTypes_WhenEnabled()
    {
        throw new NotImplementedException();

        /*
        ```mermaid
        graph
        Project1["
            **Project1**
            ClassA
            ClassB
        "]
        Project2["
            **Project2**
            ClassC
            ClassD
        "]
        Project1 --> Project2
        click Project1 "https://example.com/Project1" "Project1 Desc"
        click Project2 "https://example.com/Project2" "Project2 Desc"

        ```
         */
    }

    [Fact(DisplayName = "Rule 8.c: If the developer has not enabled class diagram URLs, the diagram **must not** include any clickable URLs for projects")]
    public void ShouldNotDisplayTopLevelTypes_WhenDisabled()
    {
        throw new NotImplementedException();

        /*
        ```mermaid
        graph
        Project1
        Project2
        Project1 --> Project2
        click Project1 "https://example.com/Project1" "Project1 Desc"
        click Project2 "https://example.com/Project2" "Project2 Desc"
        ```
         */
    }

    [Fact(DisplayName = "Rule 9.a: If a developer has enabled public types, the node **must** display all public top-level types")]
    public void ShouldDisplayAllPublicTopLevelTypes()
    {
        throw new NotImplementedException();
    }

    [Fact(DisplayName = "Rule 9.a.1: If more than a single public top-level type exists in the same `.cs` file, a warning message **must** be displayed in the console")]
    public void ShouldDisplayAWarning_WhenMultipleTopLevelTypesExist()
    {
        throw new NotImplementedException();
    }

    [Fact(DisplayName = "Rule 9.b: If the developer has not enabled public types, the node **must not** display any public types")]
    public void ShouldNotDisplayPublicTypes_WhenEnabled()
    {
        throw new NotImplementedException();
    }
}