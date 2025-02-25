# CLAUDE.md for VanDNationGame

## Build & Run Commands
- Run the game: Open in Godot and press F5 or click "Play" button
- Build project: `dotnet build`
- Export project: Use Godot's export functionality in the editor

## Code Style Guidelines

### Naming Conventions
- Classes: PascalCase (ex: MapModel, TileModel)
- Methods: PascalCase (ex: FindHabitablePlanet)
- Variables: camelCase (ex: localResources, parent)
- Private fields: camelCase (ex: storageBuildings)
- Public fields: PascalCase (ex: Buildings, Tiles)

### Code Organization
- File structure follows logical domain organization (generator, buildings, faction)
- Keep files reasonably sized with single responsibility
- Related functionality grouped in folders (ex: src/buildings/)

### C# Conventions
- Use Properties with getters/setters for public access
- Methods should be focused with clear purpose
- Prefer composition over inheritance
- Use meaningful variable names that indicate purpose
- Standard C# indentation (4 spaces)
- Use parentheses for tuples like (int, int)

### Error Handling
- Return null or boolean when operations can fail
- Use conditional checks for validation

### Comments
- Comment complex algorithms or non-obvious behavior
- XML-style comments for public APIs preferred