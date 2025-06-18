using SharpMermaid.CLI.Infrastructure.ProcessExit.Types;

namespace SharpMermaid.CLI.Infrastructure.ProcessExit;
public static class ExitTypes
{
    public static IProcessExit Success(string message) =>
        new Success(message);

    public static IProcessExit GeneralError(string errorMessage) =>
        new GeneralError(errorMessage);

    /// <inheritdoc cref="Types.FileAlreadyExists"/>
    public static IProcessExit FileAlreadyExists(string filePath, string fileName) =>
        new FileAlreadyExists(filePath, fileName);

    public static IProcessExit IOError(string path, string file, string errorMessage) =>
        new IOError(path, file, errorMessage);

    public static IProcessExit ConfigurationMissing(string fileName) =>
        new ConfigurationMissing(fileName);

    public static IProcessExit ConfigurationError(string path, string file, string message) =>
        new ConfigurationError(path, file, message);

    public static IProcessExit PermissionDenied(string path, string file) =>
        new PermissionDenied(path, file);
}
