---
uid: mermaidconfig.default
title: default mermaidconfig.json
---

```json
{
  "SolutionPath": "./TestSolution.sln",
  "OutputDirectory": "./Diagrams",
  "FileType": ".mmd",
  "Diagrams": [
   {
     "PhysicalProject": {
        "OutputDirectory": "./Override/Diagrams",
        "FileName": "PhysicalDiagram",
        "FileType": ".mmd",
        "TopLevelPublicTypes": true,
        "ClassDiagramLinks": true,
        "BaseUrl": "https://example.com/"
     }
   }
  ]
}
```
