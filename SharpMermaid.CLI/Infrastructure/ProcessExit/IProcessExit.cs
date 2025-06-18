namespace SharpMermaid.CLI.Infrastructure.ProcessExit;
public interface IProcessExit
{
    int Code { get; }
    string FormatMessage();
}
