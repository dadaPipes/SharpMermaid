using System.Xml.Linq;

namespace SharpMermaid;
public class CsprojModel
{
    public string Name { get; }
    public string FullPath { get; }
    public bool IsTopLevelProject => RelativePathFromSln.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).Length == 2;
    public string RelativePathFromSln { get; }
    public List<string> CsprojDependencies { get; private set; } = [];
    public List<CsModel> CsFiles { get; private set; } = [];
    public bool HasCsprojDependencies => CsprojDependencies.Count > 0;
    public bool HasSourceFiles => CsFiles.Count > 0;
    public string? Directory => Path.GetDirectoryName(FullPath);

    public CsprojModel(string name, string fullPath, string relativePathFromSln)
    {
        Name = name;
        FullPath = fullPath;
        RelativePathFromSln = relativePathFromSln;

        LoadCsprojDependencies();
        LoadCsFilesFromProjectFiles();
    }

    void LoadCsprojDependencies()
    {
        if (string.IsNullOrWhiteSpace(FullPath) || !File.Exists(FullPath))
            return;

        var xdoc = XDocument.Load(FullPath);
        CsprojDependencies = xdoc
            .Descendants("ProjectReference")
            .Select(e => e.Attribute("Include")?.Value)
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .Select(includePath => Path.GetFileNameWithoutExtension(includePath!))
            .ToList();
    }

    void LoadCsFilesFromProjectFiles()
    {
        if (Directory is null)
            return;

        var csFiles = System.IO.Directory.GetFiles(Directory, "*.cs", SearchOption.AllDirectories);
        CsFiles = csFiles
            .Select(path => new CsModel
            {
                Directory = path,
                Name = Path.GetFileName(path)
            })
            .ToList();
    }
}
