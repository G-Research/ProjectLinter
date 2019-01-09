using Microsoft.Build.Evaluation;
using System.Linq;

namespace ProjectLinter.Validators
{
    public class DuplicateReferenceValidator : IValidator
    {
        private readonly ILogger _logger;

        public string Id => "DuplicateReference";

        public DuplicateReferenceValidator(ILogger logger)
        {
            _logger = logger;
        }

        public bool Validate(Project project)
        {
            var itemGroupsToValidate = new[] { "ProjectReference", "PackageReference" };

            bool duplicates = false;
            foreach (var itemGroup in itemGroupsToValidate)
            {
                var items = project.GetItems(itemGroup);

                foreach (var duplicateItemGroup in items.GroupBy(r => r.EvaluatedInclude).Where(g => g.Count() > 1))
                {
                    duplicates = true;
                    _logger.LogError($"Found duplicate {itemGroup} entries for {duplicateItemGroup.First().EvaluatedInclude}");
                }
            }

            return !duplicates;
        }
    }
}
