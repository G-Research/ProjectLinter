using System.IO;
using Microsoft.Build.Evaluation;

namespace ProjectLinter.Validators
{
    public class ProjectReferenceValidator : IValidator
    {
        public string Id => "ProjectReferenceValidator";

        private readonly ILogger _logger;

        public ProjectReferenceValidator(ILogger logger)
        {
            _logger = logger;
        }

        public bool Validate(Project project)
        {
            var items = project.GetItems("ProjectReference");

            int rootedPaths = 0;
            foreach (var reference in items)
            {
                if (reference.UnevaluatedInclude.IsRootedPath())
                {
                    _logger.LogError($"{reference.ItemType} to '{reference.UnevaluatedInclude}' found with rooted path. These are considered dangerous and should not be used.");
                    rootedPaths++;
                }
            }

            return rootedPaths == 0;
        }
    }
}
