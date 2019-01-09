#Build steps for testing.

dotnet build ProjectLinter.sln --configuration Release

# Copy over latest nuget package
copy .\artefacts\* E:\Nuget\ -Force

# delete the copy in the cache.
if (Test-Path "C:\Users\$env:USERNAME\.nuget\packages\projectlinter")
{
  rmdir -Recurse "C:\Users\$env:USERNAME\.nuget\packages\projectlinter"
}
