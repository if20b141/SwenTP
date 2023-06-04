using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIL.Logging;

namespace UIL.Logging
{
    public static class ILoggerWrapperFactory
    {
        public static LoggerWrapper GetLogger()
        {
            return Log4NetLogger.CreateLogger(AppDomain.CurrentDomain.BaseDirectory + "/log4net.config");
        }
    }
}
