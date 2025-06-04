# Config Reference

> [!IMPORTANT]
> All relative paths specified in config file are relative to the location of mermaidconfig.json

## Root properties

```json
{
  "SolutionPath": "./TestSolution.sln",
  "OutputDirectory": "./Diagrams",
  "FileType": ".mmd",
  "FileName": "Mermaid",
  "Diagrams": []
}
```

### `SolutionPath`

Path to the solution file.

**Type**: string  
**Required**: no  
**Default**: -

### `OutputDirectory`

Output directory for all generated diagrams (overridable per diagram).

**Type**: string  
**Required**: yes  
**Default**: -

### `FileType`

File type for all generated diagrams.

**Type**: string  
**Required**: yes  
**Default**: -  
**Supported**: `.mmd` and `.md`

### `Diagrams`

An array of diagram configurations. Each defines one diagram to generate.

**Type**: array  
**Required**: yes  
**Default**: -  
**Supported**: `PhysicalProjectDiagram`

## Physical Project Diagram

```json
{
  "Diagrams": [
    {
      "DiagramType": "PhysicalProject",
      "OutputDirectory": "./Override/Diagrams",
      "FileName": "PhysicalDiagram",
      "FileType": ".mmd",
      "TopLevelPublicTypes": true,
      "ClassDiagramLinks": true,
      "BaseUrl": "https://example.com/"
    }
  ]
}
```

### `OutputDirectory`

Overrides root output path for this diagram.

**Type**: string  
**Required**: no  
**Default**: Inherits root

### `FileName`

File name for the generated diagram

**Type**: string  
**Required**: yes  
**Default**: -

### `TopLevelPublicTypes`

Include top-level public types in each project.

**Type**: boolean  
**Required**: no  
**Default**: `false`

### `ClassDiagramLinks`

Enable clickable links to a class diagram per project.

**Type**: boolean  
**Required**: no  
**Default**: `false`

### `BaseUrl`

Base URL used to build class diagram links.

**Type**: string  
**Required**: if `ClassDiagramLinks` is true  
**Default**: -

## Full Example Config Structure

```json
{
  "SolutionPath": "./TestSolution.sln",
  "OutputDirectory": "./Diagrams",
  "FileType": ".mmd",
  "FileName": "Mermaid",
  "Diagrams": [
    {
      "OutputDirectory": "./Override/Diagrams",
      "DiagramType": "PhysicalProject",
      "FileName": "PhysicalDiagram",
      "FileType": ".mmd",
      "TopLevelPublicTypes": true,
      "ClassDiagramLinks": true,
      "BaseUrl": "https://example.com/"
    }
  ]
}
```
