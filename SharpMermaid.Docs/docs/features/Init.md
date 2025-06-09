# Init

## Description

As a developer,  
I want to Create a default `mermaidconfig.json` in the current directory  
So that  I have a ready-to-edit configuration file for diagram generation

## Dependencies

- ***<xref:cross-features.process-exit>***
- ***<xref:mermaidconfig.default>***

## CLI Usage

```shell
dotnet sharpmermaid init
```

## Rules

### mermaidconfig.json does not exist

If `mermaidconfig.json` does not exist in {`cwd`}, the system **must** create a default `mermaidconfig.json` in {`cwd`}  
[***see: scenario***](#mermaidconfigjson-does-not-exist-system-test)  
and the system **must** display: **Created new file 'mermaidconfig.json' at '{`cwd`}'**  
and the process **must** exit with code 0  
[***see: process exit rule***](xref:cross-features.process-exit#0---success)

### mermaidconfig.json exists

If `mermaidconfig.json` exists in {`cwd`}, the system **must** exit with code 73  
[***see: process exit rule***](xref:cross-features.process-exit#73---file-already-exists)

## Scenarios

---

### mermaidconfig.json does not exist (System Test)

**Given** the working directory is `{cwd}`  
**And** no `mermaidconfig.json` exists in `{cwd}`  

**When** the system attempts to create a default configuration  

**Then** a file named `mermaidconfig.json` exist in `{cwd}`  
And its content must match the following default configuration:
[!include[mermaidconfig.json](../../docs/mermaidconfig/default.md)]

---

### mermaidconfig.json exist (System Test)

**Given** the working directory is `{cwd}`  
**And** a `mermaidconfig.json` exists in `{cwd}`

**When** the system attempts to create a default configuration

**Then** the execution stop

---
