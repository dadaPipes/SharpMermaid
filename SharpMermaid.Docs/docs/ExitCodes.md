# Exit Codes

## Legend

`{file}` = name of the target file (ex: `mermaidconfig.json`)

`{path}` = absolute or relative path to the directory (ex:  `/home/user/project`)

`{errorMessage}` = system-generated error (ex:  `disk full`, `file is locked`)

---

## 0 — Success

Exit Code: 0  
Description: Command completed successfully  
Message: [varies by command]

---

## 1 —  File Already Exists

Exit Code: 1  
Description: Attempted to create a file that already exists  
Message:  
Error: A '{file}' file already exists at '{path}/{file}'

---

## 2 —  Permission Denied

Exit Code: 2  
Description: Insufficient permissions to write file at target location  
Message:  
Error: Cannot write to '{path}/{file}' — permission denied

---

## 3 —  General I/O Error

Exit Code: 3  
Description: Unhandled I/O failure (e.g., disk full, file is locked)  
Message:  
Error: Failed to write file at '{path}/{file}': {errorMessage}

---

## Usage Convention

These codes and messages should be referenced in:

- All .feature files for CLI commands
- CLI stderr output in production
- Unit/integration test assertions
