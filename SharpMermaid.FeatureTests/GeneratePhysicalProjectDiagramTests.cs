using SharpMermaid.TestHelpers;
using System.Diagnostics;
using Xunit.Abstractions;

namespace SharpMermaid.FeatureTests;
public class GeneratePhysicalProjectDiagramTests(ITestOutputHelper output)
{
    private readonly ITestOutputHelper _output = output;

    [Fact(DisplayName = "Valid .mmd file")]
    public void Should_CreateValidMmdFile()
    {
        // Given the developer’s current working directory is {cwd}
        var cwd = Directory.GetCurrentDirectory();
        _output.WriteLine($"Current working directory: {cwd}");

        // And a solution file TestSolution.sln exists at {cwd}/TestSolution.sln
        using TemporarySolutionBuilder solution = new("TestSolution", cwd);

        // And ./TestSolution contains
        solution.AddProjectWithFiles("ProjectA", new Dictionary<string, string>
        {
            ["ExampleA.cs"]  = "public class ExampleA {}",
            ["IExampleA.cs"] = "public interface IExampleA {}"
        });

        solution.AddProjectWithFiles("ProjectB", new Dictionary<string, string>
        {
            ["ExamplePrivate.cs"]         = "private class ExampleDefaultInternal {}",
            ["ExampleDefaultInternal.cs"] = "class ExampleDefaultInternal {}",
            ["ExampleInternal.cs"]        = "class ExampleInternal {}"
        });

        // And `ProjectA` has a reference to `ProjectB`
        solution.AddProjectReference("ProjectA", "ProjectB");

        // And a `sharpmermaidconfig.json` file exist in the solution
        string configPath = Path.Combine(cwd, "sharpmermaidconfig.json");
        const string configContent =
        """
        {
          "SolutionPath": "./TestSolution.sln",
          "OutputDirectory": "./Diagrams",
          "Diagrams": [
            {
              "DiagramType": "PhysicalProject",
              "FileName": "PhysicalDiagram",
              "FileType": ".mmd",
              "TopLevelPublicTypes": true,
              "ClassDiagramLinks": true,
              "BaseUrl": "https://example.com/"
            }
          ]
        }
        """;
        File.WriteAllText(configPath, configContent);

        // When the developer runs: dotnet sharpmermaid generate
        var processStartInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"dotnet sharpmermaid generate",
            WorkingDirectory = cwd,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(processStartInfo);
        var outputText = process!.StandardOutput.ReadToEnd();
        var errorText = process.StandardError.ReadToEnd();
        process.WaitForExit();

        _output.WriteLine("Output:\n" + outputText);
        _output.WriteLine("Error:\n" + errorText);

        // Then the generated file must be created at `{cwd}/Diagrams/PhysicalDiagram.mmd`
        var mermaidFilePath = Path.Combine(cwd, "Diagrams/PhysicalDiagram.mmd");
        Assert.True(File.Exists(mermaidFilePath), $"Expected {mermaidFilePath} to be created.");

        // And the console must display: Created new file 'mermaid.md' at '{cwd}/Diagrams/PhysicalDiagram.mmd'
        Assert.Contains($"Created new file 'mermaid.md' at '{mermaidFilePath}'", outputText);

        // And the file must imclude
        string expected =
        $"""
         ---
         title: TestSolution
         ---
         graph
             ProjectA["**ProjectA**
                 Example"
                 IExample]
             ProjectB
             ProjectA --> ProjectB
             click ProjectA "https://example.com/ProjectA/ProjectA.csproj"
             click ProjectB "https://example.com/Folder1/ProjectB/ProjectB.csproj"
         """;

        var diagram = File.ReadAllText(mermaidFilePath);

        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + diagram);

