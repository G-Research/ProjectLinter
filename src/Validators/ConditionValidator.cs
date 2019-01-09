using Microsoft.Build.Evaluation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLinter.Validators
{
    public class ConditionValidator : IValidator
    {
        private readonly ILogger _logger;

        public string Id => "Condition";

        public ConditionValidator(ILogger logger)
        {
            _logger = logger;
        }

        public bool Validate(Project project)
        {
            bool groupsWithInvalidConditions = false;

            foreach (var propertyGroup in project.Xml.PropertyGroups.Where(g => g.Condition.Contains("$(Platform)")))
            {
                groupsWithInvalidConditions = true;
                _logger.LogError($"Invalid PropertyGroup condition '{propertyGroup.Condition}' found. Conditions must not contain $(Platform)");
            }

            foreach (var itemGroup in project.Xml.ItemGroups.Where(g => g.Condition.Contains("$(Platform)")))
            {
                groupsWithInvalidConditions = true;
                _logger.LogError($"Invalid ItemGroup condition '{itemGroup.Condition}' found. Conditions must not contain $(Platform)");
            }

            return !groupsWithInvalidConditions;
        }
    }
}
