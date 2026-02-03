# CLAUDE.md - Project Guidelines for Claude Code

## Project Overview

This is a Blazor Server application for learning Spanish vocabulary. It uses data from the doozan/spanish_data GitHub repository.

## Key Commands

```bash
# Build the project
dotnet build

# Run the application (starts on http://localhost:5062)
dotnet run

# Update spanish_data (if needed)
cd spanish_data && git pull
```

## Architecture

- **Blazor Server** with interactive server-side rendering
- **No database** - progress stored in browser localStorage via JS interop
- **Data loading** - WordService loads from spanish_data files at startup, falls back to embedded data

## Important Files

- `Services/WordService.cs` - Main service providing word data
- `Services/DataLoader.cs` - Parses frequency.csv and es-en.data files
- `Services/ProgressService.cs` - Handles localStorage persistence
- `spanish_data/` - Cloned repository with frequency and translation data

## Documentation Requirements

### README.md Must Be Updated When:

1. **New features are added** - Add to Features section
2. **New limitations are discovered or addressed** - Update Limitations section
3. **Data source changes** - Update Data Source section
4. **Project structure changes** - Update Project Structure section
5. **New dependencies or requirements** - Update Requirements section
6. **Running instructions change** - Update Running the App section

### README.md Sections to Maintain:

- **Features**: Document all user-facing functionality
- **Data Source**: Credit doozan/spanish_data and its upstream sources
- **Limitations**: Be honest about what the app cannot do
- **Requirements**: Keep .NET version and dependencies current
- **Project Structure**: Reflect actual file organization

## Code Style

- Use file-scoped namespaces
- Prefer `required` properties over constructor injection for models
- Use `@rendermode InteractiveServer` for interactive Blazor components
- Keep Razor components focused - one primary responsibility per page

## Data Files

The app expects these files in `spanish_data/`:
- `frequency.csv` - CSV with columns: count, spanish, pos, flags, usage
- `es-en.data` - Custom format with word blocks separated by `_____`

If files are missing, the app uses 200 embedded fallback words defined in WordService.cs.
