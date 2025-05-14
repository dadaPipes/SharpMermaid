# Physical Project Diagram

## Description

As a developer,  
I want to generate a diagram that reflects the solution structure on disk,
So that I can analyze and visualize project organization and dependencies.

## Rules

[!INCLUDE [shared rules](shared-rules.md)]

## Scenarios

---

### Empty Solution

**Given** the solution has no projects  
**When** the diagram is generated  
**Then** the diagram should have no nodes or dependencies  
**And** the title should be the solution name  

---

### Single Project With Clickable URL

**Given** the solution has a single project  
**When** the diagram is generated  
**Then** the diagram should include one node  
**And** the node should have a clickable URL  

---

### Multiple Projects With No Dependencies

**Given** the solution has multiple projects with no dependencies  
**When** the diagram is generated  
**Then** the diagram should include one node per project  
**And** no arrows should exist between them  
**And** all nodes should have clickable URLs  

---

### Multiple Projects With Dependencies

**Given** the solution has multiple projects with dependencies  
**When** the diagram is generated  
**Then** the diagram should include one node per project  
**And** arrows should represent the dependencies  
**And** all nodes should have clickable URLs  

---