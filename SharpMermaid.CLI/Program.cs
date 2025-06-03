using SharpMermaid.CLI.Commands;
using Spectre.Console.Cli;

var app = new CommandApp();

app.SetDefaultCommand<GenerateCommand>()
    .WithDescription("Generates Mermaid.js diagrams from the config");

app.Configure(config =>
{
    config.SetApplicationName("sharpmermaid");

    config.AddCommand<InitCommand>("init")
        .WithDescription("Creates an empty mermaidconfig.json at the cwd");

    config.AddCommand<GenerateCommand>("generate")
        .WithDescription("Creates an empty mermaidconfig.json at the cwd");
});

return app.Run(args);