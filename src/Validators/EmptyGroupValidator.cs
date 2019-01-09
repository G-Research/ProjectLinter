using Microsoft.Build.Evaluation;
using System.Linq;

namespace ProjectLinter.Validators
{
    public class EmptyGroupValidator : IValidator
    {
        private readonly ILogger _logger;

        public string Id => "EmptyGroup";

        public EmptyGroupValidator(ILogger logger)
        {
            _logger = logger;
        }

        public bool Validate(Project project)
        {
            bool emptyGroups = false;

            var emptyPropertyGroupCount = project.Xml.PropertyGroups.Count(g => g.Count == 0);
            if (emptyPropertyGroupCount > 0)
            {
                emptyGroups = true;
                _logger.LogError($"Found {emptyPropertyGroupCount} empty property groups");
            }

            var emptyItemGroupCount = project.Xml.ItemGroups.Count(g => g.Count == 0);
            if (emptyItemGroupCount > 0)
            {
                emptyGroups = true;
                _logger.LogError($"Found {emptyItemGroupCount} empty property groups");
            }

            return !emptyGroups;
        }
    }
}
