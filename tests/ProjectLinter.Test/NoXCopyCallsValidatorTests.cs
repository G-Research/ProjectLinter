using FluentAssertions;
using Microsoft.Build.Evaluation;
using Moq;
using ProjectLinter.Validators;
using Xunit;

namespace ProjectLinter.Test
{
    public class NoXCopyCallsValidatorTests
    {
        [Fact]
        public void SdkProjectWithNoXCopyCalls_PassesValidation()
        {
            string projectString = @"<Project Sdk=""Microsoft.NET.Sdk"">
   <PropertyGroup>
     <TargetFramework>netcoreapp2.1</TargetFramework>    
   </PropertyGroup>
   <Target Name=""CustomPostBuild"" AfterTargets=""CoreCompile"">
     <Exec Command=""dir"" />
   </Target>
 </Project>";

            using (var testProject = new TestProject("NewStyleWithoutXCopyCallsProject.csproj", projectString))
            {
                Project project = testProject.Project;
                Mock<ILogger> mockLogger = new Mock<ILogger>();

                var validator = new NoXCopyCallsValidator(mockLogger.Object);
                validator.Validate(project).Should().BeTrue();
                // I don't expect to have any errors either...
                mockLogger.Verify(l => l.LogError(It.IsAny<string>(), It.IsAny<object[]>()), Times.Never);
            }
        }

        [Fact]
        public void SdkProjectWithXCopyCalls_FailsValidation()
        {
            string projectString = @"<Project Sdk=""Microsoft.NET.Sdk"">
   <PropertyGroup>
     <TargetFramework>netcoreapp2.1</TargetFramework>    
   </PropertyGroup>
   <Target Name=""CustomPostBuild"" AfterTargets=""CoreCompile"">
     <Exec Command=""dir"" />
     <Exec Command=""xcopy someStuff elsewhere"" />
     <Exec Command=""xcopy someStuff toanotherplace"" />
   </Target>
 </Project>";

            using (var testProject = new TestProject("NewStyleWithXCopyCallsProject.csproj", projectString))
            {
                Project project = testProject.Project;
                Mock<ILogger> mockLogger = new Mock<ILogger>();

                var validator = new NoXCopyCallsValidator(mockLogger.Object);
                validator.Validate(project).Should().BeFalse();
                // I don't expect to have any errors either...
                mockLogger.Verify(l => l.LogError(It.IsAny<string>(), It.IsAny<object[]>()), Times.Exactly(2));
            }
        }

        [Fact]
        public void OldStyleProjectWithXCopyCalls_PassesValidation()
        {
            string projectString = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Project ToolsVersion=""12.0"" DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
   <PropertyGroup>
     <TargetFrameworkVersion>v4.6.1</TargetFrameworkVersion>
     <OutputPath>.\bin\A-Path-Which-We-Do-Not-Need\</OutputPath>
   </PropertyGroup>
   <Target Name=""CustomPostBuild"" AfterTargets=""CoreCompile"">
     <Exec Command=""dir"" />
     <Exec Command=""xcopy someStuff elsewhere"" />
     <Exec Command=""xcopy someStuff toanotherplace"" />
   </Target>
   <Import Project=""$(MSBuildToolsPath)\Microsoft.CSharp.targets"" />
 </Project>";

            using (var testProject = new TestProject("OldStyleWithXCopyCallsProject.csproj", projectString))
            {
                Project project = testProject.Project;
                Mock<ILogger> mockLogger = new Mock<ILogger>();

                var validator = new NoXCopyCallsValidator(mockLogger.Object);
                validator.Validate(project).Should().BeTrue();
                // I don't expect to have any errors either...
                mockLogger.Verify(l => l.LogError(It.IsAny<string>(), It.IsAny<object[]>()), Times.Never);
            }
        }
    }
}
