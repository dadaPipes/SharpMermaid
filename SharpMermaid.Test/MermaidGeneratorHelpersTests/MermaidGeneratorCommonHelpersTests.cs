using SharpMermaid.MermaidGeneratorHelpers;
using System.Text;
using Xunit.Abstractions;

namespace SharpMermaid.Test.MermaidGeneratorHelpersTests;
public class MermaidGeneratorCommonHelpersTests(ITestOutputHelper output)
{
    private readonly ITestOutputHelper _output = output;

    [Fact(DisplayName = "AddMermaidBlockStart(diagramBuilder)")]
    public void ShouldAddMermaidBlockStart()
    {
        // Arrange: Create an empty StringBuilder to hold the Mermaid diagram
        var diagram = new StringBuilder();

        // Act: Call the helper method to add the Mermaid block start
        MermaidGeneratorCommonHelpers.AddMermaidBlockStart(diagram);

        // Assert: Verify that the StringBuilder contains the expected Mermaid block start and adds a new line
        string expected =
        $"""
        ```mermaid

        """;

        // Log expected and actual values for debugging
        string actual = diagram.ToString();
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.Equal(expected, diagram.ToString());
    }

    [Fact(DisplayName = "AddSolutionNameAsTitle(\"TestSolution\", diagram)")]
    public void ShouldAddSolutionNameAsTitle()
    {
        // Arrange: create an empty StringBuilder
        var diagram = new StringBuilder();

        // Act: add the solution name as title
        MermaidGeneratorCommonHelpers.AddSolutionNameAsTitle("TestSolution", diagram);

        // Assert: the output matches the expected Mermaid title block format and adds a new line
        string expected =
        $"""
        ---
        title: TestSolution
        ---

        """;

        // Log expected and actual values for debugging
        string actual = diagram.ToString();
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.Equal(expected, diagram.ToString());
    }

    [Fact(DisplayName = "AddDiagramFooter(diagramBuilder)")]
    public void ShouldAddDiagramFooter()
    {
        // Arrange: Create an empty StringBuilder to hold the Mermaid diagram
        var diagram = new StringBuilder();

        // Act: Call the helper method to add the diagram footer
        MermaidGeneratorCommonHelpers.AddDiagramFooter(diagram);

        // Assert: Verify that the StringBuilder contains the expected Mermaid footer
        string expected =
        $"""
        ```
        """;

        // Log expected and actual values for debugging
        string actual = diagram.ToString();
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.Equal(expected, diagram.ToString());
    }


}
