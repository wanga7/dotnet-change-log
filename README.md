# dotnet-change-log

A .NET Tool for generating changelog based on .NET project dependencies, particularly useful for mono repos.

This project is **WIP**.

## Command Line Options
```
  -r, --repo-dir           Required. Repo directory (default to current working directory)

  -p, --project            Required. Project for Changelog (e.g. App.csproj)

  -f, --from               Required. Starting git tag for Changelog

  -t, --to                 Required. Ending git tag for Changelog

  -c, --change-log-mode    Type of changelog to generate, Direct/Continuous (default to Direct)

  -x, --tag-regex          Regex pattern used to filter tags for target project (only used in Continuous changelog mode)

  -e, --exclude            Excluded projects pattern (dotnet Regular Expression that will be matched against each Project's Full Path, e.g. ".Tests.")       

  -o, --output-dir         Changelog output directory relative to repo root (default to repo root directory)

  -m, --output-format      Changelog output format (Console/Text/Json/Markdown, default to Console)

  --help                   Display this help screen.

  --version                Display version information.
```