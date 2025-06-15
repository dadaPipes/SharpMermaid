using DotNet.Testcontainers.Builders;

[assembly: AssemblyFixture(typeof(SharpMermaid.FeatureTests.DockerImageFixture))]

namespace SharpMermaid.FeatureTests;
public sealed class DockerImageFixture : IAsyncLifetime
{
    public async ValueTask InitializeAsync()
    {
        var sharpmermaidcli = new ImageFromDockerfileBuilder()
        .WithName("sharpmermaidcli")
        .WithDockerfileDirectory(CommonDirectoryPath.GetSolutionDirectory(), string.Empty)
        .WithDockerfile("SharpMermaid.CLI/Dockerfile")
        .Build();

        await sharpmermaidcli.CreateAsync().ConfigureAwait(false);
    }
    public async ValueTask DisposeAsync() => await ValueTask.CompletedTask;
}
/*
public string ImageName { get; } = "sharpmermaidcli";
public async ValueTask InitializeAsync()
{
    string solutionRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));
    // Run "docker build -t sharpmermaidcli -f SharpMermaid.CLI/Dockerfile ."
    var psi = new ProcessStartInfo("docker", $"build -t {ImageName} -f SharpMermaid.CLI/Dockerfile .")
    {
        WorkingDirectory = solutionRoot,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
        CreateNoWindow = true,
    };

    using var process = Process.Start(psi)
        ?? throw new InvalidOperationException("Failed to start docker build process.");

    string output = await process.StandardOutput.ReadToEndAsync();
    string error = await process.StandardError.ReadToEndAsync();

    await process.WaitForExitAsync();

    if (process.ExitCode != 0)
    {
        var root = Directory.GetDirectoryRoot(Directory.GetCurrentDirectory());
        var cwd = Directory.GetCurrentDirectory();

        throw new InvalidOperationException(
            $"""
            Docker build failed (exit code {process.ExitCode}): {error}
            CWD: {cwd}
            Root: {root}
            """);
    }

    Console.WriteLine("Docker build output:");
    Console.WriteLine(output);
}

public async ValueTask DisposeAsync() => await ValueTask.CompletedTask;

}
*/
