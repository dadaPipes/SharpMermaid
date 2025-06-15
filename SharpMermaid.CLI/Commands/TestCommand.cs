using Spectre.Console;
using Spectre.Console.Cli;

namespace SharpMermaid.CLI.Commands;
public class TestCommand(IAnsiConsole console) : Command<TestCommand.Settings>
{
    readonly IAnsiConsole _console = console;
    public class Settings : CommandSettings
    {
    }
    public override int Execute(CommandContext context, Settings settings)
    {
        _console.WriteLine("hello from test");
        return 0;
    }
}
