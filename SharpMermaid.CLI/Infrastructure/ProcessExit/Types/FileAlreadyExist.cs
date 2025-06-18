namespace SharpMermaid.CLI.Infrastructure.ProcessExit.Types;

/// <summary>
/// Exit code 73: Indicates a file already exists and should not be overwritten.
/// </summary>
/// <param name="Directory">The directory path where the file exists.</param>
/// <param name="FileName">The name of the conflicting file.</param>
public readonly record struct FileAlreadyExists(string Directory, string FileName) : IProcessExit
{
    /// <inheritdoc />
    public int Code => 73;

    /// <inheritdoc />
    public string FormatMessage() =>
        $"Error: A '{FileName}' file already exists at '{Directory}/{FileName}'";
}
