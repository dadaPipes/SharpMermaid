using Xunit.Abstractions;
namespace SharpMermaid.Test.MermaidGeneratorTests;
public class ClassDiagramTests(ITestOutputHelper output)
{
    private readonly ITestOutputHelper _output = output;

    [Fact(DisplayName = "Project With Single Class")]
    public void Should_Generate_Class_Diagram_With_Single_Class()
    {
        // Given a project contains one .cs file

        using var solution = new TemporarySolutionBuilder();

        solution.AddProjectWithFiles("ProjectA", new Dictionary<string, string>
        {
            ["X.cs"] = "public class X {}"
        });

        // When the diagram is generated
        var mermaidGenerator = new MermaidGenerator(solution.FullPath);
        var diagram = mermaidGenerator.ClassDiagram();

        // Then the diagram should have a title same as the project name  
        // And the diagram should include a node representing the.cs file, named according to the file's actual name  
        // And the class node should include a url to the source file

        string expected =
        $"""
        ```mermaid
        ---
        title: "ProjectA"
        ---
        classDiagram
        class X
        click X href "https://example.com/ProjectA/X.cs"
        ```
        """;

        // Log expected and actual values for debugging
        string actual = diagram;
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.Equal(expected, diagram);
    }
}