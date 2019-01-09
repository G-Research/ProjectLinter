using FluentAssertions;
using Microsoft.Build.Evaluation;
using Moq;
using ProjectLinter.Validators;
using Xunit;

namespace ProjectLinter.Test
{
    public class DuplicateReferenceValidatorTests
    {
        [Fact]
        public void DuplicateProjectReferenecesFailValidation()
        {
            string invalidProject = @"<Project Sdk=""Microsoft.NET.Sdk"">
   <PropertyGroup>
     <TargetFramework>netcoreapp2.1</TargetFramework>
   </PropertyGroup>
   <ItemGroup Condition=""'$(Configuration)'=='Debug'"">
     <ProjectReference Include=""..\Source\MyProject.csproj"" />
   </ItemGroup>
   <ItemGroup Condition=""'$(Configuration)'=='Debug'"">
     <ProjectReference Include=""..\Source\MyProject.csproj"" />
   </ItemGroup>
 </Project>";

            using (var testProject = new TestProject("DuplicateProjectReferences.Tests.csproj", invalidProject))
            {
                Project project = testProject.Project;
                Mock<ILogger> mockLogger = new Mock<ILogger>();
                DuplicateReferenceValidator referenceValidator = new DuplicateReferenceValidator(mockLogger.Object);
                referenceValidator.Validate(project).Should().BeFalse();
                mockLogger.Verify(l => l.LogError(It.IsAny<string>(), It.IsAny<object[]>()), Times.Once());
            }

        }

        [Fact]
        public void DuplicateRackageReferenecesFailValidation()
        {
            string invalidProject = @"<Project Sdk=""Microsoft.NET.Sdk"">
   <PropertyGroup>
     <TargetFramework>netcoreapp2.1</TargetFramework>
   </PropertyGroup>
   <ItemGroup>
     <PackageReference Include=""A"" />
   </ItemGroup>
   <ItemGroup>
     <PackageReference Include=""A"" />
   </ItemGroup>
 </Project>";

            using (var testProject = new TestProject("DuplicatePackageReferences.Tests.csproj", invalidProject))
            {
                Project project = testProject.Project;
                Mock<ILogger> mockLogger = new Mock<ILogger>();
                DuplicateReferenceValidator referenceValidator = new DuplicateReferenceValidator(mockLogger.Object);
                referenceValidator.Validate(project).Should().BeFalse();
                mockLogger.Verify(l => l.LogError(It.IsAny<string>(), It.IsAny<object[]>()), Times.Once());
            }

        }

        [Fact]
        public void ProjectWithNoDuplicatesPassesValidation()
        {
            string validProject = @"<Project Sdk=""Microsoft.NET.Sdk"">
   <PropertyGroup>
     <TargetFramework>netcoreapp2.1</TargetFramework>
   </PropertyGroup>
   <ItemGroup>
     <ProjectReference Include=""A.csproj"" />
   </ItemGroup>
   <ItemGroup>
     <PackageReference Include=""B"" />
   </ItemGroup>
 </Project>";

            using (var testProject = new TestProject("NoDuplicatePackageReferences.Tests.csproj", validProject))
            {
                Project project = testProject.Project;
                Mock<ILogger> mockLogger = new Mock<ILogger>();
                DuplicateReferenceValidator referenceValidator = new DuplicateReferenceValidator(mockLogger.Object);
                referenceValidator.Validate(project).Should().BeTrue();
                // I don't expect to have any errors either...
                mockLogger.Verify(l => l.LogError(It.IsAny<string>(), It.IsAny<object[]>()), Times.Never());
            }

        }


    }
}
