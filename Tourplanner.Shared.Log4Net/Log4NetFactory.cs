namespace Tourplanner.Shared.Log4Net
{
    public class Log4NetFactory : ILoggerFactory
    {
        private readonly string configPath;

        public Log4NetFactory(string configPath)
        {
            this.configPath = (Path.Combine(@"..\..\..\", configPath));
        }

        public ILogger CreateLogger<TContext>()
        {
            if (!File.Exists(configPath))
            {
                throw new ArgumentException("Does not exist.", nameof(configPath));
            }

            log4net.Config.XmlConfigurator.Configure(new FileInfo(configPath));
            var logger = log4net.LogManager.GetLogger(typeof(TContext));
            return new Log4NetLogger(logger);
        }
    }
}