# Tooltip Showing description

## Description

As a developer,  
I want to see the project description of when I hover over the node,  
So that I can get a light understanding of the project, without navigating away from the diagram.

## Rules

- A project node **may** contain a description

## Scenarios

### Add Tooltip to Project Node

**Given** a project contains metadata of its description  
**When** the project dependency diagram is generated  
**Then** the diagram should include a tooltip containing the description

### Project does not have a description

**Given** a project does not contains metadata of its description  
**When** the project dependency diagram is generated  
**Then** the diagram should include a tooltip containing the "no description available"  
**And** have an icon of a red dot, to indicate that no description is available
