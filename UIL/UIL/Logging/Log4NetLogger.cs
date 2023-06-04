using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIL.Logging
{
    internal class Log4NetLogger : LoggerWrapper
    {
        private readonly string _config;
        private log4net.ILog _logger;
        public Log4NetLogger(log4net.ILog logger)
        {
            this._logger = logger;
        }
        public static LoggerWrapper CreateLogger(string _config)
        {
            if (!File.Exists(_config))
            {
                throw new ArgumentException("Does not exist", nameof(_config));
            }
            log4net.Config.XmlConfigurator.Configure(new FileInfo(_config));

            var logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            return new Log4NetLogger(logger);
        }
        public void Debug(string message)
        {
            this._logger.Debug(message);
        }
        public void Error(string message)
        {
            _logger.Error(message);
        }

        public void Fatal(string message)
        {
            _logger.Fatal(message);
        }

        public void Warning(string message)
        {
            _logger.Warn(message);
        }
        public void Info(string message)
        {
            _logger.Info(message);
        }
    }
}
