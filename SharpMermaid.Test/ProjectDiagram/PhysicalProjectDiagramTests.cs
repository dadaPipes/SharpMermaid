using Xunit.Abstractions;

namespace SharpMermaid.Test.ProjectDiagram;
public class PhysicalProjectDiagramTests(ITestOutputHelper output)
{
    private readonly ITestOutputHelper _output = output;

    [Fact]
    public void Should_Generate_Diagram_With_No_Projects()
    {
        // Given a solution with no projects
        using var solution = new TemporarySolutionBuilder();

        // When the diagram is generated
        var mermaidGenerator = new MermaidGenerator(solution.FullPath);
        var diagram = mermaidGenerator.PhysicalProjectDiagram();

        // Then the diagram should be empty
        string expected =
        $"""
        ```mermaid
        ---
        {solution.Name}
        ---
        graph
        ```
        """;

        // Log expected and actual values for debugging
        string actual = diagram;
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.Contains("```mermaid", diagram);
        Assert.Contains("graph", diagram);
        Assert.Equal(expected, diagram);
    }

    [Fact]
    public void Should_Generate_Diagram_With_Multiple_Projects_and_Zero_Project_Dependencies()
    {
        // Given a solution with multiple projects
        using var solution = new TemporarySolutionBuilder();

        solution.AddProject("ProjectA");
        solution.AddProject("ProjectB");
        solution.AddProject("ProjectC");

        // When the diagram is generated
        var mermaidGenerator = new MermaidGenerator(solution.FullPath);
        var diagram = mermaidGenerator.PhysicalProjectDiagram();

        // Then the diagram should include nodes for each project
        string expected =
        $"""
        ```mermaid
        ---
        {solution.Name}
        ---
        graph
            ProjectA
            ProjectB
            ProjectC
        ```
        """;

        // Log expected and actual values for debugging
        string actual = diagram;
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.True(mermaidGenerator._solution.HasProjects);
        Assert.Equal(expected, diagram);
    }

    [Fact]
    public void Should_Generate_Diagram_With_Multiple_Projects_and_Project_Dependencies()
    {
        // Given a solution with projects and dependencies
        using var solution = new TemporarySolutionBuilder();

        var projectA = solution.AddProject("ProjectA");
        var projectB = solution.AddProject("ProjectB");
        var projectC = solution.AddProject("ProjectC");

        solution.AddProjectReference(projectA, projectB);
        solution.AddProjectReference(projectA, projectC);
        solution.AddProjectReference(projectB, projectC);

        // When the diagram is generated
        var mermaidGenerator = new MermaidGenerator(solution.FullPath);
        var diagram = mermaidGenerator.PhysicalProjectDiagram();

        // Then the diagram should include nodes and arrows for each project and its dependencies
        string expected =
        $"""
        ```mermaid
        ---
        {solution.Name}
        ---
        graph
            ProjectA
            ProjectB
            ProjectC
            ProjectA --> ProjectB
            ProjectA --> ProjectC
            ProjectB --> ProjectC
        ```
        """;

        // Log expected and actual values for debugging
        string actual = diagram;
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.True(mermaidGenerator._solution.HasProjects);
        Assert.Equal(expected, diagram);
    }

    [Fact]
    public void Should_Generate_Diagram_With_1_Project_and_A_Clickable_URL()
    {
        // Given a solution with a single project named ProjectA,
        // And the project includes source files
        using var solution = new TemporarySolutionBuilder();

        solution.AddProjectWithFiles("ProjectA", new Dictionary<string, string>
        {
            ["X.cs"] = "public class X {}",
            ["Y.cs"] = "public class Y {}",
            ["Z.cs"] = "public class Z {}"
        });

        // When the diagram is generated
        var mermaidGenerator = new MermaidGenerator(solution.FullPath);
        var diagram = mermaidGenerator.PhysicalProjectDiagram();

        // Then the diagram should include a clickable URL for the project
        string expected =
        $"""
        ```mermaid
        ---
        {solution.Name}
        ---
        graph
            ProjectA
            click ProjectA "https://example.com/ProjectA/ProjectA.csproj"
        ```
        """;

        // Log expected and actual values for debugging
        string actual = diagram;
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.True(mermaidGenerator._solution.HasProjects);
        Assert.Equal(expected, diagram);
    }

