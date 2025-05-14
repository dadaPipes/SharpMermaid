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

### Generate Diagram With No Projects

**Given** the solution folder does not contain any projects,
**When** the diagram is generated,
**Then** the diagram should:

- The root folder should be represented as a grouping box, with the same name as the folder.

---

### Generate Diagram with Multiple Projects in the Root Folder

**Given** all projects are in the solution root folder,  
**When** the diagram is generated,  
**Then** the diagram should:

- The root folder should be represented as a grouping box, with the same name as the folder.
- Represent each project as an individual node.

---

### Generate Diagram With Multiple Projects in the Root Folder with Project Dependencies

**Given** all projects are in the solution root folder,
**And** the projects have dependencies,
**When** the diagram is generated,
**Then** the diagram should:

- The root folder should be represented as a grouping box, with the same name as the folder.
- Represent each project as an individual node.
- Include arrows representing dependencies between projects.

---

### Generate Diagram With 1 Project in the Root Folder and Multiple Projects in Subfolders Including Dependencies

**Given** the solution folder contains multiple projects with dependencies  
**And** 1 project is in the root directory
**And** some of those projects are in grouped folders,
**When** the diagram is generated  
**Then** the diagram should:

- The root folder should be represented as a grouping box, with the same name as the folder.
- Represent each project as an individual node.
- Group projects visually according to their folder structure.
- Include arrows representing dependencies between projects.
- Each folder should be represented as a grouping box, with the same name as the folder.

---

### Generate Diagram With multiple Projects in the Root Folder and Multiple Projects in Subfolders Including Dependencies

**Given** the solution folder contains multiple projects with dependencies
**And** multiple projects are in the root directory
**And** some of those projects are in grouped folders,
**When** the diagram is generated
**Then** the diagram should:


- The root folder should be represented as a grouping box, with the same name as the folder.
- Represent each project as an individual node.
- Group projects visually according to their folder structure.
- Include arrows representing dependencies between projects.
- Each folder should be represented as a grouping box, with the same name as the folder.

---
