namespace SharpMermaid.RulesTests;
public class SaveDiagramTests
{
    [Fact(DisplayName = "Rule 1: Saves diagram as a `.md` file containing a Mermaid.js code block")]
    public void ShouldSaveDiagramAsMarkdownFile()
    {
        throw new NotImplementedException();
    }

    [Fact(DisplayName = "Rule 2: Always overwrites existing file")]
    public void ShouldOverrideExistingFile()
    {
        throw new NotImplementedException();
    }

    [Fact(DisplayName = "Rule 3: Saves diagram in execution directory when no custom path is specified")]
    public void ShouldSaveDiagramToExecutionDirectory_WhenNoCustomPathSpecified()
    {
        throw new NotImplementedException();
    }

    [Fact(DisplayName = "Rule 3.a: Saves diagram to specified custom path when provided")]
    public void ShouldSaveDiagramToCustomPath_WhenCustomPathIsSpecified()
    {
        throw new NotImplementedException();
    }

    [Fact(DisplayName = "Rule 4: Saves diagram with default filename `PhysicalProjectDiagram.md` when no custom filename is provided")]
    public void ShouldSaveDiagramWithDefaultFileName_WhenNoCustomNameSpecified()
    {
        throw new NotImplementedException();
    }

    [Fact(DisplayName = "Rule 4.a: Saves diagram with specified custom filename when provided")]
    public void ShouldSaveDiagramWithCustomFileName_WhenCustomNameSpecified()
    {
        throw new NotImplementedException();
    }

    [Fact(DisplayName = "Rule 5: Console displays a confirmation message showing saved file location")]
    public void ShouldDisplayConfirmationMessage_WhenDiagramIsSaved()
    {
        throw new NotImplementedException();
    }
}
