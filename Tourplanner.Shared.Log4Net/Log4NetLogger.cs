using Tourplanner.Shared;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tourplanner.Shared.Log4Net
{
    public class Log4NetLogger : ILogger
    {
        private readonly ILog logger;

        public Log4NetLogger(ILog logger)
        {
            this.logger = logger;
        }

        public void Debug(string message)
        {
            logger.Debug(message);
        }

        public void Error(string message)
        {
            logger.Error(message);
        }

        public void Fatal(string message)
        {
            logger.Fatal(message);
        }

        public void Warn(string message)
        {
            logger.Warn(message);
        }
    }
}
