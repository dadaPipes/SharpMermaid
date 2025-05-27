using Spectre.Console;
using System.Xml.Linq;

namespace SharpMermaid.Models;
public class CsprojModel
{
    public string Name { get; }
    public string FullPath { get; }
    public bool IsTopLevelProject => RelativePathFromSln.Split([Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar], StringSplitOptions.RemoveEmptyEntries).Length == 2;
    public string RelativePathFromSln { get; }
    public string RelativePathFromSlnWithoutExtension => Path.GetFileNameWithoutExtension(RelativePathFromSln);
    public List<string> CsprojDependencies { get; private set; }
    public List<CsModel> CsFiles { get; private set; }
    public bool HasCsprojDependencies => CsprojDependencies.Count > 0;
    public bool HasCsFiles => CsFiles.Count > 0;
    public string? Directory => Path.GetDirectoryName(FullPath);
    public bool IncludeClassDiagramUrl { get; }
    public string? ClassDiagramUrl { get; }

    public CsprojModel(string name, string fullPath, string relativePathFromSln, bool includeClassDiagramUrl = false, string classDiagramUrl = null)
    {
        Name                   = name;
        FullPath               = fullPath;
        RelativePathFromSln    = relativePathFromSln;
        CsprojDependencies     = LoadCsprojDependencies();
        CsFiles                = LoadCsFilesFromProjectFiles();
        IncludeClassDiagramUrl = includeClassDiagramUrl;
        ClassDiagramUrl        = classDiagramUrl;

        if (!HasCsFiles)
        {
            AnsiConsole.Markup($"[yellow]No .cs files found in {Name}[/]");
        }
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
