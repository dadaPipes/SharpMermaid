using SharpMermaid.TestHelpers;
using Xunit;

namespace SharpMermaid.FeatureTests;
public class GenerateClassDiagramTests(ITestOutputHelper output)
{
    /*
    private readonly ITestOutputHelper _output = output;

    [Fact(DisplayName = "Generating Diagrams for Multiple Projects")]
    public void Should_Generate_Class_Diagram_With_Single_Class()
    {
        // Given a solution containing multiple projects
        // And each project contains multiple `.cs` files, including files within nested directories
        using var solution = new TemporarySolutionBuilder();

        solution.AddProjectWithFiles("ProjectA", new Dictionary<string, string>
        {
            ["X.cs"] = "public class X {}",
            ["Y.cs"] = "public class Y {}",
            ["Z.cs"] = "public class Z {}"
        });

        solution.AddProjectWithFiles("ProjectB", new Dictionary<string, string>
        {
            ["E.cs"] = "public class E {}",
            ["F.cs"] = "public class F {}",
            ["FolderOne/G.cs"] = "public class G {}"
        });

        solution.AddProjectWithFiles("ProjectC", new Dictionary<string, string>
        {
            ["L.cs"] = "public class L {}",
            ["FolderOne/M.cs"] = "public class M {}",
            ["FolderOne/FolderTwo/N.cs"] = "public class N {}"
        });

        Settings.BaseUrl = "https://example.com/";

        // When I generate the diagrams
        var generator = new ClassDiagramGenerator(solution.FullPath);
        var diagrams = generator.GenerateClassDiagrams();

        // Then a separate class diagram should be created for each project  
        // And each diagram should have a title matching the project name  
        // And each class node should be named after its .cs file  
        // And each class node should have a clickable URL reflecting its full directory structure, including nested folders
        var expectedClassDiagrams = new Dictionary<string, string>
        {
            ["ProjectA"] =
            $"""
            ```mermaid
            ---
            title: ProjectA
            ---
            classDiagram
                class X
                class Y
                class Z
                click X href "https://example.com/ProjectA/X.cs"
                click Y href "https://example.com/ProjectA/Y.cs"
                click Z href "https://example.com/ProjectA/Z.cs"
            ```
            """,

            ["ProjectB"] =
            $"""
            ```mermaid
            ---
            title: ProjectB
            ---
            classDiagram
                class E
                class F
                class G
                click E href "https://example.com/ProjectB/E.cs"
                click F href "https://example.com/ProjectB/F.cs"
                click G href "https://example.com/ProjectB/FolderOne/G.cs"
            ```
            """,

            ["ProjectC"] =
            $"""
            ```mermaid
            ---
            title: ProjectC
            ---
            classDiagram
                class L
                class M
                class N
                click L href "https://example.com/ProjectC/L.cs"
                click M href "https://example.com/ProjectC/FolderOne/M.cs"
                click N href "https://example.com/ProjectC/FolderOne/FolderTwo/N.cs"
            ```
            """
        };

        foreach (var csproject in expectedClassDiagrams.Keys)
        {
            _output.WriteLine($"Checking {csproject}");
            _output.WriteLine("Expected:\n" + expectedClassDiagrams[csproject]);
            _output.WriteLine("Actual:\n" + diagrams[csproject]);

            Assert.Equal(expectedClassDiagrams[csproject], diagrams[csproject]);
        }
    }
    */
}
