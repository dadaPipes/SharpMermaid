namespace SharpMermaid.CLI.ProcessExit;
internal class ExitCodeDescriptor(ExitCode exitCode, string template)
{
    public ExitCode ExitCode { get; } = exitCode;
    public string Template { get; } = template;

    public string Format(params object[] args) => string.Format(Template, args);
}
