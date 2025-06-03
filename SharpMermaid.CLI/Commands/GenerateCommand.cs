using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace SharpMermaid.CLI.Commands;
public class GenerateCommand : Command<GenerateCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandOption("--solution <SOLUTION_PATH>")]
        [Description("Path to the solution file.")]
        public string Solution { get; set; }

        
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        AnsiConsole.MarkupLine("[green]Generating diagrams...[/]");

        AnsiConsole.MarkupLine($"Solution: [yellow]{settings.Solution}[/]");

        // TODO: call your diagram generation logic here

        AnsiConsole.MarkupLine("[green]Done![/]");

        return 0;
    }
}
