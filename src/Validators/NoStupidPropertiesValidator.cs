using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Build.Evaluation;

namespace ProjectLinter.Validators
{
    public class NoStupidPropertiesValidator : IValidator
    {
        public string Id => "PropertyValidator";

        public enum AlertType
        {
            Warning,
            Error
        }

        [Flags]
        public enum ProjectType
        {
            OldStyle = 1,
            NewStyle = 2,
            All = 3
        }

        private class EvaluationSettings
        {
            public AlertType AlertType { get; set; }
            public ProjectType ProjectType { get; set; }
            public string ValidPropertyValue { get; set; }
            public string Description { get; set; }
        }

        private readonly ILogger _logger;

        public NoStupidPropertiesValidator(ILogger logger)
        {
            _logger = logger;
        }

        private static readonly Dictionary<string, EvaluationSettings> PropertyEvaluationSettings = new Dictionary<string, EvaluationSettings>
            {
                {"BaseAddress", new EvaluationSettings{AlertType = AlertType.Warning, ProjectType = ProjectType.NewStyle, ValidPropertyValue = null, Description = "SDK projects should in general not have BaseAddress set."}},
                {"OutputPath", new EvaluationSettings{AlertType = AlertType.Warning, ProjectType = ProjectType.NewStyle, ValidPropertyValue = null, Description = "SDK projects do not need should not have OutputPath set."}}
        };


        public bool Validate(Project project)
        {
            _logger.LogDetail($"Validating project property values in {project.FullPath}");

            int invalidProperties = 0;
            foreach (var property in project.Properties.Where(p => !p.IsGlobalProperty && !p.IsEnvironmentProperty && !p.IsImported && !p.IsReservedProperty))
            {
                if (PropertyEvaluationSettings.TryGetValue(property.Name, out EvaluationSettings evaluationSettings))
                {
                    if ((project.IsSdkProject() && evaluationSettings.ProjectType.HasFlag(ProjectType.NewStyle))
                        || (evaluationSettings.ProjectType.HasFlag(ProjectType.OldStyle)))
                    {
                        if (evaluationSettings.ValidPropertyValue == null || evaluationSettings.ValidPropertyValue != property.UnevaluatedValue)
                        {
                            if (evaluationSettings.AlertType == AlertType.Warning)
                            {
                                _logger.LogWarning($"{evaluationSettings.Description}: Value: {property.UnevaluatedValue}");
                            }
                            else
                            {
                                invalidProperties++;
                                _logger.LogError($"{evaluationSettings.Description}: Value: {property.UnevaluatedValue}");
                            }
                        }
                    }
                }
            }

            return invalidProperties == 0;
        }
    }
}
