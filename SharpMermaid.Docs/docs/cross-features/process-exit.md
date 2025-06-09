---
uid: cross-features.process-exit
title: process exit
---

# Process Exit

## Description

As a developer  
I want the process to terminate with standardized exit codes and messages  
So that I can handle success and failure conditions consistently

## Rules

---

### 0 - Success

When a command completes successfully:

- **May** display a message indicating the operation succeeded: **"{sucessMessage}"**
- The system **must** exit with code 0  
[***see: scenario***](#scenario-successful-execution)

---

### 1 - General Error (Catchall)

If an unexpected error occurs that does not match a specific exit code:  

- The system **must** display: **"Error: An unexpected failure occurred - {errorMessage}"**  
- The system **must** exit with code 1  
[***see: scenario***](#scenario-general-error-catchall)

---

### 73 - File Already Exists

When a file already exists at the target path and should not be overwritten:  

- The system **must** display: **"Error: A '{fileName}' file already exists at '{path}/{fileName}'"**  
- The system **must** exit with code 73  
[***see: scenario***](#scenario-file-already-exists)

---

### 74 - General I/O Error

If a general I/O error occurs (disk full, file locked, etc ..):

- The system **must** display: **"Error: Failed to write file at '{path}/{file}': {errorMessage}"**  
- The system **must** exit with code 74  
[***see: scenario***](#scenario-general-io-error)

---

### 75 - Configuration Missing

If the application expects a configuration file but it does not exist:

- The system **must** display: **"Error: Required configuration file '{fileName}' is missing"**  
- The system **must** exit with code 75  
[***see: scenario***](#scenario-configuration-missing)

---

### 76 - Configuration Error

If a configuration file **exists but is invalid**:

- The system **must** display: **"Error: Invalid configuration in '{path}/{file}' - {validationMessage}"**  
- The system **must** exit with code 76  
[***see: scenario***](#scenario-configuration-error-malformed-or-invalid-file)

---

### 77 - Permission Denied

If the system does not have permission to write to the target path:

- The system **must** display: **"Error: Cannot write to '{path}/{file}' - No write permission"**  
- The system **must** exit with code 77  
[***see: scenario***](#scenario-permission-denied)

---

## Scenarios

---

### Scenario: Successful Execution

Given the application executes a command successfully  
And no errors occur during the operation  

When the process completes  

Then the system **may** display: **"{successMessage}"**  
And the system **must** exit with code 0

---

### Scenario: General Error (Catchall)

Given an unknown failure occurs during execution  
And the error does not match any specific exit code condition  

When the application fails unexpectedly  

Then the system **must** display: **"Error: An unexpected failure occurred - {errorMessage}"**  
And the system **must** exit with code 1

---

### Scenario: File Already Exists

Given a file named `{fileName}` already exists at `{path}`  
And the operation **must not** overwrite existing files  

When the application attempts to write to `{path}/{fileName}`  

Then the system **must** display: **"Error: A '{fileName}' file already exists at '{path}/{fileName}'"**  
And the system **must** exit with code 73

---

### Scenario: General I/O Error

Given an I/O issue occurs while trying to write to `{path}/{file}`  
And possible reasons include disk space exhaustion or file locks  

When the application attempts to write data  

Then the system **must** display: **"Error: Failed to write file at '{path}/{file}': {errorMessage}"**  
And the system **must** exit with code 74

---

### Scenario: Configuration Missing

Given the system requires a configuration file named `{fileName}`  
And the file does **not** exist  

When the application starts and tries to load the configuration  

Then the system **must** display: **"Error: Required configuration file '{fileName}' is missing"**  
And the system **must** exit with code 75

---

### Scenario: Configuration Error (Malformed or Invalid File)

Given the system loads the configuration from `{path}/{file}`  
And the configuration file **is present but invalid or improperly formatted**  

When the application attempts to parse the configuration file  

Then the system **must** display: **"Error: Invalid configuration in '{path}/{file}' - {validationMessage}"**  
And the system **must** exit with code 76

---

## Scenario: Permission Denied

Given the system needs to write to `{path}/{file}`  
And the application **does not** have the required permissions  

When the application attempts to write to the file  

Then the system **must** display: **"Error: Cannot write to '{path}/{file}' - No write permission"**  
And the system **must** exit with code 77

---
