namespace SharpMermaid.CLI.Infrastructure.ProcessExit.Types;
public readonly record struct GeneralError(string ErrorMessage) : IProcessExit
{
    public int Code => 1;
    public string FormatMessage() => $"Error: An unexpected failure occurred - {ErrorMessage}";
}
