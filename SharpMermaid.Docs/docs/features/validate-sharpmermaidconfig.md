---
uid: features.validate-mermaidconfig
title: validate mermaidconfig.json
---

# Validate mermaidconfig.json

## Description

As a Developer  
I want the `mermaidconfig.json` file to be validated using a standardized schema  
So that I can detect structural or semantic errors early and ensure the configuration is safe to process

## Rules

If the configuration is invalid:

- The system must display an error message:  
  **"Error: Invalid configuration in '{path}/{file}' - {validationMessage}"**
- The system must exit with code [76](<xref:features.process-exit#76---configuration-error>)

### Root

- Must contain a `SolutionPath` property
  - Must not be empty
  - Must end in `.sln`
- Must contain a `OutputDirectory` property
  - Must not be empty
- Must contain a `FileType` property
  - Must not be empty
  - Must be `.mmd` or `.md`
- Must contain a `Diagrams` property
  - Must be an array
  - Must be a non-empty array

### Diagrams

- Must be a non-empty array
- Each item must:
  - Be an object with exactly one property
  - The property name must be a supported diagram type (`PhysicalProjectDiagram`)

### PhysicalProjectDiagram

- Must contain a **FileName** property
  - Must not be empty
  - Must not include a file extension
  - Must not contain invalid filename characters (e.g. `\/:*?"<>|`)
- Optional properties:
  - `OutputDirectory` (a string, if specified)
  - `FileType` (must be `.mmd` or `.md`)
  - `TopLevelPublicTypes` and `ClassDiagramLinks` (booleans, default: `false`)
- If `ClassDiagramLinks` is `true`, then `BaseUrl`:
  - Must be present
  - Must be a valid absolute URL

## Scenarios

### Root: SolutionPath is missing

Given a mermaidconfig.json missing the SolutionPath property

When validation runs

Then the system must display:
"Error: Invalid configuration in './mermaidconfig.json' - Missing required property: SolutionPath"
And the system must exit with code 76

### Root: SolutionPath is empty

Given "SolutionPath": ""

When validation runs

Then the system must display:
"Error: Invalid configuration in './mermaidconfig.json' - SolutionPath must be a non-empty string"
And the system must exit with code 76

### Root: SolutionPath does not end in .sln

Given "SolutionPath": "./src/project.csproj"

When validation runs

Then the system must display:
"Error: Invalid configuration in './mermaidconfig.json' - SolutionPath must point to a .sln file"
And the system must exit with code 76

### Root: OutputDirectory is missing

Given a mermaidconfig.json without OutputDirectory

When validation runs

Then the system must display:
"Error: Invalid configuration in './mermaidconfig.json' - Missing required property: OutputDirectory"
And the system must exit with code 76

### Root: OutputDirectory is empty

Given "OutputDirectory": ""

When validation runs

Then the system must display:
"Error: Invalid configuration in './mermaidconfig.json' - OutputDirectory must be a non-empty string"
And the system must exit with code 76

### Root: Filetype is missing

Given a mermaidconfig.json without Filetype

When validation runs

Then the system must display:
"Error: Invalid configuration in './mermaidconfig.json' - Missing required property: Filetype"
And the system must exit with code 76

### Root: Empty Filetype

Given "Filetype": ""

When validation runs

Then the system must display:
"Error: Invalid configuration in './mermaidconfig.json' - Filetype must be a non-empty string"
And the system must exit with code 76

### Root: FileType is not .mmd or .md

Given "FileType": ".svg"

When validation runs

Then the system must display:
"Error: Invalid configuration in './mermaidconfig.json' - FileType must be one of: .mmd, .md"
And the system must exit with code 76

### Root: Diagrams is missing

Given no Diagrams property is present

When validation runs

Then the system must display:
"Error: Invalid configuration in './mermaidconfig.json' - Missing required property: Diagrams"
And the system must exit with code 76

### Root: Diagrams is not an array

Given "Diagrams": {}

When validation runs

Then the system must display:
"Error: Invalid configuration in './mermaidconfig.json' - Diagrams must be an array"
And the system must exit with code 76

### Root: Diagrams is empty

Given "Diagrams": []

When validation runs

Then the system must display:
"Error: Invalid configuration in './mermaidconfig.json' - Diagrams must contain at least one item"
And the system must exit with code 76

### Diagrams: object has more than one property

Given a diagram object with two keys

{ "PhysicalProjectDiagram": {}, "SomeOtherDiagram": {} }

When validation runs

Then the system must display:
"Error: Invalid configuration in './mermaidconfig.json' - Diagram object must have exactly one property"
And the system must exit with code 76

### Diagrams: Diagrams type not recognized

Given { "UnknownDiagram": { "FileName": "Foo" } }

When validation runs

Then the system must display:
"Error: Invalid configuration in './mermaidconfig.json' - Unsupported diagram type: UnknownDiagram"
And the system must exit with code 76

### PhysicalProjectDiagram: Missing FileName

Given

{ "PhysicalProjectDiagram": {} }

When validation runs

Then the system must display:
Error: Invalid configuration in './mermaidconfig.json' - Missing required property: FileName
And the system must exit with code 76

### PhysicalProjectDiagram: FileName is empty

Given "FileName": ""
When validation runs
Then the system must display:
Error: Invalid configuration in './mermaidconfig.json' - FileName must not be empty
And the system must exit with code 76

### PhysicalProjectDiagram: FileName includes file extension

Given "FileName": "Diagram.md"

When validation runs

Then the system must display:
Error: Invalid configuration in './mermaidconfig.json' - FileName must not include file extension
And the system must exit with code 76

### PhysicalProjectDiagram: FileName contains invalid characters

Given "FileName": "My/File|Name"

When validation runs

Then the system must display:
Error: Invalid configuration in './mermaidconfig.json' - FileName contains invalid characters
And the system must exit with code 76



{
  "$schema": "https://json-schema.org/draft/2020-12/schema",
  "$id": "https://example.com/schemas/mermaidconfig.schema.json",
  "title": "Mermaid Config Schema",
  "type": "object",
  "required": ["SolutionPath", "OutputDirectory", "FileType", "Diagrams"],
  "properties": {
    "SolutionPath": {
      "type": "string",
      "minLength": 1,
      "pattern": ".*\\.sln$",
      "description": "Path to the .sln file."
    },
    "OutputDirectory": {
      "type": "string",
      "minLength": 1,
      "description": "Output directory for diagrams."
    },
    "FileType": {
      "type": "string",
      "enum": [".mmd", ".md"],
      "description": "File type of generated diagrams."
    },
    "Diagrams": {
      "type": "array",
      "minItems": 1,
      "items": {
        "type": "object",
        "minProperties": 1,
        "maxProperties": 1,
        "additionalProperties": false,
        "properties": {
          "PhysicalProjectDiagram": {
            "$ref": "#/definitions/PhysicalProjectDiagram"
          }
        }
      }
    }
  },
  "definitions": {
    "PhysicalProjectDiagram": {
      "type": "object",
      "properties": {
        "OutputDirectory": {
          "type": "string",
          "description": "Optional override for diagram output directory."
        },
        "FileName": {
          "type": "string",
          "minLength": 1,
          "pattern": "^[^\\\\/:*?\"<>|]+$",
          "not": {
            "pattern": ".*\\.[a-zA-Z0-9]+$"
          },
          "description": "File name without extension."
        },
        "FileType": {
          "type": "string",
          "enum": [".mmd", ".md"],
          "description": "Optional override for file type."
        },
        "TopLevelPublicTypes": {
          "type": "boolean",
          "default": false
        },
        "ClassDiagramLinks": {
          "type": "boolean",
          "default": false
        },
        "BaseUrl": {
          "type": "string",
          "format": "uri",
          "description": "Base URL for class diagram links. Required if ClassDiagramLinks is true."
        }
      },
      "required": ["FileName"],
      "if": {
        "properties": {
          "ClassDiagramLinks": { "const": true }
        },
        "required": ["ClassDiagramLinks"]
      },
      "then": {
        "required": ["BaseUrl"]
      }
    }
  }
}