    [Fact]
    public void Should_Generate_Diagram_With_Multiple_Projects_and_A_Clickable_URLs()
    {
        // Given a solution with multiple projects,
        // And each project includes .cs files
        using var solution = new TemporarySolutionBuilder();

        solution.AddProjectWithFiles("ProjectA", new Dictionary<string, string>
        {
            ["X.cs"] = "public class X {}",
            ["Y.cs"] = "public class Y {}",
            ["Z.cs"] = "public class Z {}"
        });

        solution.AddProjectWithFiles("ProjectB", new Dictionary<string, string>
        {
            ["A.cs"] = "public class A {}",
            ["B.cs"] = "public class B {}"
        });

        solution.AddProjectWithFiles("ProjectC", new Dictionary<string, string>
        {
            ["M.cs"] = "public class M {}",
            ["N.cs"] = "public class N {}"
        });

        // When the diagram is generated
        var mermaidGenerator = new MermaidGenerator(solution.FullPath);
        var diagram = mermaidGenerator.PhysicalProjectDiagram();

        // Then the diagram should include clickable URLs for each project node
        string expected =
        $"""
        ```mermaid
        ---
        {solution.Name}
        ---
        graph
            ProjectA
            ProjectB
            ProjectC
            click ProjectA "https://example.com/ProjectA/ProjectA.csproj"
            click ProjectB "https://example.com/ProjectB/ProjectB.csproj"
            click ProjectC "https://example.com/ProjectC/ProjectC.csproj"
        ```
        """;

        // Log expected and actual values for debugging
        string actual = diagram;
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.True(mermaidGenerator._solution.HasProjects);
        Assert.Equal(expected, diagram);
    }

    [Fact]
    public void Should_Generate_Diagram_With_Multiple_Projects_and_A_Clickable_URLs_and_Project_Dependencies()
    {
        // Given a solution with multiple projects and dependencies
        // And each project includes .cs files

        using var solution = new TemporarySolutionBuilder();

        var projectD = solution.AddProjectWithFiles("ProjectD", new Dictionary<string, string>
        {
            ["X.cs"] = "public class X {}",
            ["Y.cs"] = "public class Y {}",
            ["Z.cs"] = "public class Z {}"
        });

        var projectE = solution.AddProjectWithFiles("ProjectE", new Dictionary<string, string>
        {
            ["A.cs"] = "public class A {}",
            ["B.cs"] = "public class B {}"
        });

        var projectF = solution.AddProjectWithFiles("ProjectF", new Dictionary<string, string>
        {
            ["M.cs"] = "public class M {}",
            ["N.cs"] = "public class N {}"
        });

        solution.AddProjectReference(projectD, projectE);
        solution.AddProjectReference(projectD, projectF);
        solution.AddProjectReference(projectE, projectF);

        // When the diagram is generated
        var mermaidGenerator = new MermaidGenerator(solution.FullPath);
        var diagram = mermaidGenerator.PhysicalProjectDiagram();

        // Then the diagram should include clickable URLs for each project node and show project dependencies
        string expected =
        $"""
        ```mermaid
        ---
        {solution.Name}
        ---
        graph
            ProjectD
            ProjectE
            ProjectF
            click ProjectD "https://example.com/ProjectD/ProjectD.csproj"
            click ProjectE "https://example.com/ProjectE/ProjectE.csproj"
            click ProjectF "https://example.com/ProjectF/ProjectF.csproj"
            ProjectD --> ProjectE
            ProjectD --> ProjectF
            ProjectE --> ProjectF
        ```
        """;

        // Log expected and actual values for debugging
        string actual = diagram;
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.True(mermaidGenerator._solution.HasProjects);
        Assert.Equal(expected, diagram);
    }
}
