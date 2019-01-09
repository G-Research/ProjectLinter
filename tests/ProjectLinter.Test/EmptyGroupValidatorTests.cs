using FluentAssertions;
using Microsoft.Build.Evaluation;
using Moq;
using ProjectLinter.Validators;
using Xunit;

namespace ProjectLinter.Test
{
    public class EmptyGroupValidatorTests
    {
        [Fact]
        public void EmptyPropertyGroupFailValidation()
        {
            string invalidProject = @"<Project Sdk=""Microsoft.NET.Sdk"">
   <PropertyGroup>
     <TargetFramework>netcoreapp3.1</TargetFramework>
   </PropertyGroup>
   <PropertyGroup>
   </PropertyGroup>
   <ItemGroup>
     <ProjectReference Include=""..\Source\MyProject.csproj"" />
   </ItemGroup>
 </Project>";

            using (var testProject = new TestProject("EmptyPropertyGroup.Tests.csproj", invalidProject))
            {
                Project project = testProject.Project;
                Mock<ILogger> mockLogger = new Mock<ILogger>();
                EmptyGroupValidator emptyGroupValidator = new EmptyGroupValidator(mockLogger.Object);
                emptyGroupValidator.Validate(project).Should().BeFalse();
                mockLogger.Verify(l => l.LogError(It.IsAny<string>(), It.IsAny<object[]>()), Times.Once());
            }
        }

        [Fact]
        public void EmptyItemGroupFailValidation()
        {
            string invalidProject = @"<Project Sdk=""Microsoft.NET.Sdk"">
   <PropertyGroup>
     <TargetFramework>netcoreapp3.1</TargetFramework>
   </PropertyGroup>
   <ItemGroup>
   </ItemGroup>
   <ItemGroup>
     <ProjectReference Include=""..\Source\MyProject.csproj"" />
   </ItemGroup>
 </Project>";

            using (var testProject = new TestProject("EmptyItemGroup.Tests.csproj", invalidProject))
            {
                Project project = testProject.Project;
                Mock<ILogger> mockLogger = new Mock<ILogger>();
                EmptyGroupValidator emptyGroupValidator = new EmptyGroupValidator(mockLogger.Object);
                emptyGroupValidator.Validate(project).Should().BeFalse();
                mockLogger.Verify(l => l.LogError(It.IsAny<string>(), It.IsAny<object[]>()), Times.Once());
            }

        }

        [Fact]
        public void NoEmptyGroupsPassesValidation()
        {
            string invalidProject = @"<Project Sdk=""Microsoft.NET.Sdk"">
   <PropertyGroup>
     <TargetFramework>netcoreapp3.1</TargetFramework>
   </PropertyGroup>
   <ItemGroup>
     <ProjectReference Include=""..\Source\MyProject.csproj"" />
   </ItemGroup>
 </Project>";

            using (var testProject = new TestProject("NoEmptyGroup.Tests.csproj", invalidProject))
            {
                Project project = testProject.Project;
                Mock<ILogger> mockLogger = new Mock<ILogger>();
                EmptyGroupValidator emptyGroupValidator = new EmptyGroupValidator(mockLogger.Object);
                emptyGroupValidator.Validate(project).Should().BeTrue();
                mockLogger.Verify(l => l.LogError(It.IsAny<string>(), It.IsAny<object[]>()), Times.Never());
            }

        }
    }
}
