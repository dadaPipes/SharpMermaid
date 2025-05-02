# Physical Project Diagram

## Description

As a developer,  
I want to generate a diagram that accurately reflects the physical arrangement of projects and files on disk,  
So that I can analyze the hierarchy and structure of the solution.

## Rules

[!INCLUDE [shared rules](shared-rules.md)]

## Scenarios

### Generate Diagram With No Projects

**Given** the solution folder does not contain any projects,
**When** the diagram is generated,
**Then** the diagram should:

- Not include any nodes or arrows.

### Generate Diagram With Multiple Projects and Zero Project Dependencies

**Given** the solution folder contains multiple projects with dependencies,  
**When** the diagram is generated,  
**Then** the diagram should:

- Include nodes representing projects.
- Represent the physical hierarchy of files on disk.
- Not include any arrows representing dependencies between projects.

### Generate Diagram With Multiple Projects and Project Dependencies

**Given** the solution folder contains multiple projects with dependencies,  
**When** the diagram is generated,  
**Then** the diagram should:

- Include nodes representing projects.
- Represent the physical hierarchy of files on disk.
- Include arrows representing dependencies between projects.

### Generate Diagram With 1 Project and A Clickable URL

**Given** the solution folder contains a single project
**And** the project include .cs files.
**When** the diagram is generated,
**Then** the diagram should:
- Include a node representing the project.
- Include a clickable URL for the project node, leading to its details.

### Generate Diagram With Multiple Projects and Clickable URLs

**Given** the solution folder contains multiple projects, each with one or more classes,  
**When** the diagram is generated,  
**Then** the diagram should:
- Include a node representing each project.
- Include a clickable URL for each project node, leading to its details.

### Generate Diagram With Multiple Projects and Project Dependencies and Clickable URLs

### Generate Diagram With Multiple Projects and Clickable URLs and Tooltips

Tooltips does not even work in the Mermaid Live Editor, so this one is for another time

### Generate Diagram With Multiple Projects and Project Dependencies and Clickable URLs and Tooltips
