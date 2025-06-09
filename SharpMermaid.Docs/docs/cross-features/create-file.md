---
uid: cross-features.create-file
title: scenario
---

# Create File

## Description

As a developer,  
I want the system to create a new file when needed and overwrite existing files when specified,  
So that I can ensure file operations are handled efficiently without unintended conflicts.

## Dependencies

- ***<xref:cross-features.process-exit>***

## Preconditions

- The file system is available with write access

## Rules

### Create new file

If no file, with a specified `FileName` and `FileType` exists at `OutputDirectory`,  
the system **must** create a new file with the specified `FileName` and `FileType` at `OutputDirectory`  
[***see: scenario***](#create-new-file-system-test)  
and the system **must** display: **"Created new file '{fileName}.{FileType}' at '{OutputDirectory}'"**  
and the process **must** exit with code `0`  
[***see: process exit rule***](xref:cross-features.process-exit#0---success)

### Override existing file

If a file with the same `FileName` and `FileType` already exists at `OutputDirectory`,  
the system **must** overwrite it  
[***see: scenario***](#override-file-system-test)  
and the system **must** display: **"Overwriting existing file at '{OutputDirectory}/{FileName}{FileType}"**  
and the process **must** exit with code `0`  
[***see: process exit rule***](xref:cross-features.process-exit#0---success)

## Scenarios

### Create new file (System Test)

### Override file (System Test)
