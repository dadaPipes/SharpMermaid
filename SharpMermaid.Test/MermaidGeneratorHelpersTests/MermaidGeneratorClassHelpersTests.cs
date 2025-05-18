using SharpMermaid.MermaidGeneratorHelpers;
using System.Text;

namespace SharpMermaid.Test.MermaidGeneratorHelpersTests;
public class MermaidGeneratorClassHelpersTests()
{
    [Fact(DisplayName = "AddClassDeclaration")]
    public void Should_Add_ClassDiagram_Declaration()
    {
        // Arrange: create an empty StringBuilder
        var diagram = new StringBuilder();

        // Act: add the classDiagram declaration
        MermaidGeneratorClassHelpers.AddClassDeclaration(diagram);

        // Assert: the output matches the expected Mermaid classDiagram format
        string expected =
        $"""
        classDiagram

        """;
        Assert.Equal(expected, diagram.ToString());
    }

    [Fact(DisplayName = "AddCsProjectAsTitle")]
    public void Should_Add_csProjName_As_Title()
    {
        // Arrange: create an empty StringBuilder
        var diagram = new StringBuilder();

        // Act: add the csProject name as title
        MermaidGeneratorClassHelpers.AddCsProjectAsTitle(diagram);

        // Assert: the output matches the expected Mermaid title block format and adds a new line
        string expected =
        $"""
        ---
        title: TestSolution
        ---

        """;

        Assert.Equal(expected, diagram.ToString());
    }
}
