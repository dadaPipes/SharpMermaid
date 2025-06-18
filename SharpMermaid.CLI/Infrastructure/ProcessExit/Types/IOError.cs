namespace SharpMermaid.CLI.Infrastructure.ProcessExit.Types;
public readonly record struct IOError(string Path, string File, string ErrorMessage) : IProcessExit
{
    public int Code => 74;
    public string FormatMessage() =>
        $"Error: Failed to write file at '{Path}/{File}': {ErrorMessage}";
}
