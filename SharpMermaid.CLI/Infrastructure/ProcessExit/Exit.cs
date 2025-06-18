using Spectre.Console;

namespace SharpMermaid.CLI.Infrastructure.ProcessExit;

public static class Exit
{
    public static int With<T>(T result, IAnsiConsole console) where T : IProcessExit
    {
        var color = result.Code == 0 ? "green" : "red";
        console.MarkupLine($"[{color}]{result.FormatMessage()}[/]");
        return result.Code;
    }
}
