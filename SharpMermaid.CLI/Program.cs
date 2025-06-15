using Microsoft.Extensions.DependencyInjection;
using SharpMermaid.CLI.Commands;
using SharpMermaid.CLI.Infrastructure;
using Spectre.Console.Cli;

var registrations = new ServiceCollection();

var registrar = new MyTypeRegistrar(registrations);

var app = new CommandApp(registrar);

app.Configure(config =>
{
    config.SetApplicationName("sharpmermaid");

    config.AddCommand<TestCommand>("test");

    config.AddCommand<InitCommand>("init")
        .WithDescription("Creates an empty sharpmermaidconfig.json at the cwd");

    config.AddCommand<GenerateCommand>("generate")
        .WithDescription("Generates Mermaid.js diagrams from the sharpmermiadconfig.json");
});

return app.Run(args);