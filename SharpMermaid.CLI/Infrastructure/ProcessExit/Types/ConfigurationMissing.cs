namespace SharpMermaid.CLI.Infrastructure.ProcessExit.Types;
public readonly record struct ConfigurationMissing(string FileName) : IProcessExit
{
    public int Code => 75;
    public string FormatMessage() =>
        $"Error: Required configuration file '{FileName}' is missing";
}
