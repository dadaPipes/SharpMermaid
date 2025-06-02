using SharpMermaid.CLI.Commands;
using Spectre.Console.Cli;

var app = new CommandApp();

app.Configure(config =>
{
    config.AddCommand<PhysicalCommand>("physical")
        .WithDescription("Creates a project diagram that represents the physical structure of a solution on disk");
});

return app.Run(args);