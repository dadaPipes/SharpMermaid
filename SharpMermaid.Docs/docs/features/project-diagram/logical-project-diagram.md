# Logical Project Diagram

## Description

As a developer,  
I want to generate a package-style diagram that groups projects by their folder structure,  
So that I can understand how the solution is logically organized and interconnected.

## Rules

[!INCLUDE [shared rules](shared-rules.md)]
- Nodes **must** be grouped visually according to their folder structure.
- Grouping boxes **must** be based on the folder names containing the projects.
- The diagram **must not** include `bin`, `obj`, or non-project folders.

## Scenarios

---

### Empty Solution

**Given** the solution contains no projects  
**When** the diagram is generated  
**Then** the diagram should show a root grouping box with the solution folder name  
**And** no nodes or dependencies

---

### Root Projects Only

**Given** all projects are in the solution root folder  
**When** the diagram is generated  
**Then** the diagram should show one node per project  
**And** the root folder should be a grouping box  

---

### Root Projects With Dependencies

**Given** all projects are in the solution root folder  
**And** some projects depend on each other  
**When** the diagram is generated  
**Then** the diagram should show one node per project  
**And** arrows should represent the dependencies  
**And** the root folder should be a grouping box  

---

### Mixed Folder Structure With 1 Root Project

**Given** one project is in the root folder  
**And** other projects are in subfolders  
**When** the diagram is generated  
**Then** the diagram should show nodes grouped by folder  
**And** each folder should be a grouping box  
**And** arrows should represent dependencies  

---

### Mixed Folder Structure With Multiple Root Projects

**Given** multiple projects are in the root folder  
**And** other projects are in subfolders  
**And** some projects depend on each other  
**When** the diagram is generated  
**Then** the diagram should show all nodes  
**And** group nodes by folder  
**And** draw arrows for dependencies  
**And** skip non-project folders  

---
