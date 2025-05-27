namespace SharpMermaid.Models;
public class CsModel(string name, string fullPath, string relativePathFromCsProjDirectory)
{
    public string Name { get; set; } = name;
    public string NameWithFileExtension => $"{Name}.cs";
    public string DirectoryName { get; set; } = Path.GetDirectoryName(fullPath) ?? string.Empty;
    public string FullPath { get; set; } = fullPath;
    public string RelativePathFromCsProjDirectory { get; } = relativePathFromCsProjDirectory;
}
