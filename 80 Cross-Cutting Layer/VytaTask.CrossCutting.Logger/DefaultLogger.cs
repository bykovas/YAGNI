using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VytaTask.Business.Contracts.Managers;

namespace VytaTask.CrossCutting.Logger
{
    public class DefaultLogger : ILogger
    {
        //Filter
        public void Debug(string message, Exception exception = null)
        {
            FilteredLog(LogLevel.Debug, message, exception);
        }

        public void Information(string message, Exception exception = null)
        {
            FilteredLog(LogLevel.Information, message, exception);
        }

        public void Warning(string message, Exception exception = null)
        {
            FilteredLog(LogLevel.Warning, message, exception);
        }

        public void Error(string message, Exception exception = null)
        {
            FilteredLog(LogLevel.Error, message, exception);
        }

        public void Fatal(string message, Exception exception = null)
        {
            FilteredLog(LogLevel.Fatal, message, exception);
        }

        private void FilteredLog(LogLevel level, string message, Exception exception = null)
        {
            //don't log thread abort exception
            if (exception is System.Threading.ThreadAbortException)
                return;

            if (!Enabled(level)) return;

            var fullMessage = exception?.ToString() ?? string.Empty;
            InsertLog((int) level, message, fullMessage);
        }

        public void InsertLog(int level, string message, string fullMessage)
        {
            throw new NotImplementedException("Fake log insert");
        }

        public bool IsEnabled(int level)
        {
            return Enabled((LogLevel)level);
        }

        private bool Enabled(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    return false;
                default:
                    return true;
            }
        }

        public void LogException(Exception exception)
        {
            InsertLog((int)LogLevel.Error, exception.Message, exception.StackTrace);
        }
    }
}
