using System.Text.RegularExpressions;

namespace SharpMermaid.FeatureTests.Helpers;
internal static class TestOutputNormalizer
{
    /// <summary>
    /// Normalizes Docker container log output by removing timestamps and flattening multiline messages.
    /// This is useful when testing console output in containerized environments, where long lines are split and prefixed with timestamps.
    /// </summary>
    /// <param name="rawOutput">
    /// The raw output string retrieved from a container log stream, such as the result of <c>container.GetLogsAsync()</c>.
    /// </param>
    /// <returns>
    /// A single-line string with all Docker timestamps and line breaks removed. Consecutive whitespace is also collapsed to a single space.
    /// </returns>
    public static string NormalizeContainerOutput(string rawOutput)
    {
        if (string.IsNullOrWhiteSpace(rawOutput))
            return string.Empty;

        var lines = rawOutput
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(line =>
            {
                // Strip Docker timestamps (up to the first 'Z ')
                var idx = line.IndexOf("Z ");
                return idx >= 0 ? line[(idx + 2)..].Trim() : line.Trim();
            });

        var joined = string.Join(" ", lines);

        // Optional: Collapse multiple whitespaces into a single space for clean matching
        return Regex.Replace(joined, @"\s+", " ");
    }
}
