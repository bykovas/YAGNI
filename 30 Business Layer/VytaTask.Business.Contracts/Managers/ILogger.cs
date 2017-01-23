using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VytaTask.Business.Contracts.Managers
{
    public interface ILogger
    {
        void Debug(string message, Exception exception = null);
        void Information(string message, Exception exception = null);
        void Warning(string message, Exception exception = null);
        void Error(string message, Exception exception = null);
        void Fatal(string message, Exception exception = null);
        void InsertLog(int level, string message, string fullMessage);
        bool IsEnabled(int level);
        void LogException(Exception exception);
    }
}
