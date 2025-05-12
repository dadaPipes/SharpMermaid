namespace SharpMermaid;
public class SlnModel
{
    public readonly string Name;
    public readonly string Directory;
    public readonly List<CsprojModel> Csprojs = [];
    public bool HasProjects => Csprojs.Count > 0;

    public SlnModel(string slnFullPath)
    {
        Directory = Path.GetDirectoryName(slnFullPath) ?? string.Empty;
        Name = Path.GetFileNameWithoutExtension(slnFullPath);
        LoadProjectFilesFromSolution();
    }

    void LoadProjectFilesFromSolution()
    {
        foreach (var csprojFullPath in System.IO.Directory.GetFiles(Directory, "*.csproj", SearchOption.AllDirectories))
        {
            var relativePathFromSolution = Path.GetRelativePath(Directory, csprojFullPath);
            var name = Path.GetFileNameWithoutExtension(csprojFullPath);

            Csprojs.Add(new CsprojModel(name, csprojFullPath, relativePathFromSolution));
        }
    }
}
