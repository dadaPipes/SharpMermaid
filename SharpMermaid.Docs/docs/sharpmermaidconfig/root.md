---
uid: sharpmermaidconfig.root
title: validate sharpmermaidconfig root
---

# Validate sharpmermaidconfig root

## Description

As a Developer  
I want to validate the root of the sharpmermaidconfig.json  
So that I can ensure basic configuration structure and types are correct before proceeding with diagram generation

## Example

```json
{
  "SolutionPath": "./TestSolution.sln",
  "OutputDirectory": "./Diagrams",
  "FileType": ".mmd",
  "Diagrams": []
}
```

## Properties

### `SolutionPath`

Path to the solution file.

**Type**: `string`  
**Required**: yes  
**Default**: -  
**Validation**:

- [Must not be empty](#solutionpath-is-empty-acceptance-test)
- [Must end with .sln](#solutionpath-does-not-end-with-sln-acceptance-test)

### `OutputDirectory`

Output directory for all generated diagrams (overridable per diagram).

**Type**: string  
**Required**: yes  
**Default**: -
**Validation**:

- [Must not be empty](#outputdirectory-is-empty-acceptance-test)
- [Must be a valid directory path](#outputdirectory-is-not-a-valid-path-acceptance-test)

### `FileType`

File type for all generated diagrams.

**Type**: string  
**Required**: yes  
**Default**: -  
**Supported**: `.mmd` and `.md`
**Validation**:

- [Must be either .mmd or .md](#filetype-is-not-mmd-or-md-acceptance-test)
- [Must start with a dot (.)](#filetype-does-not-start-with-a-dot-acceptance-test)

### `Diagrams`

An array of diagram configurations.  
Each item must contain exactly one property,  
where the key is the diagram type and the value is its specific configuration.

**Type**: array  
**Required**: yes  
**Default**: -  
**Supported diagram types**:
**Validation**:

- [Must not be empty](#diagrams-is-empty-acceptance-test)
- [Each item must contain exactly one property](#diagram-entry-contains-more-than-one-property-acceptance-test)
- [Diagram type must be supported](#diagram-type-is-unsupported-acceptance-test)

**Example**:

```json
{
  "Diagrams": [
    {
      "PhysicalProjectDiagram": {}
    }
  ] 
}
```

## Scenarios

---

### SolutionPath is empty (Acceptance test)

**Given** the user provides an empty SolutionPath  
**When** the system validates the configuration  
**Then** the console must display:  
**"Error: Invalid JSON in file '{`path to sharpmermaidConfig.json`}': SolutionPath is required"**
**And** the process must exit with code 4

---

### SolutionPath does not end with .sln (Acceptance test)

**Given** the user provides SolutionPath "project.txt"  
**When** the system validates the configuration  
**Then** the console must display:  
**"Error: Invalid JSON in file '{`path to sharpmermaidConfig.json`}': SolutionPath must point to a .sln file"**  
**And** the process must exit with code 4

---

### OutputDirectory is empty (Acceptance test)

**Given** the user provides an empty OutputDirectory  
**When** the system validates the configuration  
**Then** the console must display:  
**"Error: Invalid JSON in file '{`path to sharpmermaidConfig.json`}': OutputDirectory is required"**  
**And** the process must exit with code 4

---

### OutputDirectory is not a valid path (Acceptance test)

**Given** the user provides OutputDirectory with an invalid path (ex: containing illegal characters)  
**When** the system validates the configuration  
**Then** the console must display:  
**"Error: Invalid JSON in file '{`path to sharpmermaidConfig.json`}': OutputDirectory is not a valid directory path"**  
**And** the process must exit with code 4

---

### FileType is missing (Acceptance test)

**Given** the user omits the FileType property  
**When** the system validates the configuration  
**Then** the console must display:  
**"Error: Invalid JSON in file '{`path to sharpmermaidConfig.json`}': FileType is required"**  
**And** the process must exit with code 4

---

### FileType does not start with a dot (Acceptance test)

**Given** the user provides FileType "mmd"  
**When** the system validates the configuration  
**Then** the console must display:  
**"Error: Invalid JSON in file '{`path to sharpmermaidConfig.json`}': FileType must begin with a dot"**  
**And** the process must exit with code 4

---

### FileType is not .mmd or .md (Acceptance test)

**Given** the user provides FileType ".pdf"  
**When** the system validates the configuration  
**Then** the console must display:  
**"Error: Invalid JSON in file '{`path to sharpmermaidConfig.json`}': FileType must be either '.mmd' or '.md'"**  
**And** the process must exit with code 4

---

### Diagrams is empty (Acceptance test)

**Given** the user provides an empty array for Diagrams  
**When** the system validates the configuration  
**Then** the console must display:  
**"Error: Invalid JSON in file '{`path to sharpmermaidConfig.json`}': Diagrams must contain at least one item"**  
**And** the process must exit with code 4

---

### Diagram entry contains more than one property (Acceptance test)

**Given** a diagram entry in Diagrams contains multiple keys  
**When** the system validates the configuration  
**Then** the console must display:  
**"Error: Invalid JSON in file '{`path to sharpmermaidConfig.json`}': Each diagram entry must define exactly one diagram type"**  
**And** the process must exit with code 4

---

### Diagram type is unsupported (Acceptance test)

**Given** the user provides an unsupported diagram type "LogicalLayerDiagram"  
**When** the system validates the configuration  
**Then** the console must display:  
**"Error: Invalid JSON in file '{`path to sharpmermaidConfig.json`}': Unsupported diagram type 'LogicalLayerDiagram'"**  
**And** the process must exit with code 4

---
