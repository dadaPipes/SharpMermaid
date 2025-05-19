using System.Xml.Linq;

namespace SharpMermaid;
class CsprojModel
{
    public string Name { get; }
    public string FullPath { get; }
    /// <summary>
    /// Determines whether the project is a top-level project in the solution.
    /// </summary>
    /// <remarks>
    /// A top-level project is defined as a project that resides directly in the solution root.
    /// Since .NET projects are typically stored in their own directories, this method checks
    /// if the relative path consists of exactly two segments:
    /// 1. The project folder.
    /// 2. The `.csproj` file.
    /// If the path contains more than two segments, the project is inside a subfolder.
    /// </remarks>
    /// <example>
    /// Given the following project structures:
    /// - "ProjectA/ProjectA.csproj" → Top-level project (returns true)
    /// - "Subfolder1/ProjectB/ProjectB.csproj" → Nested project (returns false)
    /// - "Subfolder1/Subfolder2/ProjectC/ProjectC.csproj" → Nested project (returns false)
    /// </example>
    public bool IsTopLevelProject => RelativePathFromSln.Split([Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar], StringSplitOptions.RemoveEmptyEntries).Length == 2;
    public string RelativePathFromSln { get; }
    public string RelativePathFromSlnWithoutExtension => Path.GetFileNameWithoutExtension(RelativePathFromSln);
    public List<string> CsprojDependencies { get; private set; }
    public List<CsModel> CsFiles { get; private set; }
    public bool HasCsprojDependencies => CsprojDependencies.Count > 0;
    public bool HasSourceFiles => CsFiles.Count > 0;
    public string? Directory => Path.GetDirectoryName(FullPath);

    public CsprojModel(string name, string fullPath, string relativePathFromSln)
    {
        Name                                = name;
        FullPath                            = fullPath;
        RelativePathFromSln                 = relativePathFromSln;
        CsprojDependencies                  = LoadCsprojDependencies();
        CsFiles                             = LoadCsFilesFromProjectFiles();

    }

    List<string> LoadCsprojDependencies()
    {
        if (string.IsNullOrWhiteSpace(FullPath) || !File.Exists(FullPath))
            return [];

        var xdoc = XDocument.Load(FullPath);
        return xdoc
            .Descendants("ProjectReference")
            .Select(e => e.Attribute("Include")?.Value)
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .Select(includePath => Path.GetFileNameWithoutExtension(includePath!))
            .ToList();
    }

    List<CsModel> LoadCsFilesFromProjectFiles()
    {
        if (Directory is null)
            return [];

        return System.IO.Directory.GetFiles(Directory, "*.cs", SearchOption.AllDirectories)
            .Select(csFileFullPath => new CsModel(
                name: Path.GetFileNameWithoutExtension(csFileFullPath),
                fullPath: csFileFullPath,
                relativePathFromCsProjDirectory: Path.GetRelativePath(Directory, csFileFullPath)))
            .ToList();
    }
}
