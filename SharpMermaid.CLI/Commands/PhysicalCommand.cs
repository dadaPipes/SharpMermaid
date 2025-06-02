using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace SharpMermaid.CLI.Features.CreateProjectPhysical;
public class CreateProjectPhysicalCommand : Command<CreateProjectPhysicalCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandOption("--solution <SOLUTION_PATH>")]
        [Description("Path to the solution file.")]
        public string Solution { get; set; }

        [CommandOption("--output <OUTPUT_DIRECTORY>")]
        [Description("Directory to output diagrams.")]
        public string Output { get; set; }
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        AnsiConsole.MarkupLine("[green]Generating diagrams...[/]");

        AnsiConsole.MarkupLine($"Solution: [yellow]{settings.Solution}[/]");
        AnsiConsole.MarkupLine($"Output Directory: [yellow]{settings.Output}[/]");

        // TODO: call your diagram generation logic here

        AnsiConsole.MarkupLine("[green]Done![/]");

        return 0;
    }
}
