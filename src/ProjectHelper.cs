using Microsoft.Build.Evaluation;
using System.Linq;
using System.Text.RegularExpressions;

namespace ProjectLinter
{
    public static class ProjectHelper
    {
        public static bool IsSdkProject(this Project project)
        {
            return !string.IsNullOrEmpty(project.Xml.Sdk);
        }

        public static readonly char[] DirectorySeparatorChars = { '/', '\\' };

        public static bool IsRootedPath(this string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }

            if (DirectorySeparatorChars.Contains(path[0]))
            {
                return true;
            }

            if (path[0] == '.')
            {
                return false;
            }

            if (Regex.IsMatch(path, @"\w:[\\|/]"))
            {
                return true;
            }

            return false;
        }
    }
}