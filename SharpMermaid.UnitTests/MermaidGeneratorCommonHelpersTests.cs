using SharpMermaid.MermaidGeneratorHelpers;
using System.Text;
using Xunit.Abstractions;

namespace SharpMermaid.UnitTests;
public class MermaidGeneratorCommonHelpersTests(ITestOutputHelper output)
{
    private readonly ITestOutputHelper _output = output;

    [Fact]
    public void ShouldAddMermaidBlockStart()
    {
        // Arrange:
        var diagram = new StringBuilder();

        // Act:
        MermaidGeneratorCommonHelpers.AddMermaidBlockStart(diagram);

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

    [Fact]
    public void ShouldAddSolutionNameAsTitle()
    {
        // Arrange:
        var diagram = new StringBuilder();

        // Act:
        MermaidGeneratorCommonHelpers.AddSolutionNameAsTitle("TestSolution", diagram);

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

    [Fact]
    public void ShouldAddDiagramFooter()
    {
        // Arrange:
        var diagram = new StringBuilder();

        // Act:
        MermaidGeneratorCommonHelpers.AddDiagramFooter(diagram);

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


}
