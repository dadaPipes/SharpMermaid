namespace SharpMermaid.CLI.Infrastructure.ProcessExit.Types;
public readonly record struct PermissionDenied(string Path, string File) : IProcessExit
{
    public int Code => 77;
    public string FormatMessage() =>
        $"Error: Cannot write to '{Path}/{File}' - No write permission";
}
