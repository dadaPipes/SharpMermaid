# Generate Class Diagrams

## Description

As a developer,  
I want to generate a class diagram for each project in my solution,  
So that I can visualize the relationships between classes and understand their dependencies.

## Rules

- Each generated diagram **must** have a title matching the project name.
- Each class node **must** include a clickable link to the corresponding source file.
- The diagram **may** represent inheritance, dependencies, and associations
- The diagram **may** represent:
	- Inheritance
	- Dependencies
	- Assosiations between classes

### Generating Diagrams for Multiple Projects

**Given** a solution containing multiple projects  
**And** each project contains multiple .cs files  
**When** I generate the diagrams  
**Then** a separate class diagram should be created for each project  
**And** each diagram should have a title matching the project name  
**And** each class node should be named after its .cs file  
**And** the class node should include a clickable URL to the source file.

---
