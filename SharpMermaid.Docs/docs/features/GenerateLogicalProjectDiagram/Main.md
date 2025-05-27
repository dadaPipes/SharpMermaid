# Logical Project Diagram

## Description

As a developer,  
I want to generate a package-style diagram that groups projects by their folder structure,  
So that I can understand how the solution is logically organized and interconnected.

## Rules

---

[!INCLUDE [Shared Diagram Rules](../Shared/DiagramRules.md)]

---
 
Diagram **must** start with a `graph` Declaration

---

The console **must** display a warning message "No projects found in the solution", when the solution contains no `.csproj` files

---

Diagram **must** have a title same as the solution name

---

The diagram **may** include a node for each project in the solution, with the same name as the project


Relationships **must** be indicated with usage arrows **when** dependencies exist between projects,  
  including bi-directional dependencies
- A warning **must** be issued in the console **when** a bi-directional dependency is detected
- Project nodes **must** be grouped into `subgraphs` based on their folder structure **when** multiple projects exist inside grouping folders

### Configuration

- Project nodes that has source files, **may** include a url to the projects class diagram
- Each project node in the diagram **may** display all public types defined within the project,  
  including:
	- classes
	- interfaces
	- structs
	- enums
	- records

## Scenarios

---

### Solution With Without Projects

**Given** the solution contains no projects  
**When** the diagram is generated  
**Then** the title should be the solution name  
**And** the diagram should have no nodes or dependencies

---

### Solution With Root Projects Only

**Given** all projects are in the solution root folder  
**When** the diagram is generated  
**Then** the title should be the solution name  
**And** the diagram should include a node for each project

---

### Solution With Root Projects Only With Dependencies

**Given** all projects are in the solution root folder  
**And** some projects depend on each other  
**When** the diagram is generated  
**Then** the title should be the solution name  
**And** the diagram should include a node for each project  
**And** arrows should represent the dependencies between projects  

---

### Mixed Folder Structure With 1 Root Project

**Given** one project is in the root folder  
**And** the other projects are in a subfolder  
**And** the root project has dependencies to the other projects  
**When** the diagram is generated  
**Then** the title should be the solution name  
**And** the diagram should include a node for each project  
**And** nodes should be grouped into subgraphs based on their folder structure  
**And** And arrows should represent project dependencies

---

### Mixed Folder Structure With Multiple Root Projects

**Given** multiple projects are in the root folder  
**And** other projects are in subfolders  
**And** some projects depend on each other  
**When** the diagram is generated  
**Then** the title should be the solution name  
**And** the diagram should include a node for each project  
**And** nodes should be grouped into subgraphs based on their folder structure  
**And** And arrows should represent project dependencies

---
