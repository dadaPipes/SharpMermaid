using Xunit.Abstractions;

namespace SharpMermaid.Test.ProjectDiagram;
public class LogicalProjectDiagramTests(ITestOutputHelper output)
{
    private readonly ITestOutputHelper _output = output;

    [Fact]
    public void Should_Generate_Diagram_With_No_Projects()
    {
        // Given a solution with no projects
        using var solution = new TemporarySolutionBuilder();

        // When the diagram is generated
        var mermaidGenerator = new MermaidGenerator(solution.FullPath);
        var diagram = mermaidGenerator.LogicalProjectDiagram();
        
        // Then the root folder should be represented as a grouping box, with the same name as the folder.
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

        Assert.Equal(expected, diagram);
    }

    [Fact]
    public void Should_Generate_Diagram_With_Multiple_Projects_in_the_Root_Folder()
    {
        // Given all projects are in the solution root folder,
        // And the projects have dependencies
        using var solution = new TemporarySolutionBuilder();

        solution.AddProject("ProjectA");
        solution.AddProject("ProjectB");
        solution.AddProject("ProjectC");

        // When the diagram is generated
        var mermaidGenerator = new MermaidGenerator(solution.FullPath);
        var diagram = mermaidGenerator.LogicalProjectDiagram();

        // Then the diagram should:
        // The root folder should be represented as a grouping box, with the same name as the folder.
        // Represent each project as an individual node
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
    public void Should_Generate_Diagram_With_Multiple_Projects_in_the_Root_Folder_with_Project_Dependencies()
    {
        // Given a solution with multiple projects in the root folder and dependencies
        using var solution = new TemporarySolutionBuilder();

        var projectA = solution.AddProject("ProjectA");
        var projectB = solution.AddProject("ProjectB");
        var projectC = solution.AddProject("ProjectC");

        solution.AddProjectReference(projectA, projectB);
        solution.AddProjectReference(projectA, projectC);
        solution.AddProjectReference(projectB, projectC);

        // When the diagram is generated
        var mermaidGenerator = new MermaidGenerator(solution.FullPath);
        var diagram = mermaidGenerator.LogicalProjectDiagram();

        // Then the diagram should:
        // The root folder should be represented as a grouping box, with the same name as the folder.
        // Represent each project as an individual node
        // Include arrows representing dependencies between projects.
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
    public void Should_Generate_Diagram_With_1_Project_In_The_Root_Folder_and_Multiple_Projects_in_1_Subfolder()
    {
        // Given a solution with 1 project in the root folder and multiple projects in 1 subfolder

        using var solution = new TemporarySolutionBuilder();

        solution.AddProject("ProjectA");
        solution.AddProject("Subfolder1", "ProjectB");
        solution.AddProject("Subfolder1", "ProjectC");
        solution.AddProject("Subfolder1", "ProjectD");

        // When the diagram is generated
        var mermaidGenerator = new MermaidGenerator(solution.FullPath);
        var diagram = mermaidGenerator.LogicalProjectDiagram();

        // Then the diagram should:
        // The root folder should be represented as a grouping box, with the same name as the folder.
        // Represent each project as an individual node.
        // Group projects visually according to their folder structure.

        string expected =
            $"""
        ```mermaid
        ---
        {solution.Name}
        ---
        graph
            ProjectA
            subgraph Subfolder1
                ProjectB
                ProjectC
                ProjectD
            end
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
    public void Should_Generate_Diagram_With_1_Project_In_The_Root_Folder_and_Multiple_Projects_in_Subfolders_Including_Dependencies()
    {
        // Given a solution with  1 project in the root folder and projects in subfolders including dependencies
        using var solution = new TemporarySolutionBuilder();

        var projectA = solution.AddProject("ProjectA");
        var projectB = solution.AddProject("Subfolder1","ProjectB");
        var projectC = solution.AddProject("Subfolder1", "ProjectC");
        var projectD = solution.AddProject("Subfolder1/Subfolder2", "ProjectD");
        var projectE = solution.AddProject("Subfolder1/Subfolder2", "ProjectE");

        solution.AddProjectReference(projectA, projectB);
        solution.AddProjectReference(projectA, projectC);
        solution.AddProjectReference(projectC, projectD);
        solution.AddProjectReference(projectC, projectE);

        // When the diagram is generated
        var mermaidGenerator = new MermaidGenerator(solution.FullPath);
        var diagram = mermaidGenerator.LogicalProjectDiagram();

        // Then the diagram should:
        // The root folder should be represented as a grouping box, with the same name as the folder.
        // Represent each project as an individual node.
        // Group projects visually according to their folder structure.
        // Include arrows representing dependencies between projects.
        // Each folder should be represented as a grouping box, with the same name as the folder.

        string expected =
        $"""
        ```mermaid
        ---
        {solution.Name}
        ---
        graph
            ProjectA
            subgraph Subfolder1
                ProjectB
                ProjectC
                subgraph Subfolder2
                    ProjectD
                    ProjectE
                end
            end
            ProjectA --> ProjectB
            ProjectA --> ProjectC
            ProjectC --> ProjectD
            ProjectC --> ProjectE
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
    public void Should_Generate_Diagram_With_Multiple_Projects_in_The_Root_Folder_and_Multiple_Projects_in_Subfolders_Including_Dependencies()
    {
        // Given a solution with projects in subfolders
        using var solution = new TemporarySolutionBuilder();

        var projectA = solution.AddProject("ProjectA");
        var projectB = solution.AddProject("ProjectB");
        var projectC = solution.AddProject("Subfolder1", "ProjectC");
        var projectD = solution.AddProject("Subfolder1", "ProjectD");
        var projectE = solution.AddProject("Subfolder1/Subfolder2", "ProjectE");
                       solution.AddProject("Subfolder1/Subfolder2", "ProjectF");

        solution.AddProjectReference(projectA, projectB);
        solution.AddProjectReference(projectA, projectC);
        solution.AddProjectReference(projectC, projectD);
        solution.AddProjectReference(projectC, projectE);

        // When the diagram is generated
        var mermaidGenerator = new MermaidGenerator(solution.FullPath);
        var diagram = mermaidGenerator.LogicalProjectDiagram();


        // Then the diagram should:
        // The root folder should be represented as a grouping box, with the same name as the folder.
        // Represent each project as an individual node.
        // Group projects visually according to their folder structure.
        // Include arrows representing dependencies between projects.
        // Each folder should be represented as a grouping box, with the same name as the folder.

        string expected =
        $"""
        ```mermaid
        ---
        {solution.Name}
        ---
        graph
            ProjectA
            ProjectB
            subgraph Subfolder1
                ProjectC
                ProjectD
                subgraph Subfolder2
                    ProjectE
                    ProjectF
                end
            end
            ProjectA --> ProjectB
            ProjectA --> ProjectC
            ProjectC --> ProjectD
            ProjectC --> ProjectE
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