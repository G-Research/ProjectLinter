using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace ProjectLinter
{
    public interface ILogger
    {
        void LogInfo(string message, params object[] messageArgs);
        void LogWarning(string message, params object[] messageArgs);
        void LogError(string message, params object[] messageArgs);
        void LogDetail(string message, params object[] messageArgs);
        void LogDiagnostic(string message, params object[] messageArgs);
    }

    public class Logger : ILogger
    {
        private readonly TaskLoggingHelper _taskLoggingHelper;
        private readonly bool _warningsAsErrors;

        public Logger(TaskLoggingHelper taskLoggingHelper, bool warningsAsErrors)
        {
            _taskLoggingHelper = taskLoggingHelper;
            _warningsAsErrors = warningsAsErrors;
        }

        public void LogInfo(string message, params object[] messageArgs)
        {
            _taskLoggingHelper.LogMessage(MessageImportance.High, message, messageArgs);
        }

        public void LogWarning(string message, params object[] messageArgs)
        {
            if (_warningsAsErrors)
            {
                LogError(message, messageArgs);
            }
            else
            {
                _taskLoggingHelper.LogWarning(message, messageArgs);
            }
        }

        public void LogError(string message, params object[] messageArgs)
        {
            _taskLoggingHelper.LogError(message, messageArgs);
        }

        public void LogDetail(string message, params object[] messageArgs)
        {
            _taskLoggingHelper.LogMessage(MessageImportance.Normal, message, messageArgs);
        }

        public void LogDiagnostic(string message, params object[] messageArgs)
        {
            _taskLoggingHelper.LogMessage(MessageImportance.Low, message, messageArgs);
        }
    }
}