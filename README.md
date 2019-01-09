# Project Linter

<img src="./Xml.png" width="300px" />

An MSBuild project file linter to validate project file as part of build process.

## How does it work?

The ProjectLinter consists of a set of validators which run against the MSBuild project to check for known issues.

These include:

1. Rooted paths
2. Unnecessary|incorrect properties
3. Required dependencies

These validators are automatically wired into the build process using the package .targets file and run before the 'CoreBuild' target.

Warnings and errors are written to the standard build output and any validation failures will fail the build.

## Usage

To add linting to your project simply add a PacakgeReference.
The easiest way to do this for a large number of projects is to include a Directory.Build.targets file at the root of the repo containing:

```xml
<Project>
  <ItemGroup>
    <PackageReference Include="ProjectLinter" Version="0.2.9" PrivateAssets="All" />
  </ItemGroup>
</Project>
```

To automatically import the nuget pacakge into all projects.

### Skip validators

To skip particular validators for a project add their Ids (separated by ;) to the `ProjectLinterSuppressions` property

```xml
<Project>
  <PropertyGroup>
    <ProjectLinterSuppressions>NoOldStyleProjects</ProjectLinterSuppressions>
  </PropertyGroup>
</Project>
```
