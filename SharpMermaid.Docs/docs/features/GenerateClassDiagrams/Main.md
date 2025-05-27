# Generate Class Diagrams

## Description

As a developer,  
I want to generate a class diagram for each `.csproj` in my solution,  
So that I can visualize the structure, responsibilities, and relationships of types in the project.

## Rules

### General Structure

[!INCLUDE [Shared Diagram Rules](../Shared/DiagramRules.md)]
- Each diagram **must** start with a `classDiagram` declaration
- Each generated diagram **must** have a title matching the project name
- Each class **must** be represented with the `.cs` file name as the node name
- Class nodes **must** display the type name and its kind:
	- Supported kinds:
		- `class` (including regular, abstract, sealed, generic, partial)
		- `interface` (including generic and partial)
		- `struct` (including readonly, partial, and generic)
		- `enum`
		- `record` (including record class, record struct, readonly record struct, generic, and partial)
- Only types from .cs files within the target .csproj **must** be included

### File Organization and Grouping

- Namespaces **must** be used for grouping **when** applicable
- Class nodes **must** be grouped based on their nested folder structure
- Partial types **must** be grouped according to their file path structure within the .csproj
- Dependencies to partial classes **must** link to each individual partial class, rather than a merged representation

### Relationships

Relationships **must** be indicated with usage arrows **when** dependencies exist between classes
	- Supported relationship types include:
		- Inheritance  (<|--) � Extends a class (parent-child relationship)
		- Composition  (*--)  � Has-a relationship (strong ownership; part-whole)
		- Aggregation  (o--)  � Has-a relationship (weaker ownership; shared instance)
		- Association  (-->)  � References or links to another class
		- Dependency   (..>)  � Uses another class but does not own it (loose coupling)
		- Realization  (..|>) � Implements an interface (contract fulfillment)

### Clickable Links

- Each class node **must** include a clickable link to its corresponding source file,  
  formatted as `[BaseUrl] / [Relative Path to Project] / [Source File Name]`

### Styling

- Types with `public` access modifiers **must** have a green edge for clear visibility,  
  formatted as: `style [TypeName] stroke:green,stroke-width:4px`
	- Supported types include: `class`, `interface`, `enum`, `struct`, `record`.
		

TODO: 
How to visualize multiple implemented interfaces?
Should inheritance chains show only direct parent-child links, or full ancestry?
How to differentiate between inherited members and explicitly declared members?

constructors, methods, and properties

**May** ( configurables )    

## Scenarios

---

### Generating Diagrams for Multiple Projects

**Given** a solution containing multiple projects  
**And** each project contains multiple `.cs` files, including files within nested directories 
**When** I generate the diagrams  
**Then** a separate class diagram should be created for each project  
**And** each diagram should have a title matching the project name  
**And** each class node should be named after its .cs file  
**And** each class node should have a clickable URL reflecting its full directory structure, including nested folders

---
