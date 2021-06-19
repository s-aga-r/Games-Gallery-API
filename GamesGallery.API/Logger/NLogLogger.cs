using NLog;

namespace GamesGallery.API.Logger
{
    public class NLogLogger : INLogLogger
    {
        private static ILogger _logger = LogManager.GetCurrentClassLogger();

        public NLogLogger()
        {

        }

        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        public void Error(string message)
        {
            _logger.Error(message);
        }

        public void Information(string message)
        {
            _logger.Info(message);
        }

        public void Warning(string message)
        {
            _logger.Warn(message);
        }
    }
}
