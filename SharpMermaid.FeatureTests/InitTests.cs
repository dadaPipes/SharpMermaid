using DotNet.Testcontainers.Builders;
using SharpMermaid.FeatureTests.Helpers;
using System.Text;

namespace SharpMermaid.FeatureTests;

/// <summary>
/// Feature tests for the `dotnet sharpmermaid init` command.
/// Each test spins up a CLI container using Testcontainers to verify expected behavior
/// in realistic isolated environments.
/// </summary>
public class InitTests(ITestOutputHelper outputHelper)
{
    readonly ITestOutputHelper _outputHelper = outputHelper;

    [Fact(DisplayName = "Scenario: Display success message after sharpmermaidconfig.json creation")]
    [Trait("Feature", "Init")]
    public async Task Should_Display_Success_Message_And_Exit_0_When_Config_Created()
    {
        // Given the developer is in a writable working directory  
        // And no sharpmermaidconfig.json exists in that directory
        // When they run: dotnet sharpmermaid init
        var container = new ContainerBuilder()
            .WithImage("sharpmermaidcli")
            .WithCommand("init")
            .WithWaitStrategy(
                Wait
                .ForUnixContainer()
                .AddCustomWaitStrategy(new WaitUntilProcessExits()))
            .Build();

        await container.StartAsync(TestContext.Current.CancellationToken);


        // Then the console must display: "Created new file 'sharpmermaidconfig.json' at the working directory"
        var (actualOutput, _) = await container.GetLogsAsync(ct: TestContext.Current.CancellationToken);
        const string expectedOutput = "Created new file 'sharpmermaidconfig.json' at '/app'";

        _outputHelper.WriteLine($"Expected console output: {expectedOutput}");
        _outputHelper.WriteLine($"Actual console output: {actualOutput}");

        Assert.Contains(expectedOutput, actualOutput);

        // And exit with code 0
        var exitCode = await container.GetExitCodeAsync(TestContext.Current.CancellationToken);

        Assert.Equal(0, exitCode);
    }

    [Fact(DisplayName = "Scenario: Create default sharpmermaidconfig.json")]
    [Trait("Feature", "Init")]
    public async Task Should_Create_default_sharpmermaidconfig()
    {
        // Given the developer is in a writable working directory  
        // And no sharpmermaidconfig.json exists in that directory
        // When they run: dotnet sharpmermaid init
        var container = new ContainerBuilder()
            .WithImage("sharpmermaidcli")
            .WithCommand("init")
            .WithWaitStrategy(
                Wait
                .ForUnixContainer()
                .AddCustomWaitStrategy(new WaitUntilProcessExits()))
            .Build();

        await container.StartAsync(TestContext.Current.CancellationToken);

        // Then a file named sharpmermaidconfig.json must exist in the working directory
        var fileBytes = await container.ReadFileAsync("/app/sharpmermaidconfig.json", TestContext.Current.CancellationToken);
        Assert.NotNull(fileBytes);
        Assert.NotEmpty(fileBytes);

        // And its content must match the default sharpmermaidconfig.json:
        var fileContent = Encoding.UTF8.GetString(fileBytes);

        const string expectedJson = """
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

        _outputHelper.WriteLine($"Expected sharpmermaid.json: {Normalize(expectedJson)}");
        _outputHelper.WriteLine($"Actual sharpmermaid.json: {Normalize(fileContent)}");

        Assert.Equal(Normalize(expectedJson), Normalize(fileContent));

        static string Normalize(string s) => s.Replace("\r\n", "\n").Trim();
    }

    /// <summary>
    /// The Resources/sharpmermaidconfig.json file is included in the test project and copied to the output directory during build,
    /// as configured in the SharpMermaid.FeatureTests.csproj file.
    /// This allows the test to reference the file by relative path when using
    /// <see cref="ContainerBuilder.WithResourceMapping(FileInfo, string)"/> to map it into the container.
    /// </summary>
    [Fact(DisplayName = "Scenario: Display error message if sharpmermaidconfig.json already exists")]
    [Trait("Feature", "Init")]
    public async Task Should_Display_Error_And_Exit_76_When_Config_Already_Exists()
    {
        // Given the developer is in a writable working directory  
        // And a sharpmermaidconfig.json already exists in that directory
        // When they run: dotnet sharpmermaid init

        var container = new ContainerBuilder()
            .WithImage("sharpmermaidcli")
            .WithCommand("init")
            .WithResourceMapping(new FileInfo("Resources/sharpmermaidconfig.json"), "app/")
            .WithWaitStrategy(
                Wait
                .ForUnixContainer()
                .AddCustomWaitStrategy(new WaitUntilProcessExits()))
            .Build();

        await container.StartAsync(TestContext.Current.CancellationToken);

        // Then the system must display: "Error: A 'sharpmermaidconfig.json' file already exists at '{cwd}/sharpmermaidconfig.json'"
        var (rawOutput, _) = await container.GetLogsAsync(ct: TestContext.Current.CancellationToken);
        var actual = TestOutputNormalizer.NormalizeContainerOutput(rawOutput);

        const string expectedOutput = "Error: A 'sharpmermaidconfig.json' file already exists at '/app/sharpmermaidconfig.json'";

        _outputHelper.WriteLine($"Expected console output: {expectedOutput}");
        _outputHelper.WriteLine($"Actual console output: {actual}");

        Assert.Contains(expectedOutput, actual);

        // And exit with code 73
        var exitCode = await container.GetExitCodeAsync(TestContext.Current.CancellationToken);

        Assert.Equal(73, exitCode);
    }
}
