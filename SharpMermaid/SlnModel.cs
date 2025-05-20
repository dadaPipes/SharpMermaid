namespace SharpMermaid;
public class SlnModel
{
    public readonly string Name;
    public readonly string Directory;
    public List<CsprojModel> Csprojs { get; }
    public bool HasProjects => Csprojs.Count > 0;

    public SlnModel(string slnFullPath)
    {
        Directory = Path.GetDirectoryName(slnFullPath) ?? string.Empty;
        Name      = Path.GetFileNameWithoutExtension(slnFullPath);
        Csprojs   = LoadProjectFilesFromSolution();
    }

    List<CsprojModel> LoadProjectFilesFromSolution()
    {
        return System.IO.Directory.GetFiles(Directory, "*.csproj", SearchOption.AllDirectories)
            .Select(filePath => new CsprojModel(
                name               : Path.GetFileNameWithoutExtension(filePath),
                fullPath           : filePath,
                relativePathFromSln: Path.GetRelativePath(Directory, filePath)))
            .ToList();
    }
}
