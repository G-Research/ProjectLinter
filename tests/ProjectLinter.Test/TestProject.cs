using System;
using System.IO;
using Microsoft.Build.Evaluation;

namespace ProjectLinter.Test
{
    public class DisposableFile : IDisposable
    {
        private string _tempFile;
        public DisposableFile(string fileName, string contents = null)
        {
            _tempFile = Path.Combine(Path.GetTempPath(), fileName);

            if (contents != null)
            {
                File.WriteAllText(_tempFile, contents);
            }
            else
            {
                File.CreateText(_tempFile);
            }
        }
        public string FullPath => _tempFile;
        public void Dispose()
        {
            File.Delete(_tempFile);
        }
    }

    public class TestProject : DisposableFile
    {
        public TestProject(string fileName, string projectContents)
            : base(fileName, projectContents)
        {
            Project = ProjectHelper.GetProject(FullPath);
        }

        public Project Project { get; }
    }
}