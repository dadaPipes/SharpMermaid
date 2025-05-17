# Class Diagram

## Description

As a developer,
I want to generate a class diagram for a project,
So that I can visualize the relationships between classes and understand their dependencies

## Rules

- Diagram **must** have a title same as the project name
- Each class node **must** include a url to the source file
- The diagram **may** represent inheritance, dependencies, and associations

### Project With Single Class

**Given** a project contains one .cs file  
**When** the diagram is generated  
**Then** the diagram should include a node for the .cs file  
**And** the diagram should have a title same as the project name  
**And** the class node should include a url to the source file

---

### Project With Multiple Classes & Relationships

**Given** a project contains multiple .cs files  
**And** some classes inherit from others or depend on other classes  
**When** the diagram is generated  
**Then** each class should be represented as a node  
**And** arrows should indicate relationships
