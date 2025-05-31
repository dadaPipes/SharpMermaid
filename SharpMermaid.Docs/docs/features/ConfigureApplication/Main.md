# Configure Application

## Description

As a developer,
I want to configure the application via a combination of CLI parameters, JSON files, and default values,
So that I can ensure flexibility and robust fallback behavior in different environments

## Rules

- CLI parameters always override JSON and defaults
- JSON configuration file overrides default values
- Defaults are used only when a value is not provided in either CLI or JSON
- If a required CLI parameter is missing (and not in JSON), the system must stop execution and display an error message
- If CLI input references a non-existent file or resource, the system must stop execution and display an error message
- If CLI parameters are malformed, the system must stop execution and display an error message
- If the JSON file does not exist or is malformed, the system must stop execution and display an error message
- If JSON config is partial, it must merge with defaults for missing values
- Final Options object must reflect the merged configuration (CLI > JSON > Defaults)

## Scenarios

**Given** no CLI parameter `SlnPath` and no value in JSON
**When** the CLI is run
**Then** the system stops execution
**And** the console displays an error message indicating the missing required parameter

Given the CLI parameter `--configPath=nonexistent.json`
When the CLI is run 
Then the system stops execution
And the console displays an error message indicating the file was not found

This is an example command where "generate-diagram" is the main(?) command, "--solution" is the path to the solution, relative from where directory the command is executed from, and "--include-urls false" is a flag:

sharpmermaid generate-diagram --solution ./TestSolution.sln --include-urls false