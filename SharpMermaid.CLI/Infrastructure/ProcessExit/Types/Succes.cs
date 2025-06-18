namespace SharpMermaid.CLI.Infrastructure.ProcessExit.Types;
public readonly record struct Success(string SuccessMessage) : IProcessExit
{
    public int Code => 0;
    public string FormatMessage() => $"Success: {SuccessMessage}";
}
