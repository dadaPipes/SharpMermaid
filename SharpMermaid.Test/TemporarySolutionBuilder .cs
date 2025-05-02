using System.Diagnostics;

namespace SharpMermaid.Test;

public class TemporarySolutionBuilder : IDisposable
{
    public string RootPath { get; }

    private readonly List<string> _projectPaths = [];

    public TemporarySolutionBuilder(string solutionName)
    {
        RootPath = Path.Combine(Path.GetTempPath(), $"SharpMermaidTest_{Guid.NewGuid()}");
        Directory.CreateDirectory(RootPath);

        RunDotnet($"new sln --name {solutionName}", RootPath);
    }

    public string AddProject(string projectName)
    {
        string projectPath = Path.Combine(RootPath, projectName);
        Directory.CreateDirectory(projectPath);

        RunDotnet($"new classlib -n {projectName}", RootPath);

        string projectFilePath = Path.Combine(projectPath, $"{projectName}.csproj");
        _projectPaths.Add(projectFilePath);

        RunDotnet($"sln add {projectFilePath}", RootPath);

        // Remove the default Class1.cs file
        var defaultClassFile = Path.Combine(projectPath, "Class1.cs");
        if (File.Exists(defaultClassFile))
        {
            File.Delete(defaultClassFile);
        }

        return projectFilePath;
    }

    public string AddProject(string projectName, Dictionary<string, string> csFiles)
    {
        string projectPath = Path.Combine(RootPath, projectName);
        Directory.CreateDirectory(projectPath);

        RunDotnet($"new classlib -n {projectName}", RootPath);

        string projectFilePath = Path.Combine(projectPath, $"{projectName}.csproj");
        _projectPaths.Add(projectFilePath);

        RunDotnet($"sln add {projectFilePath}", RootPath);

        // Remove the default Class1.cs if it exists
        var defaultClass = Path.Combine(projectPath, "Class1.cs");
        if (File.Exists(defaultClass))
            File.Delete(defaultClass);

        // Add custom class files
        foreach (var (fileName, sourceCode) in csFiles)
        {
            string fullPath = Path.Combine(projectPath, fileName);
            File.WriteAllText(fullPath, sourceCode);
        }

        return projectFilePath;
    }

    public void Dispose()
    {
        try
        {
            Directory.Delete(RootPath, recursive: true);
        }
        catch
        {
            // Ignore cleanup errors in test context
        }
    }

    public void AddProjectReference(string fromProjectPath, string toProjectPath)
    {
        RunDotnet($"add \"{fromProjectPath}\" reference \"{toProjectPath}\"", RootPath);
    }

    private void RunDotnet(string arguments, string workingDirectory)
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
