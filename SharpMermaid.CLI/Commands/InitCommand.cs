using Spectre.Console;
using Spectre.Console.Cli;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace SharpMermaid.CLI.Commands;

public class InitCommand : Command<InitCommand.Settings>
{
    private readonly IAnsiConsole _console;

    public InitCommand(IAnsiConsole console)
    {
        _console = console;
    }
    public class Settings : CommandSettings
    {
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        var cwd = Directory.GetCurrentDirectory();
        var configPath = Path.Combine(cwd, "mermaidconfig.json");

        if (File.Exists(configPath))
        {
            AnsiConsole.MarkupLine($"[red]Error:[/] A [bold]'mermaidconfig.json'[/] file already exists at '[underline]{configPath}[/]'");
            return 1; // Non-zero exit code signals failure
        }

        var defaultConfig = new
        {
            SolutionPath = "./YourSolution.sln",
            OutputDirectory = "./Diagrams",
            Diagrams = new[]
            {
                new
                {
                    DiagramType = "PhysicalProject",
                    FileName = "PhysicalDiagram",
                    FileType = ".mmd",
                    TopLevelPublicTypes = false,
                    ClassDiagramLinks = false,
                    BaseUrl = "https://example.com/"
                }
            }
        };

        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        var json = JsonSerializer.Serialize(defaultConfig, options);
        File.WriteAllTextAsync(configPath, json);

        _console.MarkupLine($"[green]Created new file[/] 'mermaidconfig.json' at '[underline]{cwd}[/]'");

        return 0;
    }
}
