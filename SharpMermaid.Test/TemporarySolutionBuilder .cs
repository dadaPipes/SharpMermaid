using System.Diagnostics;

namespace SharpMermaid.Test;

/// <summary>
/// Builds a temporary .NET solution with projects for test purposes.
/// Automatically cleans up temporary files when disposed.
/// </summary>
public class TemporarySolutionBuilder : IDisposable
{
    /// <summary>
    /// Gets the solution name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the directory where the temporary solution is created.
    /// </summary>
    public string Directory { get; }

    /// <summary>
    /// Gets the full path where the temporary solution is created.
    /// </summary>
    public readonly string FullPath;

    public List<string> ProjectPaths { get; private set; } = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="TemporarySolutionBuilder"/> class,
    /// creates a temporary folder and initializes a solution.
    /// </summary>
    public TemporarySolutionBuilder()
    {
        Name = $"SharpMermaidTest_{Guid.NewGuid()}";
        Directory = Path.Combine(Path.GetTempPath(), Name);
        System.IO.Directory.CreateDirectory(Directory);
        FullPath = Path.Combine(Directory, $"{Name}.sln");

        RunDotnet($"new sln --name {Name}", Directory);
    }

    /// <summary>
    /// Adds a new class library project to the solution at the root level.
    /// </summary>
    /// <param name="projectName">The name of the project.</param>
    /// <returns>The full path to the project file (.csproj).</returns>
    public string AddProject(string projectName)
    {
        string newDirectory = Path.Combine(Directory, projectName);
        System.IO.Directory.CreateDirectory(newDirectory);

        RunDotnet($"new classlib -n {projectName}", Directory);

        string projectFilePath = Path.Combine(newDirectory, $"{projectName}.csproj");
        ProjectPaths.Add(projectFilePath);

        RunDotnet($"sln add {projectFilePath}", Directory);

        // Remove the default Class1.cs if it exists
        var defaultClass = Path.Combine(newDirectory, "Class1.cs");
        if (File.Exists(defaultClass))
            File.Delete(defaultClass);

        return projectFilePath;
    }

    /// <summary>
    /// Adds a new class library project to a specified subfolder in the solution.
    /// </summary>
    /// <param name="relativeFolderPath">The subfolder path relative to the root.</param>
    /// <param name="projectName">The name of the project.</param>
    /// <returns>The full path to the project file (.csproj).</returns>
    /// /// <example>
    /// var builder = new TemporarySolutionBuilder("MySolution");
    /// builder.AddProject("Libraries", "Utils"); // Creates Libraries/Utils project
    /// builder.AddProject("Services/Email", "EmailService"); // Creates Services/Email/EmailService project
    /// </example>
    public string AddProject(string relativeFolderPath, string projectName)
    {
        string parentFolder = Path.Combine(Directory, relativeFolderPath);
        System.IO.Directory.CreateDirectory(parentFolder);

        // Create the project in the correct subdirectory
        RunDotnet($"new classlib -n {projectName}", parentFolder);

        // The project path is now directly inside the parent folder
        string projectFolder = Path.Combine(parentFolder, projectName);
        string projectFilePath = Path.Combine(projectFolder, $"{projectName}.csproj");

        ProjectPaths.Add(projectFilePath);
        RunDotnet($"sln add \"{projectFilePath}\"", Directory);

        var defaultClassFile = Path.Combine(projectFolder, "Class1.cs");
        if (File.Exists(defaultClassFile))
            File.Delete(defaultClassFile);

        return projectFilePath;
    }

    /// <summary>
    /// Adds a new class library project with custom source files to the solution at the root level.
    /// </summary>
    /// <param name="projectName">The name of the project.</param>
    /// <param name="sourceFiles">A dictionary mapping file names to their C# source content.</param>
    /// <returns>The full path to the project file (.csproj).</returns>
    public string AddProjectWithFiles(string projectName, Dictionary<string, string> sourceFiles)
    {
        string projectPath = Path.Combine(Directory, projectName);
        System.IO.Directory.CreateDirectory(projectPath);

        RunDotnet($"new classlib -n {projectName}", Directory);

        string projectFilePath = Path.Combine(projectPath, $"{projectName}.csproj");
        ProjectPaths.Add(projectFilePath);

        RunDotnet($"sln add {projectFilePath}", Directory);

        var defaultClassFile = Path.Combine(projectPath, "Class1.cs");
        if (File.Exists(defaultClassFile))
            File.Delete(defaultClassFile);

        foreach (var (fileName, sourceCode) in sourceFiles)
        {
            string fullPath = Path.Combine(projectPath, fileName);
            File.WriteAllText(fullPath, sourceCode);
        }

        return projectFilePath;
    }

    /// <summary>
    /// Adds a new class library project with custom source files to a specified subfolder in the solution.
    /// </summary>
    /// <param name="relativeFolderPath">The subfolder path relative to the root.</param>
    /// <param name="projectName">The name of the project.</param>
    /// <param name="csFiles">A dictionary mapping file names to their C# source content.</param>
    /// <returns>The full path to the project file (.csproj).</returns>
    public string AddProjectWithFiles(string relativeFolderPath, string projectName, Dictionary<string, string> csFiles)
    {
        string projectFolder = Path.Combine(Directory, relativeFolderPath, projectName);
        System.IO.Directory.CreateDirectory(projectFolder);

        RunDotnet($"new classlib -n {projectName}", Path.Combine(Directory, relativeFolderPath));

        string projectFilePath = Path.Combine(projectFolder, $"{projectName}.csproj");
        ProjectPaths.Add(projectFilePath);

        RunDotnet($"sln add \"{projectFilePath}\"", Directory);

        var defaultClassFile = Path.Combine(projectFolder, "Class1.cs");
        if (File.Exists(defaultClassFile))
            File.Delete(defaultClassFile);

        foreach (var (fileName, sourceCode) in csFiles)
        {
            string fullPath = Path.Combine(projectFolder, fileName);
            File.WriteAllText(fullPath, sourceCode);
        }

        return projectFilePath;
    }

    /// <summary>
    /// Adds a project reference from one project to another.
    /// </summary>
    /// <param name="fromProjectPath">The path of the referencing project (.csproj).</param>
    /// <param name="toProjectPath">The path of the referenced project (.csproj).</param>
    public void AddProjectReference(string fromProjectPath, string toProjectPath)
    {
        RunDotnet($"add \"{fromProjectPath}\" reference \"{toProjectPath}\"", Directory);
    }

    /// <summary>
    /// Deletes the temporary solution directory and all its contents.
    /// </summary>
    public void Dispose()
    {
        try
        {
            System.IO.Directory.Delete(Directory, recursive: true);
        }
        catch
        {
            // Ignore cleanup errors in test context
        }
    }

    void RunDotnet(string arguments, string workingDirectory)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = arguments,
                WorkingDirectory = workingDirectory,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            var error = process.StandardError.ReadToEnd();
            throw new Exception($"dotnet CLI failed: {error}");
        }
    }
}
