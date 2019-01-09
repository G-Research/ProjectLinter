using FluentAssertions;
using Microsoft.Build.Evaluation;
using Moq;
using ProjectLinter.Validators;
using System;
using Xunit;

namespace ProjectLinter.Test
{
    public class TestProjectDependencyValidatorTests
    {
        [Fact]
        public void TestProjectWithOnlyNUnitReference_FailsValidation()
        {
            string validProject = @"<Project Sdk=""Microsoft.NET.Sdk"">
   <PropertyGroup>
     <TargetFramework>netcoreapp2.1</TargetFramework>
   </PropertyGroup>
   <ItemGroup>
     <PackageReference Include=""NUnit"" Version=""3.11.0"" />
   </ItemGroup>
 </Project>";

            using (var testProject = new TestProject("InvalidProject.Test.csproj", validProject))
            {
                Project project = testProject.Project;

                Mock<ILogger> mockLogger = new Mock<ILogger>();

                TestProjectDependencyValidator testProjectDependencyValidator = new TestProjectDependencyValidator(mockLogger.Object);
                testProjectDependencyValidator.Validate(project).Should().BeFalse();
                mockLogger.Verify(l => l.LogError(It.IsAny<string>(), It.IsAny<object[]>()), Times.AtLeast(2));
            }
        }

        [Fact]
        public void ProjectWithValidNUnitReferences_PassesValidation()
        {
            string validProject = @"<Project Sdk=""Microsoft.NET.Sdk"">
   <PropertyGroup>
     <TargetFramework>netcoreapp2.1</TargetFramework>
   </PropertyGroup>
   <ItemGroup>
     <PackageReference Include=""Microsoft.NET.Test.Sdk"" Version=""15.8.0"" />
     <PackageReference Include=""NUnit"" Version=""3.11.0"" />
     <PackageReference Include=""NUnit3TestAdapter"" Version=""3.10.1"" />
   </ItemGroup>
 </Project>";

            using (var testProject = new TestProject($"{Guid.NewGuid()}.Tests.csproj", validProject))
            {
                Project project = testProject.Project;

                Mock<ILogger> mockLogger = new Mock<ILogger>();

                TestProjectDependencyValidator testProjectDependencyValidator = new TestProjectDependencyValidator(mockLogger.Object);
                testProjectDependencyValidator.Validate(project).Should().BeTrue();
                // I don't expect to have any errors either...
                mockLogger.Verify(l => l.LogError(It.IsAny<string>(), It.IsAny<object[]>()), Times.Never);
            }
        }

        [Fact]
        public void ProjectWithMissingNUnitTestAdapter_FailsValidation()
        {
            string validProject = @"<Project Sdk=""Microsoft.NET.Sdk"">
   <PropertyGroup>
     <TargetFramework>netcoreapp2.1</TargetFramework>
   </PropertyGroup>
   <ItemGroup>
     <PackageReference Include=""Microsoft.NET.Test.Sdk"" Version=""15.8.0"" />
     <PackageReference Include=""NUnit"" Version=""3.11.0"" />
   </ItemGroup>
 </Project>";

            using (var testProject = new TestProject($"{Guid.NewGuid()}.Tests.csproj", validProject))
            {
                Project project = testProject.Project;
                Mock<ILogger> mockLogger = new Mock<ILogger>();
                TestProjectDependencyValidator testProjectDependencyValidator = new TestProjectDependencyValidator(mockLogger.Object);
                testProjectDependencyValidator.Validate(project).Should().BeFalse();
                mockLogger.Verify(l => l.LogError(It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
            }
        }

        [Fact]
        public void ProjectWithMissingXUnitReference_FailsValidation()
        {
            string validProject = @"<Project Sdk=""Microsoft.NET.Sdk"">
   <PropertyGroup>
     <TargetFramework>netcoreapp2.1</TargetFramework>
   </PropertyGroup>
   <ItemGroup>
     <PackageReference Include=""Microsoft.NET.Test.Sdk"" Version=""15.8.0"" />
     <PackageReference Include=""xunit"" Version=""2.2.0"" />
   </ItemGroup>
 </Project>";

            using (var testProject = new TestProject($"{Guid.NewGuid()}.Tests.csproj", validProject))
            {
                Project project = testProject.Project;
                Mock<ILogger> mockLogger = new Mock<ILogger>();
                TestProjectDependencyValidator testProjectDependencyValidator = new TestProjectDependencyValidator(mockLogger.Object);
                testProjectDependencyValidator.Validate(project).Should().BeFalse();
                mockLogger.Verify(l => l.LogError(It.IsAny<string>(), It.IsAny<object[]>()), Times.Exactly(1));
            }
        }
    }
}
