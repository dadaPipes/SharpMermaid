# Init

## Description

**As a** developer,  
**I want** to Create a default `mermaidconfig.json` in the current directory  
**So that**  I have a ready-to-edit configuration file for diagram generation

## CLI Usage

```shell
sharpmermaid init --solution {string} --output {string}
```

## Required arguments  

`--solution {string}`  
Path to the solution file (relative to the current directory)  

`--output {string}`  
Path to the output directory for generated diagrams (relative to the current directory)  

## Rules

- **Must** create a default `mermaidconfig.json` in the current directory
[***see: scenario***](#create-default-config-file)

- Must pre-populate the file with:
  - `solution`: the provided solution path
  - `output`: the provided output directory  
[***see: scenario***](#create-default-config-file)

- **Must** check if `mermaidconfig.json` already exists in the current directory  
If it does, execution must stop and the console must display:  
"Error: A `mermaidconfig.json` file already exists at '{cwd}/mermaidconfig.json'"
[***see: scenario***](#config-file-already-exist)

## Scenarios

---

### Create Default Config File

**Given** the developer’s current working directory is `{cwd}`  
**And** a solution file `TestSolution.sln` exists at `{cwd}/TestSolution.sln`  
**And** the developer specifies:  

- Solution: `./TestSolution.sln`
- Output: `./TestSolution`

**When** the developer runs:

```shell
sharpmermaid init --solution ./TestSolution.sln --output ./TestSolution
```

**Then** the system must create a new `mermaidconfig.json` file at `{cwd}/mermaidconfig.json`  
**And** the console must display: "Created new file 'mermaidconfig.json' at '{cwd}/mermaidconfig.json'"  
**And** the `mermaidconfig.json` file must contain:

```json
{
  "solution": "./TestSolution.sln",
  "output": "./TestSolution"
}
```

---

### Config File Already Exist

**Given** the developer’s current working directory is `{cwd}`  
**And** a solution file `TestSolution.sln` exists at `{cwd}/TestSolution.sln`  
**And** the developer specifies:  

- Solution: `./TestSolution.sln`
- Output: `./TestSolution`

**And** `mermaidconfig.json` file already exists at `{cwd}/mermaidconfig.json`

**When** the developer runs:

```shell
sharpmermaid init --solution ./TestSolution.sln --output ./TestSolution
```

**Then** the execution **must** stop and the console **must** display:  
"Error: A `mermaidconfig.json` file already exists at '{cwd}/mermaidconfig.json'"

---
