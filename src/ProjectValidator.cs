using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using ProjectLinter.Validators;

namespace ProjectLinter
{
    public interface IValidator
    {
        string Id { get; }
        bool Validate(Project project);
    }

    public class ProjectValidator : Task
    {
        [Required]
        public string ProjectFile { get; set; }

        public ITaskItem[] Suppressions { get; set; }

        /// <summary>
        /// Indicates whether to have validation warnings as errors
        /// </summary>
        public bool WarningsAsErrors { get; set; }

        protected Project GetProject()
        {
            Log.LogMessage(MessageImportance.Normal, $"Loading project file [{ProjectFile}] for validation.");
            var project = ProjectCollection.GlobalProjectCollection.LoadProject(ProjectFile);
            Log.LogMessage(MessageImportance.Low, "Successfully loaded project file.");
            return project;
        }

        private IEnumerable<IValidator> GetValidators()
        {
            ILogger logger = new Logger(Log, WarningsAsErrors);

            return new IValidator[]
            {
                new NoOldStyleProjectsValidator(logger),
                new NoStupidPropertiesValidator(logger),
                new TestProjectDependencyValidator(logger),
                new ReferenceValidator(logger),
                new ProjectReferenceValidator(logger),
                new NoXCopyCallsValidator(logger),
                new EmptyGroupValidator(logger),
                new ConditionValidator(logger),
                new DuplicateReferenceValidator(logger)
            };
        }

        public override bool Execute()
        {
            var project = GetProject();
            var stopWatch = Stopwatch.StartNew();

            HashSet<string> suppressions;
            if (Suppressions == null || Suppressions.Length == 0)
            {
                suppressions = new HashSet<string>();
            }
            else
            {
                suppressions = new HashSet<string>(Suppressions.Select(s => s.ItemSpec));
            }

            Log.LogMessage(MessageImportance.Low, $"Starting validation of project file '{project.FullPath}'.");
            bool valid = true;
            foreach (IValidator validator in GetValidators())
            {
                if (suppressions.Contains(validator.Id))
                {
                    Log.LogWarning($"Skipping {validator.Id} validation.");
                    continue;
                }

                if (!validator.Validate(project))
                {
                    valid = false;
                }
            }

            stopWatch.Stop();

            bool isValid = valid && !Log.HasLoggedErrors;
            if (isValid)
            {
                Log.LogMessage(MessageImportance.Normal, $"Validation of project file '{project.FullPath}' has succeeded. Time taken: [{stopWatch.Elapsed}]");
            }
            else
            {
                Log.LogError($"Validation of project '{project.FullPath}' has failed. See logs for details.");
            }

            return isValid;
        }
    }
}
