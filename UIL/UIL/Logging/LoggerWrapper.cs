using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIL.Logging
{
    public interface LoggerWrapper
    {
        void Debug(string message);
        void Warning(string message);
        void Error(string message);
        void Fatal(string message);
        void Info(string message);
    }
}
