using Microsoft.Build.Evaluation;

namespace ProjectLinter.Validators
{
    public class NoOldStyleProjectsValidator : IValidator
    {
        private readonly ILogger _logger;

        public string Id => "NoOldStyleProjects";

        public NoOldStyleProjectsValidator(ILogger logger)
        {
            _logger = logger;
        }

        public bool Validate(Project project)
        {
            bool valid = project.IsSdkProject();
            if (!valid)
            {
                _logger.LogError("No old style projects permitted");
            }

            return valid;
        }
    }
}
