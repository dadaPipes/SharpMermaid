# Config Test Coverage Checklist

#### CLI parameters provided

`CLI` values set configuration options and override `JSON` and `defaults`

- If required `CLI` parameters are missing, the system falls back to `JSON` (if `JSON` file exists), otherwise to `defaults`
- If `CLI` input references a file or resource that does not exist, the system stops execution and displays an error message in the console
- If `CLI` parameters are malformed, system stops execution and displays an error message
- Optional: the console displays which configuration source was used for each value

#### JSON config file exist

`JSON` file sets configuration options and overrides `defaults`

- If the `JSON` file is not found, the system stops execution and displays an error message in the console
- If the `JSON` file is malformed, the system stops execution and displays an error message in the console
- If `JSON` has partial config, the system merges with `default` for unspecified values
- Optional: the console displays which configuration source was used for each value

#### Default behavior

- If required keys are missing and have no `defaults`, the system stops execution and displays an error message in the console
- Optional: the console displays which configuration source was used for each value
