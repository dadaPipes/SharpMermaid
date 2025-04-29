# Physical Dependencies

## Description

As a developer,  
I want to generate a diagram that accurately reflects the physical arrangement of projects and files on disk,  
So that I can analyze the hierarchy and structure of the solution.

## Rules

[!INCLUDE [shared rules](shared-rules.md)]

## Scenarios

### Generate Diagram Representing Physical Structure

**Given** the solution folder contains multiple projects with dependencies  
**When** the diagram is generated  
**Then** the diagram should:

- Include nodes representing projects.
- Represent the physical hierarchy of files on disk.
- Include arrows representing dependencies between projects.
