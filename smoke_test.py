import os
import sys
import subprocess

os.chdir(os.path.dirname(__file__))
print(f"Current working directory is {os.getcwd()}")

subprocess.run(["dotnet", "build", "./tests/TestMultiTargetedProjectLinterPackage/TestMultiTargetedProjectLinterPackage.proj", "--configuration=Release"], check=True)
subprocess.run(["dotnet", "build", "./tests/TestProjectLinterPackage/TestProjectLinterPackage.proj", "--configuration=Release"], check=True)

if sys.platform == 'win32':
   # Works because I have msbuild defined.
   subprocess.run(["msbuild", "./tests/TestMultiTargetedProjectLinterPackage/TestMultiTargetedProjectLinterPackage.proj", "/p:Configuration=Release"], check=True)
