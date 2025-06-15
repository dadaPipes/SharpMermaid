using DotNet.Testcontainers.Builders;

namespace SharpMermaid.FeatureTests;

public class InitTests(ITestOutputHelper outputHelper)
{
    readonly ITestOutputHelper _outputHelper = outputHelper;

    [Fact(DisplayName = "Scenario: Display success message after sharpmermaidconfig.json creation")]
    public async Task Should_Display_Success_Message_And_Exit_0_When_Config_Created()
    {
        // Arrange
        var container = new ContainerBuilder()
            .WithImage("sharpmermaidcli")
            .WithCommand("init")
            .WithWaitStrategy(
                Wait
                .ForUnixContainer()
                .AddCustomWaitStrategy(new WaitUntilProcessExits()))
            .Build();

        await container.StartAsync(TestContext.Current.CancellationToken);

        try
        {
            // Act
            var (Stdout, _) = await container.GetLogsAsync(ct: TestContext.Current.CancellationToken);
            var exitCode = await container.GetExitCodeAsync(TestContext.Current.CancellationToken);

            _outputHelper.WriteLine($"Stdout: {Stdout}");
            _outputHelper.WriteLine($"Exit-code: {exitCode}");

            // Assert
            Assert.Equal(0, exitCode);
            Assert.Contains("Hello from init", Stdout);
        }
        finally
        {
            // Ensure the container is stopped, even if an assertion or exception occurs.
            await container.StopAsync(TestContext.Current.CancellationToken);
        }
    }

    /*
    [Fact(DisplayName = "Scenario: Create default sharpmermaidconfig.json")]
    public void Should_Create_default_sharpmermaidconfig()
    {
        // Given the Developers working directory is {cwd}
        using var cwd = new TempDirectory();
        Directory.SetCurrentDirectory(cwd.Path);

        // And no sharpmermaidconfig.json exists in {cwd}
        var configPath = Path.Combine(cwd.Path, "sharpmermaidconfig.json");
        Assert.False(File.Exists(configPath));

        // When the Developer runs: dotnet sharpmermaid init
        var app = new CommandAppTester();
        app.SetDefaultCommand<InitCommand>();

        app.Run();

        // Then a file named sharpmermaidconfig.json must exist in {cwd}
        Assert.True(File.Exists(configPath));

        // And its content must match the default sharpmermaidconfig.json:    
        var expected = sharpmermaidconfig;
        var actual = File.ReadAllText(configPath);
        
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        static string NormalizeLineEndings(string text) => text.Replace("\r\n", "\n").Trim();

        Assert.Equal(
        NormalizeLineEndings(expected),
        NormalizeLineEndings(actual)
        );
    }
    */
    /*
    [Fact(DisplayName = "Scenario: Display error message if sharpmermaidconfig.json already exists")]
    public void Should_Display_Error_And_Exit_76_When_Config_Already_Exists()
    {
        // Given the Developers working directory is {cwd}
        using var cwd = new TempDirectory();
        Directory.SetCurrentDirectory(cwd.Path);

        // And a file named sharpmermaidconfig.json already exists in { cwd}
        var configPath = Path.Combine(cwd.Path, "sharpmermaidconfig.json");
        File.WriteAllText(configPath, "existing content");

        // When the Developer runs: dotnet sharpmermaid init
        var app = new CommandAppTester();
        app.SetDefaultCommand<InitCommand>();

        var result = app.Run();

        // Then the system must display: "Error: A 'sharpmermaidconfig.json' file already exists at '{cwd}/sharpmermaidconfig.json'"
        var expected = $"Error: A 'sharpmermaidconfig.json' file already exists at '{cwd.Path}'";
        var actual = result.Output;

        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.Equal(expected, actual);

        // And exit with code 73
        Assert.Equal(73, result.ExitCode);
    }
    */
}
