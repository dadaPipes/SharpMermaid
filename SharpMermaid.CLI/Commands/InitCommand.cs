using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.IO.Abstractions;

namespace SharpMermaid.CLI.Commands;

public class InitCommand(IAnsiConsole console) : Command<InitCommand.Settings>
{
    readonly IAnsiConsole _console = console;

    public class Settings : CommandSettings
    {
        [CommandOption("-d|--directory <DIRECTORY>")]
        [Description("Optional working directory. If not specified, the current working directory is used.")]
        public string? WorkingDirectory { get; set; }
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        _console.WriteLine("Hello from init");
        return 0;
        /*
        // Use the provided directory if given, otherwise fall back to the current working directory.
        var directory = string.IsNullOrWhiteSpace(settings.WorkingDirectory)
            ? _fileSystem.Directory.GetCurrentDirectory()
            : settings.WorkingDirectory;

        var configPath = Path.Combine(directory, "sharpmermaidconfig.json");

        if (_fileSystem.File.Exists(configPath))
        {
            _console.MarkupLine($"Error: A 'sharpmermaidconfig.json' file already exists at '{directory}'");
            return 73;
        }
        
        _fileSystem.File.WriteAllText(configPath, defaultConfig);
        _console.WriteLine($"Created new file 'sharpmermaidconfig.json' at '{directory}'");
        return 0;
    }
    
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
        """;*/
    }
}
