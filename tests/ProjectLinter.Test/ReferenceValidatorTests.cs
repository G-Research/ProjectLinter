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
    public class ReferenceValidatorTests
    {
        [Fact]
        public void ProjectWithDodgyHintPath_FailsValidation()
        {
            string validProject = @"<Project Sdk=""Microsoft.NET.Sdk"">
   <PropertyGroup>
     <TargetFramework>netcoreapp2.1</TargetFramework>
   </PropertyGroup>
   <ItemGroup>
     <!-- Found this reference recently -->
     <Reference Include=""System.Net.Http, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"">
      <HintPath>C:\Windows\Microsoft.NET\assembly\GAC_MSIL\System.Net.Http\v4.0_4.0.0.0__b03f5f7f11d50a3a\System.Net.Http.dll</HintPath>
    </Reference>
   </ItemGroup>
 </Project>";

            using (var testProject = new TestProject("DodgyReference.Tests.csproj", validProject))
            {
                Project project = testProject.Project;
                Mock<ILogger> mockLogger = new Mock<ILogger>();
                ReferenceValidator referenceValidator = new ReferenceValidator(mockLogger.Object);
                referenceValidator.Validate(project).Should().BeFalse();
                // I don't expect to have any errors either...
                mockLogger.Verify(l => l.LogError(It.IsAny<string>(), It.IsAny<object[]>()), Times.Once());
            }
        }


    }
}
