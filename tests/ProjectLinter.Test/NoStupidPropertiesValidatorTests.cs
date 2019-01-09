using FluentAssertions;
using Microsoft.Build.Evaluation;
using Moq;
using ProjectLinter.Validators;
using Xunit;

namespace ProjectLinter.Test
{
    public class NoStupidPropertiesValidatorTests
    {
        [Fact]
        public void SdkProjectWithSuperfluousProperties_PassesValidation()
        {
            string validProject = @"<Project Sdk=""Microsoft.NET.Sdk"">
   <PropertyGroup>
     <TargetFramework>netcoreapp2.1</TargetFramework>
     <BaseAddress>8756</BaseAddress>
     <OutputPath>.\bin\A-Path-Which-We-Do-Not-Need\</OutputPath>
   </PropertyGroup>
   <ItemGroup>
     <PackageReference Include=""Microsoft.NET.Test.Sdk"" Version=""15.8.0"" />
     <PackageReference Include=""NUnit"" Version=""3.11.0"" />
     <PackageReference Include=""NUnit3TestAdapter"" Version=""3.10.1"" />
   </ItemGroup>
 </Project>";

            using (var testProject = new TestProject("NewStyleValidPropertiesProject.Tests.csproj", validProject))
            {
                Project project = testProject.Project;

                Mock<ILogger> mockLogger = new Mock<ILogger>();

                var validator = new NoStupidPropertiesValidator(mockLogger.Object);
                validator.Validate(project).Should().BeTrue();
                // I don't expect to have any errors either...
                mockLogger.Verify(l => l.LogError(It.IsAny<string>(), It.IsAny<object[]>()), Times.Never);
                mockLogger.Verify(l => l.LogWarning(It.Is<string>(message => message.Contains("OutputPath")), It.IsAny<object[]>()), Times.Never);
                mockLogger.Verify(l => l.LogWarning(It.Is<string>(message => message.Contains("BaseAddress")), It.IsAny<object[]>()), Times.Once);
            }
        }

        [Fact]
        public void OldStyleProjectWithSuperflousProperties_PassesValidation_WithNoWarnings()
        {
            string validProject = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Project ToolsVersion=""12.0"" DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
   <PropertyGroup>
     <TargetFrameworkVersion>v4.6.1</TargetFrameworkVersion>
     <OutputPath>.\bin\A-Path-Which-We-Do-Not-Need\</OutputPath>
   </PropertyGroup>
   <ItemGroup>
     <PackageReference Include=""Microsoft.NET.Test.Sdk"" Version=""15.8.0"" />
     <PackageReference Include=""NUnit"" Version=""3.11.0"" />
     <PackageReference Include=""NUnit3TestAdapter"" Version=""3.10.1"" />
   </ItemGroup>
   <Import Project=""$(MSBuildToolsPath)\Microsoft.CSharp.targets"" />
 </Project>";

            using (var testProject = new TestProject("OldStyleValidPropertiesProject.Tests.csproj", validProject))
            {
                Project project = testProject.Project;

                Mock<ILogger> mockLogger = new Mock<ILogger>();

                var validator = new NoStupidPropertiesValidator(mockLogger.Object);
                validator.Validate(project).Should().BeTrue();
                // I don't expect to have any errors either...
                mockLogger.Verify(l => l.LogError(It.IsAny<string>(), It.IsAny<object[]>()), Times.Never);
                mockLogger.Verify(l => l.LogWarning(It.IsAny<string>(), It.IsAny<object[]>()), Times.Never);
            }
        }


    }
}
