namespace SharpMermaid.CLI.ProcessExit;
/// <summary>
/// Exit codes used by the SharpMermaid CLI tool to indicate specific outcomes.
/// </summary>
public enum ExitCode
{
    /// <summary>
    /// Success: Operation completed as expected.
    /// System must exit with code 0.
    /// Example: Initialization completed successfully.
    /// </summary>
    Success = 0,

    /// <summary>
    /// General error: An unexpected failure occurred.
    /// System must display: "Error: An unexpected failure occurred - {errorMessage}"
    /// Exit with code 1.
    /// </summary>
    GeneralError = 1,

    /// <summary>
    /// File already exists: A config or output file already exists and must not be overwritten.
    /// System must display: "Error: A '{fileName}' file already exists at '{path}/{fileName}'"
    /// Exit with code 73.
    /// </summary>
    FileAlreadyExists = 73,

    /// <summary>
    /// I/O error: Disk full, file locked, or other I/O issue.
    /// System must display: "Error: Failed to write file at '{path}/{file}': {errorMessage}"
    /// Exit with code 74.
    /// </summary>
    IOError = 74,

    /// <summary>
    /// Missing configuration: Expected config file is not found.
    /// System must display: "Error: Required configuration file '{fileName}' is missing"
    /// Exit with code 75.
    /// </summary>
    ConfigurationMissing = 75,

    /// <summary>
    /// Invalid configuration: File exists but contains invalid content.
    /// System must display: "Error: Invalid configuration in '{path}/{file}' - {validationMessage}"
    /// Exit with code 76.
    /// </summary>
    ConfigurationError = 76,

    /// <summary>
    /// Permission denied: System lacks write permission to the file path.
    /// System must display: "Error: Cannot write to '{path}/{file}' - No write permission"
    /// Exit with code 77.
    /// </summary>
    PermissionDenied = 77
}
