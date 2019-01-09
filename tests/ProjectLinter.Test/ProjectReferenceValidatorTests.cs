using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Microsoft.Build.Evaluation;
using Moq;
using ProjectLinter.Validators;
using Xunit;

namespace ProjectLinter.Test
{
    public class ProjectReferenceValidatorTests
    {
        [Fact]
        public void ProjectWithDodgyHintPath_FailsValidation()
        {
            string validProject = @"<Project Sdk=""Microsoft.NET.Sdk"">
   <PropertyGroup>
     <TargetFramework>netcoreapp2.1</TargetFramework>
   </PropertyGroup>
   <ItemGroup>
     <ProjectReference Include=""C:\Source\MyProject.csproj"" />
   </ItemGroup>
 </Project>";

            using (var testProject = new TestProject("AbsolutePathProjectReference.Tests.csproj", validProject))
            {
                Project project = testProject.Project;
                Mock<ILogger> mockLogger = new Mock<ILogger>();
                ProjectReferenceValidator referenceValidator = new ProjectReferenceValidator(mockLogger.Object);
                referenceValidator.Validate(project).Should().BeFalse();
                mockLogger.Verify(l => l.LogError(It.IsAny<string>(), It.IsAny<object[]>()), Times.Once());
            }
        }
    }
}
