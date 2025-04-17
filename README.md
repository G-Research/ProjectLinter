# Project Linter

<img src="./Xml.png" width="300px" />

An MSBuild project file linter to validate project files as part of the build process. ProjectLinter helps maintain consistent and correct MSBuild project files across your codebase by enforcing best practices and catching common issues early.

## About the Project

ProjectLinter is a development tool designed to enforce MSBuild project file standards and best practices. It integrates seamlessly into your build process and helps catch common issues before they cause problems in production. The tool is particularly useful for:

- Maintaining consistent project structures across a large codebase
- Preventing common MSBuild configuration mistakes
- Enforcing best practices for project references and dependencies
- Ensuring test projects have the correct testing framework dependencies
- Catching potential build issues early in the development cycle

## Getting Started

1. Add ProjectLinter to your project using NuGet:
   ```xml
   <PackageReference Include="ProjectLinter" Version="0.2.9" PrivateAssets="All" />
   ```

2. For multi-project solutions, add it to your `Directory.Build.targets` file at the root of your repository:
   ```xml
   <Project>
     <ItemGroup>
       <PackageReference Include="ProjectLinter" Version="0.2.9" PrivateAssets="All" />
     </ItemGroup>
   </Project>
   ```

3. Build your project - ProjectLinter will automatically run as part of the build process.

## How does it work?

The ProjectLinter consists of a set of validators which run against the MSBuild project to check for known issues. These validators are automatically wired into the build process using the package .targets file and run before the 'CoreBuild' target.

The tool includes the following validators:

1. **NoOldStyleProjects**: Ensures projects use the modern SDK-style format
2. **NoStupidProperties**: Checks for unnecessary or incorrect property settings
3. **TestProjectDependencyValidator**: Validates test project dependencies
4. **ReferenceValidator**: Checks for proper reference configurations
5. **ProjectReferenceValidator**: Validates project references
6. **NoXCopyCallsValidator**: Prevents use of xcopy commands in favor of MSBuild tasks
7. **EmptyGroupValidator**: Detects empty property and item groups
8. **ConditionValidator**: Validates condition expressions
9. **DuplicateReferenceValidator**: Prevents duplicate project and package references

Warnings and errors are written to the standard build output, and any validation failures will fail the build.

## Usage

### Basic Setup

To add linting to your project, simply add a PackageReference:
```xml
<Project>
  <ItemGroup>
    <PackageReference Include="ProjectLinter" Version="0.2.9" PrivateAssets="All" />
  </ItemGroup>
</Project>
```

### Skip Validators

To skip particular validators for a project, add their IDs (separated by semicolons) to the `ProjectLinterSuppressions` property:

```xml
<Project>
  <PropertyGroup>
    <ProjectLinterSuppressions>NoOldStyleProjects;XCopyValidator</ProjectLinterSuppressions>
  </PropertyGroup>
</Project>
```

### Available Validator IDs

- `NoOldStyleProjects`: Skip SDK-style project validation
- `PropertyValidator`: Skip property validation
- `TestDependencyValidator`: Skip test project dependency checks
- `ReferenceValidator`: Skip reference validation
- `ProjectReferenceValidator`: Skip project reference validation
- `XCopyValidator`: Skip xcopy command validation
- `EmptyGroup`: Skip empty group validation
- `Condition`: Skip condition validation
- `DuplicateReference`: Skip duplicate reference validation

## Contributing

Please see our [contributing guide](CONTRIBUTING.md) for details on how to contribute to the project.

## Security

Please see our [security policy](https://github.com/G-Research/ProjectLinter/blob/main/SECURITY.md) for details on reporting security vulnerabilities.
