# Cross Features

## Rules



### Config File

- **Missing `mermaidconfig.json` file**:
  If `mermaidconfig.json` is not found in the current working directory, the console **must** display:
  "Error: Configuration file 'mermaidconfig.json' not found in the current directory"  
  [***see: scenario:***](#config-file-missing)

- **Missing `SolutionPath`**:
  If `SolutionPath` is not provided, the system **must** stop execution and the console **must** display:
  "Error: 'SolutionPath' is missing in 'mermaidconfig.json'. Please specify a valid solution path."
  [***see: scenario***](#missing-solution-path)

- **Invalid `SolutionPath`**:  
  If a `.sln` file does not exist at `SolutionPath`, the system **must** stop execution and the console **must** display:  
  "Error: .sln file not found at '{path}'. Please provide a valid 'SolutionPath' in 'mermaidconfig.json'"  
  [***see: scenario***](#invalid-solution-path)

### Project Files

- **No .csproj Files Found**:  
  If no `.csproj` files are found in the solution, the console **must** display:  
  Warning: No .csproj files found at '{SolutionPath}'"  
  [***see: scenario:***](#no-projects-found-in-solution)

- **No .cs Files Found**:  
  If no `.cs` files are found in a project, the console must display:  
  "{solution path}:  
  Warning: No .cs files found in '{project name}' at '{SolutionPath}'"  
  [***see: scenario:***](#no-source-files-found-in-project)

- **Bi-directional references**:  
  If a bi-directional reference is detected between `.csproj` files, the console must display the following message:
  Bi-directional reference detected between:  
  {project name} at '{SolutionPath}'  
  and  
  {project name} at '{SolutionPath}'
  [***see: scenario***](#warn-on-bi-directional-project-references)

### Diagram Conflicts

- If multiple diagrams have the same `FileName` and `OutputDirectory`, execution must stop and the console must display:  
  "Error: Multiple diagrams cannot have the same FileName '{FileName}' and OutputDirectory '{OutputDirectory}'. Please resolve the conflict."  
  [***see: scenario:***](#diagram-conflicts)

## Scenarios

---

### Warn when solution contains no projects

**Given** a solution `EmptySolution`  
**And** the solution contains no `.csproj` files  

**When** I run the command:

```shell
sharpmermaid generate-diagram --solution ./EmptySolution.sln
```

from the solution directory

**Then** the console output includes the warning: "**No projects found in the solution EmptySolution**"

---

### Create File A1

### Warn on bi-directional project references

**Given** a solution `TestSolution` containing:

- `ProjectA` at `ProjectA/ProjectA.csproj`
- `ProjectB` at `ProjectB/ProjectB.csproj`

**And** `ProjectA` references `ProjectB`  
**And** `ProjectB` references `ProjectA`

**When** I run the command:

```shell
sharpmermaid generate-diagram --solution ./TestSolution.sln
```

from the solution directory

**Then** the console output includes the warning: "**Bi-directional dependency detected between ProjectA and ProjectB**"

---

### Warn when project contains no .cs files

**Given** a solution `NoCsFilesSolution` containing:

`EmptyProject` at `EmptyProject/EmptyProject.csproj`

**And** `EmptyProject` contains no `.cs` files

**When** I run the command:

```shell
sharpmermaid generate-diagram --solution ./NoCsFilesSolution.sln`
```

from the solution directory

**Then** the console output includes the warning: "**No .cs files found in EmptyProject**"

---

### Warn when a single file contains multiple public types

**Given** a solution `MultiTypeSolution` containing:

`MultiTypeProject` at `MultiTypeProject/MultiTypeProject.csproj`

**And** MultiTypeProject has a file `Types.cs` containing:

`public class A {}`  
`public class B {}`

**When** I run the command:

```shell
sharpmermaid generate-diagram --solution ./MultiTypeSolution.sln
```

from the solution directory

**Then** the console output includes the warning: "**Multiple public types found in {FullPath}/MultiTypeProject/Types.cs: A, B**"

---
