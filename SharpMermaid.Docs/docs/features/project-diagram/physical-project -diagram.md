# Physical Project Diagram

## Description

As a developer,  
I want to generate a diagram that reflects the solution structure on disk,  
So that I can analyze and visualize project organization and dependencies.  

## Rules

[!INCLUDE [shared rules](shared-rules.md)]

## Scenarios

---

### Solution With Without Projects

**Given** the solution has no projects  
**When** the diagram is generated  
**Then** the diagram should have no nodes or dependencies  
**And** And the title should be the solution name

---

### Solution With Single Project

**Given** the solution has a single project  
**And** the project has at least one source file  
**When** the diagram is generated  
**Then** the diagram should include one node  
**And** the diagram should have a title same as the solution name  
**And** the diagram should include a node for the project in the solution  
**And** the diagram should include a url to the projects class diagram

---

### Multiple Projects Without Dependencies

**Given** the solution has multiple projects with no dependencies  
**And** each project has at least one source file
**When** the diagram is generated  
**Then** the diagram should include one node per project  
**And** the diagram should have a title same as the solution name  
**And** the diagram should include a url to the projects class diagram

---

### Multiple Projects With Dependencies

**Given** the solution has multiple projects with dependencies  
**And** each project has at least one source file  
**When** the diagram is generated  
**Then** the diagram should include one node per project  
**And** arrows should represent the dependencies between project nodes  
**And** each should include a url to the projects class diagram  
**And** the diagram should have a title same as the solution name  
**And** the diagram should include a url to the projects class diagram

---
