using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Build.Evaluation;
using NuGet.Versioning;

namespace ProjectLinter.Validators
{
    public class TestProjectDependencyValidator : IValidator
    {
        public string Id => "TestDependencyValidator";

        private Regex TestAssemblyRegex => new Regex("(?i)test(s?)$");
        private readonly ILogger _logger;

        public TestProjectDependencyValidator(ILogger logger)
        {
            // TODO: Enable minimum versions to be passed in as item group...
            _logger = logger;
        }

        public bool Validate(Project project)
        {
            // This property is only being set when this test is run through dotnet test/vstest (I assume that this is due to the way in which msbuild is bootstrapped)
            string projectName = project.GetPropertyValue("AssemblyName");

            if (String.IsNullOrEmpty(projectName))
            {
                _logger.LogWarning($"Failed to determine project name for [{project.FullPath}].");
                return false;
            }

            if (!TestAssemblyRegex.IsMatch(projectName))
            {
                _logger.LogDetail($"Validation of project [{projectName}] is skipped this is not a test project.");
                return true;
            }

            bool validReferences = true;

            _logger.LogDetail($"Validating package references for test project [{project.FullPath}]");

            var packageReferences = project.GetItems("PackageReference");

            if (!(TryGetPackage(packageReferences, "Microsoft.NET.Test.Sdk", logErrorIfMissing: true)))
            {
                validReferences = false;
            }

            if (TryGetPackage(packageReferences, "xunit"))
            {
                if (!(TryGetPackage(packageReferences, "xunit.runner.visualstudio", true)))
                {
                    validReferences = false;
                }
            }
            else if (TryGetPackage(packageReferences, "NUnit") ||
                     TryGetPackage(packageReferences, "NUnitLite"))
            {
                if (!TryGetPackage(packageReferences, "NUnit3TestAdapter", logErrorIfMissing: true))
                {
                    validReferences = false;
                }
            }
            else
            {
                _logger.LogError("Failed to find package reference to valid testing framework (xunit, NUnit, or NUnitLite) in test project.");
                validReferences = false;
            }

            return validReferences;
        }

        public bool TryGetPackage(IEnumerable<ProjectItem> packageReferences, string packageName, bool logErrorIfMissing = false)
        {
            var projectItem = packageReferences.SingleOrDefault(pr => pr.UnevaluatedInclude.ToLower() == packageName.ToLower());

            if (projectItem == null)
            {
                if (logErrorIfMissing)
                {
                    _logger.LogError($"Missing required package reference to '{packageName}' in test project");
                }

                return false;
            }

            return true;
        }
    }
}
