using FluentAssertions;
using Microsoft.Build.Evaluation;
using Moq;
using ProjectLinter.Validators;
using Xunit;

namespace ProjectLinter.Test
{
    public class NoOldStyleProjectsValidatorTests
    {
        [Fact]
        public void SdkProject_PassesValidation()
        {
            string projectString = @"<Project Sdk=""Microsoft.NET.Sdk"">
   <PropertyGroup>
     <TargetFramework>netcoreapp2.1</TargetFramework>    
   </PropertyGroup>
 </Project>";

            using (var testProject = new TestProject("OldStyleWithXCopyCallsProject.csproj", projectString))
            {
                Project project = testProject.Project;
                Mock<ILogger> mockLogger = new Mock<ILogger>();

                var validator = new NoOldStyleProjectsValidator(mockLogger.Object);
                validator.Validate(project).Should().BeTrue();
                // I don't expect to have any errors either...
                mockLogger.Verify(l => l.LogError(It.IsAny<string>(), It.IsAny<object[]>()), Times.Never);
            }
        }

        [Fact]
        public void OldStyleProject_FailsValidation()
        {
            string projectString = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Project ToolsVersion=""12.0"" DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
   <PropertyGroup>
     <TargetFrameworkVersion>v4.6.1</TargetFrameworkVersion>
   </PropertyGroup>
   <Import Project=""$(MSBuildToolsPath)\Microsoft.CSharp.targets"" />
 </Project>";

            using (var testProject = new TestProject("OldStyleWithXCopyCallsProject.csproj", projectString))
            {
                Project project = testProject.Project;
                Mock<ILogger> mockLogger = new Mock<ILogger>();

                var validator = new NoOldStyleProjectsValidator(mockLogger.Object);
                validator.Validate(project).Should().BeFalse();
                mockLogger.Verify(l => l.LogError(It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
            }
        }
    }
}
