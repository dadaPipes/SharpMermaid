# Save Physical Project Diagram  

## Description

As a developer,  
I need to save the diagram I generated,  
So I can easily reference my project's structure and dependencies later.  

## Saving Rules  

1. The diagram **must** be saved as a `.md` file containing a Mermaid.js code block.  
2. The file **must** always be overwritten to reflect the latest solution state.  
3. The default path **must** be the directory where the command is executed.  
   - **3.a** If the developer specifies a custom path in the console, the diagram **must** be saved accordingly.  
4. The default filename **must** be `PhysicalProjectDiagram.md`.  
   - **4.a** If the developer specifies a custom filename in the console, the diagram **must** be saved accordingly.  
5. The console **must** display a confirmation message showing the saved file location.  

## Edge Case Handling  

**Non-Existent Path** -> Display `"Error: The specified path does not exist."`  
**Invalid Characters in Filename** -> Display `"Error: Invalid filename detected."`  
**Insufficient Write Permissions** -> Display `"Error: Unable to save diagram. Check permissions."`  

---
