import os
import sys
import subprocess

os.chdir(os.path.dirname(__file__))
print(f"Current working directory is {os.getcwd()}")
subprocess.run(["dotnet", "test", "./ProjectLinter.sln", "--no-build", "--configuration=Release"], check=True)

if sys.platform == 'win32':
    subprocess.run(["dotnet", "vstest-runner", "--test-assemblies", "./tests/ProjectLinter.Test/bin/Release/net6.0/ProjectLinter.Test.dll", "--docker-image", "mcr.microsoft.com/dotnet/sdk:6.0.408-focal-amd64"], check=True)
