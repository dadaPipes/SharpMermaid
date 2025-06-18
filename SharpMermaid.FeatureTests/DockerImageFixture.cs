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
