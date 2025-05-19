namespace SharpMermaid;
class CsModel
{
    public string Name { get; set; }
    public string NameWithFileExtension => $"{Name}.cs";
    public string DirectoryName { get; set; }
    public string FullPath { get; set; }
    public string RelativePathFromCsProjDirectory { get; }
    
    public CsModel(string name, string fullPath, string relativePathFromCsProjDirectory)
    {
        Name                            = name;
        DirectoryName                   = Path.GetDirectoryName(fullPath) ?? string.Empty;
        FullPath                        = fullPath;
        RelativePathFromCsProjDirectory = relativePathFromCsProjDirectory;
    }
}
