namespace SharpMermaid;
public static class MermaidGenerator
{
    public static string ProjectNode(string projectName)
    {
        return
        $"""
        ```mermaid
        graph TD
          {projectName}
        ```
        """;
    }
}
