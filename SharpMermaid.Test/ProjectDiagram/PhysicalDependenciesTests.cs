namespace SharpMermaid.Test.ProjectDiagram;
public class PhysicalDependenciesTests
{
    [Fact]
    public void GenerateProjectNode_WithProjectName_ShouldReturnCorrectMermaidMarkdown()
    {
        // Arrange
        const string projectName = "MyProjectName";

        // Act
        var markdown = MermaidGenerator.ProjectNode(projectName);

        // Assert
        const string expected =
        """
        ```mermaid
        graph TD
          MyProjectName
        ```
        """;

        Assert.Equal(expected, markdown);
    }
}
