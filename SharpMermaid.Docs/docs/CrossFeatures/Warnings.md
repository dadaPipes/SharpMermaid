# Warnings

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
