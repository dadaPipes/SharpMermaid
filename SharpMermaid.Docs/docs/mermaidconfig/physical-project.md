---
uid: mermaidconfig.physicalProjectDiagram
title: mermaidconfig PhysicalProjectDiagram
---

# PhysicalProjectDiagram

## Description

Generates a physical project layout diagram.

## Example

```json
{
  "Diagrams": [
    {
      "PhysicalProjectDiagram": {
        "OutputDirectory": "./Override/Diagrams",
        "FileName": "PhysicalDiagram",
        "FileType": ".mmd",
        "TopLevelPublicTypes": true,
        "ClassDiagramLinks": true,
        "BaseUrl": "https://example.com/"
      }
    }
  ] 
}
```

## Properties

### `OutputDirectory`

Overrides root output path for this diagram.

**Type**: string  
**Required**: no  
**Default**: Inherits from root `OutputDirectory`

**Validation**:

- [If provided, must be a valid directory path](#outputdirectory-is-not-a-valid-path)

### `FileName`

File name for the generated diagram

**Type**: string  
**Required**: yes  
**Default**: -  
**Validation**:

- [Must not be empty](#filename-is-empty)  
- [Cannot contain invalid filename characters](#filename-contains-invalid-characters)
- [Must not use file extension](#filename-has-a-file-extension)

### `TopLevelPublicTypes`

Include top-level public types in each project.

**Type**: boolean  
**Required**: no  
**Default**: `false`
**Validation**:

- [Must be true or false (JSON boolean)](#toplevelpublictypes-is-not-a-boolean)

### `ClassDiagramLinks`

Turn each project node in to a clickable link, that navigates to its class diagram.

**Type**: boolean  
**Required**: no  
**Default**: `false`
**Validation**:

- [Must be true or false (JSON boolean)](#classdiagramlinks-is-not-a-boolean)

### `BaseUrl`

Base URL used to build class diagram links.

**Type**: string  
**Required**: if `ClassDiagramLinks` is `true`  
**Default**: -
**Validation**

- [Required if `Root/ClassDiagramLinks` is `true`](#baseurl-is-required-when-classdiagramlinks-is-enabled)
- [Must be a valid absolute URL (starts with `http://` or `https://`)](#baseurl-is-not-a-valid-url)

## Scenarios

---

### OutputDirectory is not a valid path

**Given** the user provides an invalid OutputDirectory (e.g., contains illegal characters)  
**When** the system validates the configuration  
**Then** the console must display:  
"**Error: Invalid JSON in file '{`path to mermaidConfig.json`}': OutputDirectory is not a valid directory path**"  
**And** the process must exit with code 4

---

### FileName is empty

**Given** the user provides an empty FileName  
**When** the system validates the configuration  
**Then** the console must display:  
"**Error: Invalid JSON in file '{`path to mermaidConfig.json`}': FileName is required**"  
**And** the process must exit with code 4

---

### FileName contains invalid characters

**Given** the user provides FileName "my?file"  
**When** the system validates the configuration  
**Then** the console must display:  
"**Error: Invalid JSON in file '{`path to mermaidConfig.json`}': FileName contains invalid characters**"  
**And** the process must exit with code 4

---

### FileName has a file extension

**Given** the user provides FileName "diagram.mmd"  
**When** the system validates the configuration  
**Then** the console must display:  
"**Error: Invalid JSON in file '{`path to mermaidConfig.json`}': FileName must not have a file extension**"  
**And** the process must exit with code 4

---

### TopLevelPublicTypes is not a boolean

**Given** the user provides a non-boolean value for TopLevelPublicTypes  
**When** the system validates the configuration  
**Then** the console must display:  
"**Error: Invalid JSON in file '{`path to mermaidConfig.json`}': TopLevelPublicTypes must be a boolean**"  
**And** the process must exit with code 4

---

### ClassDiagramLinks is not a boolean

**Given** the user provides a non-boolean value for ClassDiagramLinks  
**When** the system validates the configuration  
**Then** the console must display:  
"**Error: Invalid JSON in file '{`path to mermaidConfig.json`}': ClassDiagramLinks must be a boolean**"  
**And** the process must exit with code 4

---

### BaseUrl is required when ClassDiagramLinks is enabled

**Given** the user enables ClassDiagramLinks but omits BaseUrl  
**When** the system validates the configuration  
**Then** the console must display:  
"**Error: Invalid JSON in file '{`path to mermaidConfig.json`}': BaseUrl is required when ClassDiagramLinks is enabled**"  
**And** the process must exit with code 4

---

### BaseUrl is not a valid URL

**Given** the user provides an invalid URL for BaseUrl  
**When** the system validates the configuration  
**Then** the console must display:  
"**Error: Invalid JSON in file '{`path to mermaidConfig.json`}': BaseUrl must be a valid absolute URL**"  
**And** the process must exit with code 4

---
