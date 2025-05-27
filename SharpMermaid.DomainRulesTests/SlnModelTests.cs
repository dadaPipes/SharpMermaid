using SharpMermaid.Models;
using SharpMermaid.TestHelpers;

namespace SharpMermaid.DomainRulesTests;
public class SlnModelTests
{
    [Fact(DisplayName = "Rules: Empty Solution Warning → SlnModel reports warning when no projects")]
    public void SlnModel_ShouldReportWarning_WhenNoProjects()
    {
        // arrange
        var console = new StringWriter();
        Console.SetOut(console);
        using var solution = new TemporarySolutionBuilder();

        // act
        _ = new SlnModel(solution.FullPath);

        // assert
        Assert.Contains("No projects found in the solution", console.ToString());
    }
}
