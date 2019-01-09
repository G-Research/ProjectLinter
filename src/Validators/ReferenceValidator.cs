using System.IO;
using System.Linq;
using Microsoft.Build.Evaluation;

namespace ProjectLinter.Validators
{
    public class ReferenceValidator : IValidator
    {
        public string Id => "ReferenceValidator";

        private readonly ILogger _logger;

        public ReferenceValidator(ILogger logger)
        {
            _logger = logger;
        }

        public bool Validate(Project project)
        {
            int rootedHintPaths = 0;
            foreach (var reference in project.GetItems("Reference"))
            {
                var hintPath = reference.Metadata.SingleOrDefault(pm => pm.Name == "HintPath");
                if (hintPath != null)
                {
                    if (hintPath.UnevaluatedValue.IsRootedPath())
                    {
                        _logger.LogError($"{reference.ItemType} to '{reference.UnevaluatedInclude}' found with rooted HintPath '{hintPath.UnevaluatedValue}'. These are dangerous and should not be used.");
                        rootedHintPaths++;
                    }
                }
            }

            return rootedHintPaths == 0;
        }
    }
}
