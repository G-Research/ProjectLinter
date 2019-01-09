using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Build.Definition;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Evaluation.Context;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using NuGet.Versioning;

namespace ProjectLinter.Test
{
    public static class ProjectHelper
    {
        public static Dictionary<string, string> GetGlobalProperties(string sdkDirectory)
        {
            var globalProperties = new Dictionary<string, string>()
            {
                {"RoslynTargetsPath", Path.Combine(sdkDirectory, "Roslyn")},
                {"MSBuildSDKsPath", Path.Combine(sdkDirectory, "Sdks")},
                {"MSBuildExtensionsPath", sdkDirectory},
                {"Configuration", "Debug"}
            };

            Environment.SetEnvironmentVariable("MSBuildSDKsPath", Path.Combine(sdkDirectory, "Sdks"));
            Environment.SetEnvironmentVariable("MSBuildExtensionsPath", sdkDirectory);
            Environment.SetEnvironmentVariable("MSBUILD_NUGET_PATH", sdkDirectory);
            Environment.SetEnvironmentVariable("MSBuildToolsPath", sdkDirectory);
            return globalProperties;
        }

        private static string GetSdkPath()
        {

            string dotnetPath = "dotnet";
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                dotnetPath = "C:\\Program Files\\dotnet\\dotnet.exe";
            }

            using var process = Process.Start(new ProcessStartInfo(dotnetPath, "--info")
            {
                RedirectStandardOutput = true,
                RedirectStandardError = false,
                CreateNoWindow = true,
                UseShellExecute = false
            });

            while (!process.StandardOutput.EndOfStream)
            {
                var line = process.StandardOutput.ReadLine();
                if (line.Contains("Base Path"))
                {
                    var path = line.Replace("Base Path: ", "").Trim();
                    return path;
                }
            }

            throw new ApplicationException("Unable to find SDK path for project loading.");
        }

        public static readonly Lazy<Dictionary<string, string>> GlobalProperties = new Lazy<Dictionary<string, string>>(() => GetGlobalProperties(SdkPath.Value));
        public static readonly Lazy<string> SdkPath = new Lazy<string>(() => GetSdkPath());

        public static Project GetProject(string projectFile)
        {
            var globalProperties = GlobalProperties.Value;

            var evaluationContext = EvaluationContext.Create(EvaluationContext.SharingPolicy.Shared);

            // We could probably determine type from the file but straight forward to get from property.
            using (ProjectCollection projectCollection = new ProjectCollection(globalProperties))
            {
                var loadSettings = ProjectLoadSettings.IgnoreEmptyImports |
                    ProjectLoadSettings.IgnoreInvalidImports |
                    ProjectLoadSettings.IgnoreMissingImports |
                    ProjectLoadSettings.RecordDuplicateButNotCircularImports;

                var project = Project.FromFile(projectFile,
                     new ProjectOptions
                     {
                         ProjectCollection = projectCollection,
                         LoadSettings = loadSettings,
                         EvaluationContext = evaluationContext,
                         GlobalProperties = globalProperties,
                     });

                return project;
            }

        }
    }
}