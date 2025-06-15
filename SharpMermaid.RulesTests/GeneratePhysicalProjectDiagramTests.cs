using Microsoft.VisualStudio.TestPlatform.Utilities;
using SharpMermaid.Features.GeneratePhysicalProjectDiagram;
using SharpMermaid.Models;
using SharpMermaid.TestHelpers;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SharpMermaid.RulesTests;
public class GeneratePhysicalProjectDiagramTests(ITestOutputHelper output)
{
    /*
    private readonly ITestOutputHelper _output = output;

    [Fact(DisplayName = "Mermaid Fences: Start")]
    public void ShouldAddMermaidCodeBlockFence()
    {
        // Arrange:
        var diagram = new StringBuilder();

        // Act:
        Rules.AddMermaidCodeBlockStart(diagram);

        // Assert:
        string expected =
        $"""
        ```mermaid

        """;

        string actual = diagram.ToString();
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.Equal(expected, diagram.ToString());
    }

    [Fact(DisplayName = "Mermaid Fences: End")]
    public void ShouldAddCodeBlockFenceFooter()
    {
        // Arrange:
        var diagram = new StringBuilder();

        // Act:
        Rules.AddCodeBlockEnd(diagram);

        // Assert:
        string expected =
        $"""
        ```
        """;

        string actual = diagram.ToString();
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.Equal(expected, diagram.ToString());
    }

    [Fact(DisplayName = "Graph Declaration")]
    public void ShouldAddGraphDeclaration()
    {
        // Arrange:
        var diagram = new StringBuilder();

        // Act:
        Rules.AddGraphDeclaration(diagram);

        // Assert:
        string expected =
        $"""
        graph

        """;

        string actual = diagram.ToString();
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.Equal(expected, diagram.ToString());
    }

    [Fact(DisplayName = "Diagram Title")]
    public void ShouldAddTitle()
    {
        // Given** a solution file is provided with the name `FooApp`:
        var diagram = new StringBuilder();
        Rules.AddSolutionNameAsTitle("FooApp", diagram);

        // Then** the diagram **must** include a title `FooApp`:
        string expected =
        $"""
        ---
        title: FooApp
        ---

        """;

        string actual = diagram.ToString();
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.Equal(expected, diagram.ToString());
    }

    [Fact(DisplayName = "Project Nodes: Single")]
    public void ShouldIncludeSingleProjectNode_WhenSolutionHasSingleProject()
    {
        // Given a solution file is provided with a `Project1`:
        var diagram        = new StringBuilder();
        using var solution = new TemporarySolutionBuilder();

        var project1 = solution.AddProject("Project1");

        List<CsprojModel> projectFiles = [
            new ("Project1", Path.GetFullPath(project1), Path.GetRelativePath(solution.Directory, project1))
        ];

        // When the diagram is generated:
        Rules.AddProjectNames(projectFiles, diagram);

        // Then it must include a project node with the name `Project 1`:
        string expected =
        """
            Project1

        """;

        string actual = diagram.ToString();
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.Equal(expected, diagram.ToString());
    }

    [Fact(DisplayName = "Project Nodes: Multiple")]
    public void ShouldIncludeMultipleNodes_WhenSolutionHasMultipleProjects()
    {
        // Given a solution file is provided with a `Project1`, `Project2` and `Project3`

        var diagram = new StringBuilder();

        using var solution = new TemporarySolutionBuilder();

        var project1 = solution.AddProject("Project1");
        var project2 = solution.AddProject("Project2");
        var project3 = solution.AddProject("Project3");

        List<CsprojModel> projectFiles = [
            new ("Project1", Path.GetFullPath(project1), Path.GetRelativePath(solution.Directory, project1)),
            new ("Project2", Path.GetFullPath(project2), Path.GetRelativePath(solution.Directory, project2)),
            new ("Project3", Path.GetFullPath(project3), Path.GetRelativePath(solution.Directory, project3))
        ];

        // When the diagram is generated:
        Rules.AddProjectNames(projectFiles, diagram);

        // Then it must include a project nodes with the names `Project1`, `Project2` and `Project3`:
        string expected =
        """
            Project1
            Project2
            Project3

        """;
        string actual = diagram.ToString();
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);
        Assert.Equal(expected, diagram.ToString());
    }

    [Fact(DisplayName = "One-way Dependency")]
    public void ShouldIncludeOneWayDependency_WhenDependencyExists()
    {
        // Given a solution file is provided with a `Project1`, `Project2` and `Project3``:
        using var solution = new TemporarySolutionBuilder();

        var project1 = solution.AddProject("Project1");
        var project2 = solution.AddProject("Project2");
        var project3 = solution.AddProject("Project3");

        // And a one-way dependency exists between `Project 1` and `Project 2`
        solution.AddProjectReference(project1, project2);

        List<CsprojModel> projectFiles = [
            new ("Project1", Path.GetFullPath(project1), Path.GetRelativePath(solution.Directory, project1)),
            new ("Project2", Path.GetFullPath(project2), Path.GetRelativePath(solution.Directory, project2)),
            new ("Project3", Path.GetFullPath(project3), Path.GetRelativePath(solution.Directory, project3))
            ];

        // When the diagram is generated:
        var diagram = new StringBuilder();
        Rules.AddProjectDependencies(projectFiles, diagram);

        // Then the diagram **must** include an arrow: `1 --> 2`:
        string expected =
        """
            Project1 --> Project2

        """;

        string actual = diagram.ToString();
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.Equal(expected, diagram.ToString());
    }

    [Fact(DisplayName = "Bi-directional Dependency")]
    public void ShouldShowBidirectionalDependency_WhenBiDirectionalDependencyExist()
    {
        // Given a solution file is provided with a `Project1`, `Project2` and `Project3`:
        using var solution = new TemporarySolutionBuilder();
        var project1 = solution.AddProject("Project1");
        var project2 = solution.AddProject("Project2");

        // And a bi-directional dependency exists between `Project 1` and `Project 2`
        solution.AddProjectReference(project1, project2);
        solution.AddProjectReference(project2, project1);

        List<CsprojModel> projectFiles = [
            new ("Project1", Path.GetFullPath(project1), Path.GetRelativePath(solution.Directory, project1)),
            new ("Project2", Path.GetFullPath(project2), Path.GetRelativePath(solution.Directory, project2)),
            ];

        // When the diagram is generated
        var diagram = new StringBuilder();
        Rules.AddProjectDependencies(projectFiles, diagram);

        // Then the diagram **must** include an arrow: `1 <--> 2`:
        string expected =
        """
            Project1 <--> Project2

        """;

        string actual = diagram.ToString();
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.Equal(expected, diagram.ToString());
    }

    [Fact(DisplayName = "No Dependencies")]
    public void ShouldNotDisplayArrow_WhenNoDependencyExists()
    {
        // Given a solution file is provided with a `Project1`, `Project2` and `Project3`:
        using var solution = new TemporarySolutionBuilder();

        var project1 = solution.AddProject("Project1");
        var project2 = solution.AddProject("Project2");
        var project3 = solution.AddProject("Project3");

        List<CsprojModel> projectFiles = [
            new ("Project1", Path.GetFullPath(project1), Path.GetRelativePath(solution.Directory, project1)),
            new ("Project2", Path.GetFullPath(project2), Path.GetRelativePath(solution.Directory, project2)),
            new ("Project3", Path.GetFullPath(project3), Path.GetRelativePath(solution.Directory, project3)),
            ];

        // And `Project1`, `Project2` and `Project3` has no dependencies  

        // When the diagram is generated
        var diagram = new StringBuilder();
        Rules.AddProjectDependencies(projectFiles, diagram);

        // Then it must not include any arrows:
        string expected =
        """

        """;

        string actual = diagram.ToString();
        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);

        Assert.Equal(expected, diagram.ToString());
    }

    [Fact(DisplayName = "CD URLs Enabled")]
    public void ShouldAddUrls_URLs_Enabled()
    {
        // Given a solution file is provided with a `Project1`, `Project2` and `Project3
        // And `Project1` and `Project2` has source files
        // And `Project2` is in a folder `SubFolder1`
        using var solution = new TemporarySolutionBuilder();

        var project1 = solution.AddProjectWithFiles("Project1", new Dictionary<string, string> { ["A.cs"] = "public class A" });
        var project2 = solution.AddProjectWithFiles("Subfolder1", "Project2", new Dictionary<string, string> { ["A.cs"] = "public class A" });
        var project3 = solution.AddProject("Project3");

        List<CsprojModel> projectFiles =
        [
            new("Project1", Path.GetFullPath(project1), Path.GetRelativePath(solution.Directory, project1)),
            new("Project2", Path.GetFullPath(project2), Path.GetRelativePath(solution.Directory, project2)),
            new("Project3", Path.GetFullPath(project3), Path.GetRelativePath(solution.Directory, project3))
        ];

        // And the IncludeUrls option is enabled
        // And the base url is set to `https://example.com`
        // And the UrlPattern is set to {baseUrl}/{FilePath}/{ProjectName}
        var settings = new Settings
        {
            IncludeUrls = true,
            BaseUrl = "https://example.com",
            UrlPattern = "{baseUrl}/{FilePath}/{ProjectName}"
        };

        // When the diagram is generated
        var diagram = new StringBuilder();
        Rules.AddClickableLinks(projectFiles, diagram, settings);

        // Then each project node **must** include a clickable URL
        // And for each project, the final URL must correctly reflect the project’s file path(if present) and project name

        string expected =
        """
            click Project1 "https://example.com/Project1.csproj"
            click Project2 "https://example.com/Subfolder1/Project2.csproj"
        
        """;

        string actual = diagram.ToString().Trim();

        _output.WriteLine("Expected:\n" + expected);
        _output.WriteLine("Actual:\n" + actual);
        
        Assert.Equal(expected, diagram.ToString());
    }

    [Fact(DisplayName = "CD URLs Config: JSON Fallback")]
    public void ShouldFallbackToDefaultConfiguration_WhenJsonConfigFileIsMissing()
    {
        throw new NotImplementedException();
    }

    [Fact(DisplayName = "CD URLs Config: CLI Overrides")]
    public void ShouldOverrideJsonConfigWithCliParameters_WhenCliParametersProvided()
    {
        throw new NotImplementedException();
    }

    [Fact(DisplayName = "CD URLs Default Behavior")]
    public void ShouldUseDefaultConfigurationAndOmitUrls_WhenNeitherCliNorJsonConfigProvided()
    {
        throw new NotImplementedException();
    }

    [Fact(DisplayName = "CD URLs Disabled")]
    public void ShouldNotIncludeUrls_WhenIncludeUrlsOptionIsDisabled()
    {
        throw new NotImplementedException();
    }

    [Fact(DisplayName = "Public Types Enabled")]
    public void ShouldShouldListAllPublicTopLevelTypes_WhenShowPublicTypesOptionEnabled()
    {
        throw new NotImplementedException();
    }

    [Fact(DisplayName = "Public Types Disabled")]
    public void Should()
    {
        throw new NotImplementedException();
    }
    */
}

/*
        ```mermaid
        graph
        Project1["
            **Project1**
            ClassA
            ClassB
        "]
        Project2["
            **Project2**
            ClassC
            ClassD
        "]
        Project1 --> Project2
        click Project1 "https://example.com/Project1" "Project1 Desc"
        click Project2 "https://example.com/Project2" "Project2 Desc"

        ```
         */

/*
        ```mermaid
        graph
        Project1
        Project2
        Project1 --> Project2
        click Project1 "https://example.com/Project1" "Project1 Desc"
        click Project2 "https://example.com/Project2" "Project2 Desc"
        ```
         */