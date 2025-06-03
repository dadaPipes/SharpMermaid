# Init

## Description

**As a** developer,  
**I want** to Create a default `mermaidconfig.json` in the current directory  
**So that**  I have a ready-to-edit configuration file for diagram generation

## CLI Usage

```shell
dotnet sharpmermaid init
```

## Rules

### Create Default Config File

- If `mermaidconfig.json` does not exist in the {cwd}:  
    The system must create a default `mermaidconfig.json` in {cwd}  
    [(see: scenario U1)](#create-default-config-file-u1)  
    and the system **must** display:  
    "Created new file 'mermaidconfig.json' at '{cwd}'"  
    and the process **must** exit with code 0  
    [(see: scenario A1)](#create-default-config-file-a1)

- If `mermaidconfig.json` exists in {cwd}:  
  System **must** abort  
  [(see: scenario U2)](#create-default-config-file-u2)  
  and the system **must** display:  
  "Error: A 'mermaidconfig.json' file already exists at '{cwd}/mermaidconfig.json'"  
  and the process **must** exit with code 1  
  [(see: scenario A2)](#create-default-config-file-a2)

### File Write Failures

- If the system does not have permission to write `mermaidconfig.json` in `{cwd}`:  
  The system **must** display:  
  "Error: Cannot write to '{cwd}/mermaidconfig.json' — permission denied."  
  and must exit with code 2  
  [(see: scenario A3)](#create-default-config-file-a3)

- If a general I/O error occurs (file is locked, disk error):  
  The system **must** display:  
  "Error: Failed to write file at '{cwd}/mermaidconfig.json': {message}"  
  and must exit with code 3  
  [(see: scenario A4)](#create-default-config-file-a4)

## Default mermaidconfig.json

```json
{
  "SolutionPath": "./Solution.sln",
  "OutputDirectory": "./Diagrams",
  "Diagrams": [
    {
      "DiagramType": "PhysicalProject",
      "FileName": "PhysicalDiagram",
      "FileType": ".mmd",
      "TopLevelPublicTypes": false,
      "ClassDiagramLinks": false,
      "BaseUrl": "https://example.com/"
    }
  ]
}
```

## Scenarios

---

### Create Default Config File A1

**Given** the developer’s current working directory is `{cwd}`  
**And** no `mermaidconfig.json` exists at `{cwd}`

**When** the developer runs:

```shell
sharpmermaid init
```

**Then** the console display:

```shell
Created new file 'mermaidconfig.json' at '{cwd}'
```

**And** the process exit with code 0

---

### Create Default Config File U1

**Given** the working directory is `{cwd}`  
**And** no `mermaidconfig.json` exists in `{cwd}`  

**When** the system attempts to create a default configuration  

**Then** a file named `mermaidconfig.json` exist in `{cwd}`  
**And** the content must match the structure in [default config structure](#default-mermaidconfigjson)

---

### Create Default Config File A2

**Given** the developer’s current working directory is `{cwd}`  
**And** a `mermaidconfig.json` exists at `{cwd}`

**When** the developer runs:

```shell
sharpmermaid init
```

**Then** the console display:

```shell
Error: A 'mermaidconfig.json' file already exists at '{cwd}/mermaidconfig.json'
```

**And** the process must exit with code 1

---

### Create Default Config File U2

**Given** the working directory is `{cwd}`  
**And** a `mermaidconfig.json` exists in `{cwd}`

**When** the system attempts to create a default configuration

**Then** the execution stop

---

### Create Default Config File A3

**Given** the current directory `{cwd}` is not writable (restricted permissions)

**When** the developer runs:

```shell
sharpmermaid init
```

**Then** the console displays:

```shell
Error: Cannot write to '{cwd}/mermaidconfig.json' — permission denied.
```

**And** the process exit with code 2

---

### Create Default Config File A4

**Given** a general I/O failure occurs (file is locked or disk is full) when writing to `{cwd}`
When the developer runs:

**When** the developer runs:

```shell
sharpmermaid init
```

**Then** the console displays:

```shell
Error: Failed to write file at '{cwd}/mermaidconfig.json': {error message}
```

**And** the process must exit with code 3

---
