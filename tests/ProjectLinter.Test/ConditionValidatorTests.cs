using FluentAssertions;
using Microsoft.Build.Evaluation;
using Moq;
using ProjectLinter.Validators;
using Xunit;

namespace ProjectLinter.Test
{
    public class ConditionValidatorTests
    {
        [Fact]
        public void DodgyPropertyGroupConditionsFailsValidation()
        {
            string invalidProject = @"<Project Sdk=""Microsoft.NET.Sdk"">
   <PropertyGroup>
     <TargetFramework>netcoreapp2.1</TargetFramework>
   </PropertyGroup>
   <ItemGroup Condition=""'$(Configuration)|$(Platform)'=='Debug|AnyCPU'"">
     <ProjectReference Include=""..\Source\MyProject.csproj"" />
   </ItemGroup>
   <ItemGroup Condition=""'$(Platform)'=='AnyCPU'"">
     <ProjectReference Include=""..\Source\MyProject.csproj"" />
   </ItemGroup>
 </Project>";

            using (var testProject = new TestProject("DodgyPropertyGroupCondition.Tests.csproj", invalidProject))
            {
                Project project = testProject.Project;
                Mock<ILogger> mockLogger = new Mock<ILogger>();
                ConditionValidator conditionValidator = new ConditionValidator(mockLogger.Object);
                conditionValidator.Validate(project).Should().BeFalse();
                mockLogger.Verify(l => l.LogError(It.IsAny<string>(), It.IsAny<object[]>()), Times.Exactly(2));
            }
        }

        [Fact]
        public void DodgyItemGroupConditionFailsValidation()
        {
            string invalidProject = @"<Project Sdk=""Microsoft.NET.Sdk"">
   <PropertyGroup>
     <TargetFramework>netcoreapp2.1</TargetFramework>
   </PropertyGroup>
   <PropertyGroup Condition=""'$(Configuration)|$(Platform)'=='Debug|AnyCPU'"">
     <DoSomethingForDebug>true</DoSomethingForDebug>
   </PropertyGroup>
   <PropertyGroup Condition=""'$(Configuration)|$(Platform)'=='Release|AnyCPU'"">
     <DoSomethingForRelease>true</DoSomethingForRelease>
   </PropertyGroup>
   <ItemGroup>
     <PackageReference Include=""A"" />
   </ItemGroup>
 </Project>";

            using (var testProject = new TestProject("DodgyItemGroupCondition.Tests.csproj", invalidProject))
            {
                Project project = testProject.Project;
                Mock<ILogger> mockLogger = new Mock<ILogger>();
                ConditionValidator conditionValidator = new ConditionValidator(mockLogger.Object);
                conditionValidator.Validate(project).Should().BeFalse();
                mockLogger.Verify(l => l.LogError(It.IsAny<string>(), It.IsAny<object[]>()), Times.Exactly(2));
            }
        }

        [Fact]
        public void ProjectWithNoDodgyConditionsPassesValidation()
        {
            string validProject = @"<Project Sdk=""Microsoft.NET.Sdk"">
   <PropertyGroup>
     <TargetFramework>netcoreapp2.1</TargetFramework>
   </PropertyGroup>
   <PropertyGroup Condition=""'$(Configuration)'=='Debug'"">
     <DoSomethingForDebug>true</DoSomethingForDebug>
   </PropertyGroup>
   <PropertyGroup Condition=""'$(Configuration)'=='Release'"">
     <DoSomethingForRelease>true</DoSomethingForRelease>
   </PropertyGroup>
   <ItemGroup Condition=""'$(SpecialCondition)'=='true'"">
     <PackageReference Include=""A"" />
   </ItemGroup>
 </Project>";

            using (var testProject = new TestProject("NoDodgyConditions.Tests.csproj", validProject))
            {
                Project project = testProject.Project;
                Mock<ILogger> mockLogger = new Mock<ILogger>();
                ConditionValidator conditionValidator = new ConditionValidator(mockLogger.Object);
                conditionValidator.Validate(project).Should().BeTrue();
                // I don't expect to have any errors either...
                mockLogger.Verify(l => l.LogError(It.IsAny<string>(), It.IsAny<object[]>()), Times.Never());
            }
        }
    }
}
