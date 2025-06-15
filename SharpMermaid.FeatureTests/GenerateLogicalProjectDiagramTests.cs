using SharpMermaid.TestHelpers;
using Xunit;

namespace SharpMermaid.FeatureTests;
public class GenerateLogicalProjectDiagramTests(ITestOutputHelper output)
{
    /*
    private readonly ITestOutputHelper _output = output;

    [Fact(DisplayName = "Solution With Without Projects")]
    public void Should_Generate_Diagram_With_No_Projects()
    {
        // Given the solution contains no projects
        using var solution = new TemporarySolutionBuilder();

        // When the diagram is generated
        var generator = new LogicalProjectDiagramGenerator(solution.FullPath);
        var diagram = generator.GenerateLogicalProjectDiagram();

        // Then the title should be the solution name
        // And the diagram should have no nodes or dependencies
        string expected =
        $"""
        ```mermaid
        ---
        title: {solution.Name}
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

    [Fact(DisplayName = "Solution With Root Projects Only")]
    public void Should_Generate_Diagram_With_Multiple_Projects_in_the_Root_Folder()
    {
        // Given all projects are in the solution root folder
        using var solution = new TemporarySolutionBuilder();

        solution.AddProject("ProjectA");
        solution.AddProject("ProjectB");
        solution.AddProject("ProjectC");

        // When the diagram is generated
        var generator = new LogicalProjectDiagramGenerator(solution.FullPath);
        var diagram   = generator.GenerateLogicalProjectDiagram();

        // Then the title should be the solution name
        // And the diagram should include a node for each project
        string expected =
        $"""
        ```mermaid
        ---
        title: {solution.Name}
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

        Assert.Equal(expected, diagram);
    }

    [Fact(DisplayName = "Solution With Root Projects Only With Dependencies")]
    public void Should_Generate_Diagram_With_Multiple_Projects_in_the_Root_Folder_with_Project_Dependencies()
    {
        // Given a solution with multiple projects in the root folder and dependencies
        // And some projects depend on each other
        using var solution = new TemporarySolutionBuilder();

        var projectA = solution.AddProject("ProjectA");
        var projectB = solution.AddProject("ProjectB");
        var projectC = solution.AddProject("ProjectC");

        solution.AddProjectReference(projectA, projectB);
        solution.AddProjectReference(projectA, projectC);
        solution.AddProjectReference(projectB, projectC);

        // When the diagram is generated
        var generator = new LogicalProjectDiagramGenerator(solution.FullPath);
        var diagram = generator.GenerateLogicalProjectDiagram();

        // Then the title should be the solution name
        // And the diagram should include a node for each project
        // And arrows should represent the dependencies between projects
        string expected =
        $"""
        ```mermaid
        ---
        title: {solution.Name}
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

        Assert.Equal(expected, diagram);
    }

    [Fact(DisplayName = "Mixed Folder Structure With 1 Root Project")]
    public void Should_Generate_Diagram_With_1_Project_In_The_Root_Folder_and_Multiple_Projects_in_1_Subfolder()
    {
        // Given one project is in the root folder
        // And the other projects are in a subfolder

        using var solution = new TemporarySolutionBuilder();

        var projectA = solution.AddProject("ProjectA");
        var projectB = solution.AddProject("Subfolder1", "ProjectB");
        var projectC = solution.AddProject("Subfolder1", "ProjectC");
        var projectD = solution.AddProject("Subfolder1", "ProjectD");

        solution.AddProjectReference(projectA, projectB);
        solution.AddProjectReference(projectA, projectC);
        solution.AddProjectReference(projectA, projectD);

        // When the diagram is generated
        var generator = new LogicalProjectDiagramGenerator(solution.FullPath);
        var diagram = generator.GenerateLogicalProjectDiagram();

        // Then the title should be the solution name  
        // And the diagram should include a node for each project
        // And nodes should be grouped into subgraphs based on their folder structure
        // And And arrows should represent project dependencies  

        string expected =
            $"""
        ```mermaid
        ---
        title: {solution.Name}
        ---
        graph
            ProjectA
            subgraph Subfolder1
                ProjectB
                ProjectC
                ProjectD
            end
            ProjectA --> ProjectB
            ProjectA --> ProjectC
            ProjectA --> ProjectD
        ```
        """;

        // Log expected and actual values for debugging
        string actual = diagram;
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.Equal(expected, diagram);
    }

    [Fact(DisplayName = "Mixed Folder Structure With Multiple Root Projects")]
    public void Should_Generate_Diagram_With_1_Project_In_The_Root_Folder_and_Multiple_Projects_in_Subfolders_Including_Dependencies()
    {
        // Given  multiple projects are in the root folder
        // And other projects are in subfolders
        // And some projects depend on each other
        using var solution = new TemporarySolutionBuilder();

        var projectA = solution.AddProject("ProjectA");
        var projectB = solution.AddProject("Subfolder1", "ProjectB");
        var projectC = solution.AddProject("Subfolder1", "ProjectC");
        var projectD = solution.AddProject("Subfolder1/Subfolder2", "ProjectD");
        var projectE = solution.AddProject("Subfolder1/Subfolder2", "ProjectE");

        solution.AddProjectReference(projectA, projectB);
        solution.AddProjectReference(projectA, projectC);
        solution.AddProjectReference(projectC, projectD);
        solution.AddProjectReference(projectC, projectE);

        // When the diagram is generated
        var generator = new LogicalProjectDiagramGenerator(solution.FullPath);
        var diagram = generator.GenerateLogicalProjectDiagram();

        // Then the title should be the solution name  
        // And the diagram should include a node for each project
        // And nodes should be grouped into subgraphs based on their folder structure
        // And And arrows should represent project dependencies
        string expected =
        $"""
        ```mermaid
        ---
        title: {solution.Name}
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

        Assert.Equal(expected, diagram);
    }
    */
}
