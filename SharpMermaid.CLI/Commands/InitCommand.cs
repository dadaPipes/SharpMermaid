using SharpMermaid.CLI.Infrastructure.ProcessExit;
using Spectre.Console;
using Spectre.Console.Cli;

namespace SharpMermaid.CLI.Commands;

public class InitCommand(IAnsiConsole console) : Command<InitCommand.Settings>
{
    readonly IAnsiConsole _console = console;

    public class Settings : CommandSettings
    {
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        var cwd = Directory.GetCurrentDirectory();

        var filePath = Path.Combine(cwd, "sharpmermaidconfig.json");

        if (File.Exists(filePath))
        {
            return Exit.With(
                ExitTypes.FileAlreadyExists(cwd, "sharpmermaidconfig.json"),
                _console);
        }
        
        // refactor below to use the beautifull "Exit.With()"
        File.WriteAllText(filePath, defaultConfig);
        _console.WriteLine($"Created new file 'sharpmermaidconfig.json' at '{cwd}'");
        return 0;
    }

    // try catch: write access ? throw exception or custom error code ? 
    
    private const string defaultConfig = """
        {
            "SolutionPath": "./TestSolution.sln",
            "OutputDirectory": "./Diagrams",
            "FileType": ".mmd",
            "Diagrams": [
            {
                "PhysicalProject": {
                "OutputDirectory": "./Override/Diagrams",
                "FileName": "PhysicalDiagram",
                "FileType": ".mmd",
                "TopLevelPublicTypes": true,
                "ClassDiagramLinks": true,
                "BaseUrl": "https://example.com/"
                }
            }
            ]
        }
        """;
}