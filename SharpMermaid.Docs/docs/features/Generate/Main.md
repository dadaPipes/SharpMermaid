# Generate

**As** a developer,
**I want** to use the configuration file `mermaidconfig.json`
**So that** diagrams are automatically generated based on the settings in that file

## CLI Usage

```shell
dotnet sharpmermaid generate
```

## Config Variables

**fileName**, string, Name of the generated file (without the file extension), Default: "mermaid"  
**topLevelPublicTypes**, bool,  Whether to include top-level public types in each project node, Default: `false`
**classDiagramLinks**, bool, Whether to include clickable URLs to a class diagram for each project node  
The URL pattern for class diagram links is: `{baseUrl}/{FilePath}/{ProjectName}`  
It cannot be customized beyond this pattern,  
Default: `false`
**baseUrl**, string, Base URL to prepend to diagram links when `classDiagramLinks` is enabled  
**diagramType**, enum, What type of diagram to generate, Default: physicalProject
**fileType**, enum, Default: markdown

## Rules

- **Must** locate `mermaidconfig.json` in the current working directory
- **Must** read and parse `mermaidconfig.json` as a valid JSON configuration file
- **Must** validate that the file contains required fields:
  - `solution`: path to the solution file
  - `output`: output directory for generated diagrams
- Must generate diagrams based on the configuration
- Must support adding additional optional configuration fields in `mermaidconfig.json`
  - diagram type
  - direction
  - subgraphs
**- Must display relevant console output during generation starting generation, success message**

If `mermaidconfig.json` does not exist in the current directory,
Then execution **must** stop and the console must display:
"Error: No 'mermaidconfig.json' file found in '{cwd}'"

If `mermaidconfig.json` is invalid JSON,
Then execution must stop and display an error:
"Error: Invalid JSON in 'mermaidconfig.json'"

If required fields are missing,
Then execution must stop and display an error:
"Error: Missing required 'field(s)' in 'mermaidconfig.json'"

If generation succeeds,
Then the console must display:
"Diagram generated successfully at '{outputPath}'"

### Scenarios

---

#### Generate Diagrams from Valid Config

**Given** the developer’s current working directory is `{cwd}`  
**And** a valid `mermaidconfig.json` file exists in `{cwd}`:

```json
{
  "solution": "./TestSolution.sln",
  "output": "./TestDiagrams",
  "diagramType": "physical",
  "fileType": "markdown"
}
```

**When** the developer runs:

```shell
dotnet sharpmermaid generate
```

**Then** the system **must**:

- Parse mermaidconfig.json
- Validate required fields
- Generate diagrams based on the provided configuration
- Output diagrams to {cwd}/TestDiagrams
- Display:  
"Diagrams generated successfully at '{cwd}/TestDiagrams'"

---

#### Config File Missing

Given no `mermaidconfig.json` file exists in `{cwd}`

**When** the developer runs:

```shell
dotnet sharpmermaid generate
```

**Then** the system must stop and display:
"Error: No 'mermaidconfig.json' file found in '{cwd}'"

---

#### Invalid JSON in Config

**Given** an invalid JSON in mermaidconfig.json in `{cwd}`

**When** the developer runs:

```shell
dotnet sharpmermaid generate
```

**Then** the system must stop and display:
"Error: Invalid JSON in 'mermaidconfig.json'"

---

#### Missing Required Fields

**Given** a mermaidconfig.json in {cwd} missing required fields:

```json
{
  "diagramType": "physical"
}
```

**When** the developer runs:

```shell
dotnet sharpmermaid generate
```

**Then** the system must stop and display:
"Error: Missing required field(s) in 'mermaidconfig.json'"

---

Success with Optional Fields
Given a mermaidconfig.json in {cwd}:

```json
{
  "solution": "./TestSolution.sln",
  "output": "./TestDiagrams",
  "diagramType": "api",
  "direction": "LR",
  "subgraphs": true
}
```

**When** the developer runs:

```shell
dotnet sharpmermaid generate
```

**Then** the system must:

Parse and validate config

Use diagramType, direction, subgraphs for diagram generation

Output diagrams to {cwd}/TestDiagrams

Display:
"Diagrams generated successfully at '{cwd}/TestDiagrams'"

---
