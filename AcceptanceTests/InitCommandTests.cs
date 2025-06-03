using AcceptanceTests.TestHelpers;
using SharpMermaid.CLI.Commands;
using Spectre.Console.Cli;
using Spectre.Console.Testing;

namespace AcceptanceTests;

public class InitCommandTests
{
    [Fact(DisplayName = "Scenario: Create Default Config File")]
    public void Should_Create_Default_Config_And_Display_Message()
    {
        // --- Given the developer’s current working directory is {cwd}
        using var tempDir = new TempDirectory();
        Directory.SetCurrentDirectory(tempDir.Path);

        // --- And no `mermaidconfig.json` exists at {cwd}
        var configPath = Path.Combine(tempDir.Path, "mermaidconfig.json");
        Assert.False(File.Exists(configPath));

        var console = new TestConsole();
        var app = new CommandApp<InitCommand>();
        app.Configure(config => config.SetApplicationName("sharpmermaid"));

        // --- When the developer runs `sharpmermaid init`
        var exitCode = app.Run(new[] { "init" }, console);

        // --- Then the system must create a new `mermaidconfig.json` file at {cwd}
        Assert.True(File.Exists(configPath));

        // --- And the console must display: 
        // "Created new file 'mermaidconfig.json' at '{cwd}'"
        var output = console.Output.Trim();
        var expectedMessage = $"Created new file 'mermaidconfig.json' at '{tempDir.Path.Replace("\\", "/")}'";
        Assert.Contains(expectedMessage, output);

        Assert.Equal(0, exitCode);
    }
}
