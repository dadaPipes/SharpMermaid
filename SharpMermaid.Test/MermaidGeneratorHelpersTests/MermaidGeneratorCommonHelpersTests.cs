using SharpMermaid.MermaidGeneratorHelpers;
using System.Text;

namespace SharpMermaid.Test.MermaidGeneratorHelpersTests;
public class MermaidGeneratorCommonHelpersTests()
{
    [Fact(DisplayName = "AddMermaidBlockStart(diagramBuilder)")]
    public void ShouldAddMermaidBlockStart()
    {
        // Arrange: Create an empty StringBuilder to hold the Mermaid diagram
        var diagramBuilder = new StringBuilder();

        // Act: Call the helper method to add the Mermaid block start
        MermaidGeneratorCommonHelpers.AddMermaidBlockStart(diagramBuilder);

        // Assert: Verify that the StringBuilder contains the expected Mermaid block start and adds a new line
        string expected =
        $"""
        ```mermaid

        """;

        Assert.Equal(expected, diagramBuilder.ToString());

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

        Assert.Equal(expected, diagram.ToString());
    }

    [Fact(DisplayName = "AddDiagramFooter(diagramBuilder)")]
    public void ShouldAddDiagramFooter()
    {
        // Arrange: Create an empty StringBuilder to hold the Mermaid diagram
        var diagramBuilder = new StringBuilder();

        // Act: Call the helper method to add the diagram footer
        MermaidGeneratorCommonHelpers.AddDiagramFooter(diagramBuilder);

        // Assert: Verify that the StringBuilder contains the expected Mermaid footer
        string expected =
        $"""
        ```
        """;

        Assert.Equal(expected, diagramBuilder.ToString());
    }


}