        Assert.Equal(expected, diagram);
    }
    /*
    [Fact(DisplayName = "Solution Without Projects")]
    public void Should_Generate_Diagram_With_No_Projects()
    {
        // Given the solution has no projects
        using TemporarySolutionBuilder solution = new("TestSolution");

        // When the diagram is generated
        var generator = new PhysicalProjectDiagramGenerator(solution.FullPath);
        var diagram = generator.GeneratePhysicalProjectDiagram();

        // Then the diagram should have no nodes or dependencies
        // And the title should be the solution name
        string expected =
        $"""
        ```mermaid
        ---
        title: {solution.Name}
        ---
        graph
        ```
        """;

        // Log expected and actual values for debugging
        string actual = diagram;
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.Contains("```mermaid", diagram);
        Assert.Contains("graph", diagram);
        Assert.Equal(expected, diagram);
    }

    [Fact(DisplayName = "Solution With Single Project")]
    public void Should_Generate_Diagram_With_Single_Project()
    {
        // Given the solution has a single project
        // And the project has at least one source file
        using var solution = new TemporarySolutionBuilder();

        solution.AddProjectWithFiles("ProjectA", new Dictionary<string, string>
        {
            ["X.cs"] = "public class X {}"
        });

        Settings.BaseUrl = "https://example.com/";

        // When the diagram is generated
        var generator = new PhysicalProjectDiagramGenerator(solution.FullPath);
        var diagram = generator.GeneratePhysicalProjectDiagram();

        // Then the diagram should include one node
        // And the diagram should have a title same as the solution name
        // And the diagram should include a node for the project in the solution
        // And the diagram should include a url to the projects class diagram
        string expected =
        $"""
        ```mermaid
        ---
        title: {solution.Name}
        ---
        graph
            ProjectA
            click ProjectA "https://example.com/ProjectA/ProjectA.csproj"
        ```
        """;

        // Log expected and actual values for debugging
        string actual = diagram;
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.Equal(expected, diagram);
    }

    [Fact(DisplayName = "Multiple Projects Without Dependencies")]
    public void Should_Generate_Diagram_With_Multiple_Projects_And_No_Dependencies()
    {
        // Given the solution has multiple projects with no dependencies
        // And each project has at least one source file
        using var solution = new TemporarySolutionBuilder();

        solution.AddProjectWithFiles("ProjectA", new Dictionary<string, string>
        {
            ["X.cs"] = "public class X {}"
        });

        solution.AddProjectWithFiles("ProjectB", new Dictionary<string, string>
        {
            ["Y.cs"] = "public class Y {}"
        });

        solution.AddProjectWithFiles("ProjectC", new Dictionary<string, string>
        {
            ["Z.cs"] = "public class Z {}"
        });

        Settings.BaseUrl = "https://example.com/";

        // When the diagram is generated
        var generator = new PhysicalProjectDiagramGenerator(solution.FullPath);
        var diagram   = generator.GeneratePhysicalProjectDiagram();

        // Then the diagram should include one node per project
        // And the diagram should have a title same as the solution name
        // And the diagram should include a url to the projects class diagram
        string expected =
        $"""
        ```mermaid
        ---
        title: {solution.Name}
        ---
        graph
            ProjectA
            ProjectB
            ProjectC
            click ProjectA "https://example.com/ProjectA/ProjectA.csproj"
            click ProjectB "https://example.com/ProjectB/ProjectB.csproj"
            click ProjectC "https://example.com/ProjectC/ProjectC.csproj"
        ```
        """;

        // Log expected and actual values for debugging
        string actual = diagram;
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.Equal(expected, diagram);
    }

    [Fact(DisplayName = "Multiple Projects With Dependencies")]
    public void Should_Generate_Diagram_With_Multiple_Projects_With_Dependencies()
    {
        // Given the solution has multiple projects with dependencies
        // And each project has at least one source file
        using var solution = new TemporarySolutionBuilder();
        var projectA = solution.AddProjectWithFiles("ProjectA", new Dictionary<string, string>
        {
            ["X.cs"] = "public class X {}"
        });
        var projectB = solution.AddProjectWithFiles("ProjectB", new Dictionary<string, string>
        {
            ["Y.cs"] = "public class Y {}"
        });
        var projectC = solution.AddProjectWithFiles("ProjectC", new Dictionary<string, string>
        {
            ["Z.cs"] = "public class Z {}"
        });
        solution.AddProjectReference(projectA, projectB);
        solution.AddProjectReference(projectA, projectC);
        solution.AddProjectReference(projectB, projectC);

        Settings.BaseUrl = "https://example.com/";

        // When the diagram is generated
        var generator = new PhysicalProjectDiagramGenerator(solution.FullPath);
        var diagram   = generator.GeneratePhysicalProjectDiagram();

        // Then the diagram should include one node per project
        // And arrows should represent the dependencies between project nodes
        // And each should include a url to the projects class diagram
        // And the diagram should have a title same as the solution name
        // And the diagram should include a url to the projects class diagram
        string expected =
        $"""
        ```mermaid
        ---
        title: {solution.Name}
        ---
        graph
            ProjectA
            ProjectB
            ProjectC
            click ProjectA "https://example.com/ProjectA/ProjectA.csproj"
            click ProjectB "https://example.com/ProjectB/ProjectB.csproj"
            click ProjectC "https://example.com/ProjectC/ProjectC.csproj"
            ProjectA --> ProjectB
            ProjectA --> ProjectC
            ProjectB --> ProjectC
        ```
        """;
        // Log expected and actual values for debugging
        string actual = diagram;
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);
        Assert.Equal(expected, diagram);
    }*/
}
