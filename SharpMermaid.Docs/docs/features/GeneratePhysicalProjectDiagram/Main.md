# Generate Physical Project Diagram

## Description

As a developer,  
I want to generate a diagram that reflects the solution structure on disk,  
and format it as a mermaid code block for use in a `.md` file,  
So that I can analyze and visualize project organization and dependencies.

## Rules

---

### Console Warnings

#### Empty Solution Warning

**When** a solution has no projects,  
**Then** display console warning: "No projects found in the solution {SolutionName}"

**See: [SlnModel -> Empty Solution Warning](./../../DomainRules.md#empty-solution-warning)**

#### Missing .cs Files Warning

**Given** a project has no .cs files  
**Then** a console warning **must** be displayed: "No .cs files found in {ProjectName}"

**See: [CsProjModel -> Missing .cs Files Warning](./../../DomainRules.md#Missing-.cs-Files-Warning)**

#### Multiple Public Types Warning

**Given** a `.cs` file has more than a single public top-level type, such as:  
`public class A` and `public class B` **or** `public enum X` and `public struct Y`  
**Then** a console warning **must** be displayed in the following format: "Multiple public types found in {FilePath}: {Type1}, {Type2}, ..."  
**Where** {FilePath} is the full path to the `.cs` file, and {Type1}, {Type2}, ... are the names of the detected public types

---

### Code Block Formatting

#### Mermaid Fences

**Given** a diagram is generated  
**Then** the output **must** start with` ```mermaid`  
**And** the output **must** end with ` ``` `

---

### Diagram Structure

#### Graph Declaration

**Given** a diagram is generated  
**Then** the first non-fence line **must** begin with `graph`

#### Diagram Title

**Given** a diagram is generated for solution `FooApp`  
**Then** the diagram **must** include a title `FooApp`

#### Project Nodes

**Given** N projects are selected  
**When** the diagram is generated  
**Then** it **must** include exactly N nodes, each named after its project  

### Dependency Arrows

#### One-way Dependency

**Given** a one-way dependency exists between `Project A` and `Project B`  
**When** the diagram is generated  
**Then** the diagram **must** include an arrow: `A --> B`

#### Bi-directional Dependency

**Given** a bi-directional dependency exists between `Project A` and `Project B`  
**When** the diagram is generated  
**Then** the diagram **must** include an arrow: `A <--> B`  
**And** a console warning **must** be displayed: "Bi-directional dependency detected between A and B"

#### No Dependencies

**Given** a project has no dependencies  
**When** the diagram is generated  
**Then** it **must not** include any arrows for that project

---

### Class-Diagram URLs (Optional)

#### JSON Configuration

**Given** no CLI parameter is provided,  
**When** the diagram is generated,  
**Then** configuration must be read from a diagramconfig.json file if present.

#### CLI Overrides

**Given** CLI parameters are provided,  
**When** the diagram is generated,  
**Then** CLI parameters must override any values in the JSON file

#### Default Behavior

**Given** neither a JSON file nor CLI parameters are provided,  
**When** the diagram is generated,  
**Then** no URLs must be included in any node

#### URL Composition

**Given** IncludeUrls is true (via JSON or CLI),  
**When** the diagram is generated,  
**Then** each node must include a clickable URL,  
**And** the URL must be assembled using the UrlPattern by substituting:

- {SolutionName}
- {ProjectName}
- {FilePath}
- {TypeName}

- Example URL: `https://example.com/docs/{ProjectName}/{FilePath}#{TypeName}`

#### No URLs

**Given** IncludeUrls is false (via JSON or CLI),  
**When** the diagram is generated,  
**Then** no URLs must appear in any node

#### URLs Enabled

**Given** “include URLs” is enabled  
**When** the diagram is generated  
**Then** each node in the diagram **must** include a clickable URL to its class diagram

#### URLs Disabled

**Given** “include URLs” is disabled  
**When** the diagram is generated  
**Then** no URLs **must** appear in the diagram

---

### Public-Type Display (Optional)

#### Public Types Enabled

**Given** “show public types” is enabled  
**When** the diagram is generated  
***Then*** each node **must** list all public top-level types:

- classes
- interfaces
- structs
- enums
- records

#### Public Types Disabled

**Given** “show public types” is disabled  
**When** the diagram is generated  
**Then** no types **must** appear in any node

---

## Command-Line

[!INCLUDE [Console Arguments](./CommandLineArguments.md)]

## Scenarios

---

### Required Argument

Solution File Path (.sln) -> The tool needs to know what solution file to analyze.

```shell
sharpmermaid path/to/solution.sln

```

### Optional Arguments

To match the rules you�ve set, consider adding these CLI flags:  
--output <file> -> Define a custom output file for the Mermaid diagram.

```shell
sharpmermaid path/to/solution.sln --output diagram.md
```

--projects Project1,Project2 -> Specify which projects to include in the diagram.  
--include-urls -> Enable clickable class diagram URLs in project nodes.  
--include-public-types -> Show public top-level types in each project node.

### Automatic Console Warnings

If no projects exist -> Show warning: "No projects found in the solution"  
**If a .csproj file has no .cs files -> Show a warning for that project.`

### Solution With Without Projects

**Given** the solution has no projects  
**When** the diagram is generated  
**Then** the diagram should have no nodes or dependencies  
**And** And the title should be the solution name

---

### Solution With Single Project

**Given** the solution has a single project  
**And** the project has at least one source file  
**When** the diagram is generated  
**Then** the diagram should include one node  
**And** the diagram should have a title same as the solution name  
**And** the diagram should include a node for the project in the solution  
**And** the diagram should include a url to the projects class diagram

---

### Multiple Projects Without Dependencies

**Given** the solution has multiple projects with no dependencies  
**And** each project has at least one source file
**When** the diagram is generated  
**Then** the diagram should include one node per project  
**And** the diagram should have a title same as the solution name  
**And** the diagram should include a url to the projects class diagram

---

### Multiple Projects With Dependencies

**Given** the solution has multiple projects with dependencies  
**And** each project has at least one source file  
**When** the diagram is generated  
**Then** the diagram should include one node per project  
**And** arrows should represent the dependencies between project nodes  
**And** each should include a url to the projects class diagram  
**And** the diagram should have a title same as the solution name  
**And** the diagram should include a url to the projects class diagram

---
