using SharpMermaid.TestHelpers;
using System.Text;
using Xunit.Abstractions;
using SharpMermaid.Models;
using SharpMermaid.Features.GenerateClassDiagrams;

namespace SharpMermaid.RulesTests;
public class GenerateClassDiagramsTests(ITestOutputHelper output)
{
    /*
    private readonly ITestOutputHelper _output = output;

    [Fact(DisplayName = "Each diagram **must** start with a `classDiagram` declaration")]
    public void Should_Add_ClassDiagram_Declaration_To_Diagram()
    {
        // Arrange:
        var diagram = new StringBuilder();

        // Act:
        Rules.AddClassDeclaration(diagram);

        // Assert:
        string expected =
        $"""
        classDiagram

        """;

        string actual = diagram.ToString();
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.Equal(expected, diagram.ToString());
    }

    [Fact(DisplayName = "Each generated diagram **must** have a title matching the project name")]
    public void Should_Add_csProjName_As_Title_To_Diagram()
    {
        // Arrange:
        using var solution = new TemporarySolutionBuilder();
        solution.AddProject("ProjectA");

        var diagram = new StringBuilder();

        // Act:
        Rules.AddCsProjectAsTitle("ProjectA", diagram);

        // Assert:
        string expected =
        $"""
        ---
        title: ProjectA
        ---

        """;

        string actual = diagram.ToString();
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.Equal(expected, diagram.ToString());
    }

    [Fact(DisplayName = "Each class must be represented with the name of the `.cs` file as the node name")]
    public void Should_Add_CsFileNames_To_Diagram()
    {
        // Arrange:
        using var solution = new TemporarySolutionBuilder();
        var projectA = solution.AddProjectWithFiles(
            "ProjectA",
            new Dictionary<string, string>
            {
                ["1.cs"] = "public class X",
                ["2.cs"] = "public class Y",
                ["3.cs"] = "public class Z"
            });

        List<CsprojModel> projectFiles = [
            new ("ProjectA", Path.GetFullPath(projectA), Path.GetRelativePath(solution.Directory, projectA))
            ];

        var csFiles = projectFiles.SelectMany(p => p.CsFiles).ToList();

        var diagram = new StringBuilder();

        // Act:
        Rules.AddCsFileNames(csFiles, diagram);

        // Assert:
        string expected =
        $"""
            class 1
            class 2
            class 3

        """;

        string actual = diagram.ToString();
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.Equal(expected, diagram.ToString());
    }

    [Fact(DisplayName = "Each node must display the type name and its kind (class)")]
    public void Diagram_Should_Include_All_Class_Types()
    {
        // Arrange:
        using var solution = new TemporarySolutionBuilder();
        var projectA = solution.AddProjectWithFiles(
            "ProjectA",
            new Dictionary<string, string>
            {
                ["C1.cs"] = "public class C1 {}",
                ["C2.cs"] = "internal class C2 {}",
                ["C3.cs"] = "private class C3 {}",
                ["C4.cs"] = "public abstract class C4 {}",
                ["C5.cs"] = "public sealed class C5 {}",
                ["C6.cs"] = "public class C6<T>",
                ["C7.cs"] = "public class C7<T, T> {}",
                ["C8.cs"] = "public class C8<T, T, T> {}",
                ["C9.cs"] = "public partial class C9 {}"
            });

        CsprojModel projectFile = new(
            "ProjectA",
            Path.GetFullPath(projectA),
            Path.GetRelativePath(solution.Directory, projectA));

        var csFiles = projectFile.CsFiles.ToList();

        var diagram = new StringBuilder();

        // Act:
        Rules.AddCsFileNames(csFiles, diagram);

        // Assert:
        string expected =
        """
        class C1{

        }
        class C2{
        
        }
        class C3{
        
        }
        class C4{
            <<abstract>>
        }
        class C5{
            <<sealed>>
        }
        class C6~T~{
            
        }
        class C7~T, T~{

        }
        class C8~T, T, T~{

        }
        class C9{
            <<partial>>
        }
        """;

        string actual = diagram.ToString();

        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.Equal(expected, actual);
    }

    [Fact(DisplayName = "Each node must display the type name and its kind (interface)")]
    public void Diagram_Should_Include_All_Interface_Types()
    {
        using var solution = new TemporarySolutionBuilder();
        var projectA = solution.AddProjectWithFiles(
            "ProjectA",
            new Dictionary<string, string>
            {
                ["I1.cs"] = "public interface I1 {}",
                ["I2.cs"] = "internal interface I2 {}",
                ["I3.cs"] = "public interface I3<T> {}",
                ["I4.cs"] = "public interface I4<T1, T2> {}",
                ["I5.cs"] = "public partial interface I5 {}"
            });

        var projectFile = new CsprojModel("ProjectA",
            Path.GetFullPath(projectA),
            Path.GetRelativePath(solution.Directory, projectA));

        var csFiles = projectFile.CsFiles.ToList();
        var diagram = new StringBuilder();

        Rules.AddCsFileNames(csFiles, diagram);

        string expected =
        """
        class I1{
            <<interface>>
        }
        class I2{
            <<interface>>
        }
        class I3~T~{
            <<interface>>
        }
        class I4~T1, T2~{
            <<interface>>
        }
        class I5{
            <<partial interface>>
        }
        """;

        Assert.Equal(expected, diagram.ToString());
    }

    [Fact(DisplayName = "Each node must display the type name and its kind (struct)")]
    public void Diagram_Should_Include_All_Struct_Types()
    {
        using var solution = new TemporarySolutionBuilder();
        var projectA = solution.AddProjectWithFiles(
            "ProjectA",
            new Dictionary<string, string>
            {
                ["S1.cs"] = "public struct S1 {}",
                ["S2.cs"] = "internal readonly struct S2 {}",
                ["S3.cs"] = "public struct S3<T> {}",
                ["S4.cs"] = "public readonly struct S4<T1, T2> {}",
                ["S5.cs"] = "public partial struct S5 {}"
            });

        var projectFile = new CsprojModel("ProjectA",
            Path.GetFullPath(projectA),
            Path.GetRelativePath(solution.Directory, projectA));

        var csFiles = projectFile.CsFiles.ToList();
        var diagram = new StringBuilder();

        Rules.AddCsFileNames(csFiles, diagram);

        string expected =
        """
        class S1{
            
        }
        class S2{
            <<readonly struct>>
        }
        class S3~T~{
            <<struct>>
        }
        class S4~T1, T2~{
            <<readonly struct>>
        }
        class S5{
            <<partial struct>>
        }
        """;

        Assert.Equal(expected, diagram.ToString());
    }

    [Fact(DisplayName = "Each node must display the type name and its kind (enum)")]
    public void Diagram_Should_Include_All_Enum_Types()
    {
        using var solution = new TemporarySolutionBuilder();
        var projectA = solution.AddProjectWithFiles(
            "ProjectA",
            new Dictionary<string, string>
            {
                ["E1.cs"] = "public enum E1 { One, Two }",
                ["E2.cs"] = "internal enum E2 { A, B, C }"
            });

        var projectFile = new CsprojModel("ProjectA",
            Path.GetFullPath(projectA),
            Path.GetRelativePath(solution.Directory, projectA));

        var csFiles = projectFile.CsFiles.ToList();
        var diagram = new StringBuilder();

        Rules.AddCsFileNames(csFiles, diagram);

        string expected =
        """
        class E1{
            <<enum>>
        }
        class E2{
            <<enum>>
        }
        """;

        Assert.Equal(expected, diagram.ToString());
    }

    [Fact(DisplayName = "Each node must display the type name and its kind (record)")]
    public void Diagram_Should_Include_All_Record_Types()
    {
        using var solution = new TemporarySolutionBuilder();
        var projectA = solution.AddProjectWithFiles(
            "ProjectA",
            new Dictionary<string, string>
            {
                ["R1.cs"] = "public record R1(string Name);",
                ["R2.cs"] = "public record R2<T>(T Value);",
                ["R3.cs"] = "public partial record R3(string Title);",
                ["R4.cs"] = "public record struct R4(int X, int Y);",
                ["R5.cs"] = "public readonly record struct R5<T>(T Value);"
            });

        var projectFile = new CsprojModel("ProjectA",
            Path.GetFullPath(projectA),
            Path.GetRelativePath(solution.Directory, projectA));

        var csFiles = projectFile.CsFiles.ToList();
        var diagram = new StringBuilder();

        Rules.AddCsFileNames(csFiles, diagram);

        string expected =
        """
        class R1{
            <<record>>
        }
        class R2~T~{
            <<record>>
        }
        class R3{
            <<partial>>
            <<record>>
        }
        struct R4{
            <<record>>
            <<struct>>
        }
        struct R5~T~{
            <<readonly>>
            <<record>>
            <<struct>>
        }
        """;

        Assert.Equal(expected, diagram.ToString());
    }

    [Fact(DisplayName = "Only types from .cs files in the target .csproj **must** be included")]
    public void Diagram_Should_Only_Include__cs_files()
    {
        throw new NotImplementedException();
    }

    [Fact(DisplayName = "Each class node must include a clickable link to the corresponding source file, formatted as:  \r\n  `[BaseUrl] / [Relative Path to Project] / [Source File Name]`")]
    public void Diagram_Should_Include_Clickable_Links()
    {
        // Arrange:
        using var solution = new TemporarySolutionBuilder();

        var projectA = solution.AddProjectWithFiles(
            "ProjectA",
            new Dictionary<string, string>
            {
                ["X.cs"] = "public class X",
                ["FolderOne/Y.cs"] = "public class Y",
                ["FolderOne/FolderTwo/Z.cs"] = "public class Z"
            });

        CsprojModel csProj = new(
        "ProjectA",
        Path.GetFullPath(projectA),
        Path.GetRelativePath(solution.Directory, projectA)
        );

        Settings.BaseUrl = "https://example.com/";

        var diagram = new StringBuilder();

        // Act: 
        Rules.AddClickableLinks(csProj, diagram);

        string expected =
        """
            click X href "https://example.com/ProjectA/X.cs"
            click Y href "https://example.com/ProjectA/FolderOne/Y.cs"
            click Z href "https://example.com/ProjectA/FolderOne/FolderTwo/Z.cs"

        """;

        string actual = diagram.ToString();
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.Equal(expected, diagram.ToString());
    }


    /*
    MermaidGeneratorClassHelpersTests.Diagram_Should_Include_All_Class_Types()

    Expected:
class C1{

}
class C2{

}
class C3{

}
class C4{
    <<abstract>>
}
class C5{
    <<sealed>>
}
class C6~T~{
    
}
class C7~T, T~{

}
class C8~T, T, T~{

}
class C9{
    <<partial>>
}

    Each node must display the type name and its kind (class)
    MermaidGeneratorClassHelpersTests.Diagram_Should_Include_All_Enum_Types()

    Expected: "class E1{\r\n    <<enum>>\r\n}\r\nclass E2{\r\n  "···

    Each node must display the type name and its kind (enum)
    MermaidGeneratorClassHelpersTests.Diagram_Should_Include_All_Interface_Types()

    Expected: "class I1{\r\n    <<interface>>\r\n}\r\nclass I2"···

    Each node must display the type name and its kind (interface)
    MermaidGeneratorClassHelpersTests.Diagram_Should_Include_All_Record_Types()

     Expected: "class R1{\r\n    <<record>>\r\n}\r\nclass R2~T~"···

    Each node must display the type name and its kind (record)
    MermaidGeneratorClassHelpersTests.Diagram_Should_Include_All_Struct_Types() 

    Expected: "class S1{\r\n    \r\n}\r\nclass S2{\r\n    <<read"···

    Each node must display the type name and its kind (struct)
    MermaidGeneratorClassHelpersTests.Diagram_Should_Include_All_Struct_Types()

    Expected: "class S1{\r\n    \r\n}\r\nclass S2{\r\n    <<read"···
     */
}
