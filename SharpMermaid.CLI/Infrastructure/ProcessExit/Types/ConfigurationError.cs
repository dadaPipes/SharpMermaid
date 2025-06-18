namespace SharpMermaid.CLI.Infrastructure.ProcessExit.Types;
public readonly record struct ConfigurationError(string Path, string File, string ValidationMessage) : IProcessExit
{
    public int Code => 76;
    public string FormatMessage() =>
        $"Error: Invalid configuration in '{Path}/{File}' - {ValidationMessage}";
}
