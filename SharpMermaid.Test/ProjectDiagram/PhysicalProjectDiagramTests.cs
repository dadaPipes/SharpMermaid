namespace SharpMermaid.Test.ProjectDiagram;
public class PhysicalProjectDiagramTests
{
    [Fact]
    public void Should_Return_Diagram_With_No_Projects()
    {
        // Given a solution with no projects
        using var builder = new TemporarySolutionBuilder("TestSolution");

        // When the diagram is generated
        var diagram = MermaidGenerator.PhysicalProjectDiagram(builder.RootPath);

        // Then the diagram should be empty
        const string expected =
        """
        ```mermaid
        graph TD
        ```
        """;
        Assert.Equal(expected, diagram);
    }

    [Fact]
    public void Should_Return_Diagram_With_Multiple_Projects_and_Zero_Project_Dependencies()
    {
        // Given a solution with multiple projects
        using var builder = new TemporarySolutionBuilder("TestSolution");

        var projectA = builder.AddProject("ProjectA");
        var projectB = builder.AddProject("ProjectB");
        var projectC = builder.AddProject("ProjectC");

        // When the diagram is generated
        var diagram = MermaidGenerator.PhysicalProjectDiagram(builder.RootPath);

        // Then the diagram should include nodes for each project
        const string expected =
        """
        ```mermaid
        graph TD
            ProjectA
            ProjectB
            ProjectC
        ```
        """;

        Assert.Equal(expected, diagram);
    }

    [Fact]
    public void Should_Return_Diagram_With_Multiple_Projects_and_Project_Dependencies()
    {
        // Given a solution with projects and dependencies
        using var builder = new TemporarySolutionBuilder("TestSolution");

        var projectA = builder.AddProject("ProjectA");
        var projectB = builder.AddProject("ProjectB");
        var projectC = builder.AddProject("ProjectC");

        builder.AddProjectReference(projectA, projectB);
        builder.AddProjectReference(projectA, projectC);
        builder.AddProjectReference(projectB, projectC);

        // When the diagram is generated
        var diagram = MermaidGenerator.PhysicalProjectDiagram(builder.RootPath);

        // Then the diagram should include nodes and edges for each project and its dependencies
        const string expected =
        """
        ```mermaid
        graph TD
            ProjectA
            ProjectB
            ProjectC
            ProjectA --> ProjectB
            ProjectA --> ProjectC
            ProjectB --> ProjectC
        ```
        """;

        Assert.Equal(expected, diagram);
    }

    [Fact]
    public void Should_Generate_Diagram_With_1_Project_and_A_Clickable_URL()
    {
        // Given a solution with a single project,
        // And the project includes .cs files
        using var builder = new TemporarySolutionBuilder("TestSolution");

        var projectA = builder.AddProjectWithFiles("ProjectA", new Dictionary<string, string>
        {
            ["X.cs"] = "public class X {}",
            ["Y.cs"] = "public class Y {}",
            ["Z.cs"] = "public class Z {}"
        });

        // When the diagram is generated
        var diagram = MermaidGenerator.PhysicalProjectDiagram(builder.RootPath);

        // Then the diagram should include a clickable URL for the project
        const string expected =
        """
        ```mermaid
        graph TD
            ProjectA
            click ProjectA "dummy"
        ```
        """;

        Assert.Equal(expected, diagram);
    }

    [Fact]
    public void Should_Generate_Diagram_With_Multiple_Projects_and_A_Clickable_URLs()
    {
        // Given a solution with multiple projects,
        // And each project includes .cs files
        using var builder = new TemporarySolutionBuilder("TestSolution");

        var projectA = builder.AddProjectWithFiles("ProjectA", new Dictionary<string, string>
        {
            ["X.cs"] = "public class X {}",
            ["Y.cs"] = "public class Y {}",
            ["Z.cs"] = "public class Z {}"
        });

        var projectB = builder.AddProjectWithFiles("ProjectB", new Dictionary<string, string>
        {
            ["A.cs"] = "public class A {}",
            ["B.cs"] = "public class B {}"
        });

        var projectC = builder.AddProjectWithFiles("ProjectC", new Dictionary<string, string>
        {
            ["M.cs"] = "public class M {}",
            ["N.cs"] = "public class N {}"
        });

        // When the diagram is generated
        var diagram = MermaidGenerator.PhysicalProjectDiagram(builder.RootPath);

        // Then the diagram should include clickable URLs for each project node
        const string expected =
        """
        ```mermaid
        graph TD
            ProjectA
            click ProjectA "dummy"
            ProjectB
            click ProjectB "dummy"
            ProjectC
            click ProjectC "dummy"
        ```
        """;
    }

    [Fact]
    public void Should_Generate_Diagram_With_Multiple_Projects_and_A_Clickable_URLs_and_Project_Dependencies()
    {
        // Given a solution with multiple projects and dependencies
        // And each project includes .cs files

        using var builder = new TemporarySolutionBuilder("TestSolution");

        var projectD = builder.AddProjectWithFiles("ProjectD", new Dictionary<string, string>
        {
            ["X.cs"] = "public class X {}",
            ["Y.cs"] = "public class Y {}",
            ["Z.cs"] = "public class Z {}"
        });

        var projectE = builder.AddProjectWithFiles("ProjectE", new Dictionary<string, string>
        {
            ["A.cs"] = "public class A {}",
            ["B.cs"] = "public class B {}"
        });

        var projectF = builder.AddProjectWithFiles("ProjectF", new Dictionary<string, string>
        {
            ["M.cs"] = "public class M {}",
            ["N.cs"] = "public class N {}"
        });

        builder.AddProjectReference(projectD, projectE);
        builder.AddProjectReference(projectD, projectF);
        builder.AddProjectReference(projectE, projectF);

        // When the diagram is generated
        var diagram = MermaidGenerator.PhysicalProjectDiagram(builder.RootPath);

        // Then the diagram should include clickable URLs for each project node and show project dependencies

        const string expected =
        """
        ```mermaid
        graph TD
            ProjectD
            ProjectE
            ProjectF
            click ProjectD "dummy"
            click ProjectE "dummy"
            click ProjectF "dummy"
            ProjectD --> ProjectE
            ProjectD --> ProjectF
            ProjectE --> ProjectF
        ```
        """;

        Assert.Equal(expected, diagram);
    }
}
