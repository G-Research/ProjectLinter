using System.Linq;
using Microsoft.Build.Evaluation;

namespace ProjectLinter.Validators
{
    public class NoXCopyCallsValidator : IValidator
    {
        private readonly ILogger _logger;

        public NoXCopyCallsValidator(ILogger logger)
        {
            _logger = logger;
        }

        public string Id => "XCopyValidator";

        public bool Validate(Project project)
        {
            if (!project.IsSdkProject())
            {
                _logger.LogDetail("Only validate XCopy commands on SDK style projects");
                return true;
            }

            int xcopyCommands = 0;
            foreach (var target in project.Targets)
            {
                foreach (var task in target.Value.Tasks.Where(t => t.Name == "Exec"))
                {
                    if (task.Parameters.TryGetValue("Command", out string command))
                    {
                        if (command.ToLower().Contains("xcopy"))
                        {
                            xcopyCommands++;
                            _logger.LogError("We do not want or allow the use of 'xcopy' commands in exec calls. Remove or use MSBuild Copy task.");
                        }
                    }
                }
            }

            return xcopyCommands == 0;
        }
    }
}
