# Init

## Description

As a developer,  
I want to Create a default sharpmermaidconfig.json in the working directory  
So that  I have a ready-to-edit configuration file for diagram generation

## CLI Usage

```shell
dotnet sharpmermaid init
```

## Preconditions

- The file system is available with write access

## Scenarios

---

### Display success message after sharpmermaidconfig.json creation

Given the Developers working directory is {cwd}  
And no sharpmermaidconfig.json exists in {cwd}  

When the Developer runs: dotnet sharpmermaid init

Then the console must display:  
"Created new file 'sharpmermaidconfig.json' at '{cwd}'"  
And exit with code [0](<xref:features.process-exit#0---success>)

---

### Create default sharpmermaidconfig.json

Given the Developers working directory is {cwd}  
And no sharpmermaidconfig.json exists in {cwd}  

When the Developer runs: dotnet sharpmermaid init  

Then a file named sharpmermaidconfig.json must exist in {cwd}  
And its content must match the [***default mermaidconfig.json***](xref:mermaidconfig.default):
[!include[mermaidconfig.json](../../docs/mermaidconfig/default.md)]

---

### Display error message if sharpmermaidconfig.json already exists

Given the Developers working directory is {cwd}  
And a file named sharpmermaidconfig.json already exists in {cwd}  

When the Developer runs: dotnet sharpmermaid init  

Then the system must display:  
"Error: A 'sharpmermaidconfig.json' file already exists at '{cwd}/sharpmermaidconfig.json'"  
And exit with code [73](<xref:features.process-exit#73---file-already-exists>)
