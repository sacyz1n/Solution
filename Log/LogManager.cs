using Microsoft.Extensions.Logging;

namespace Log
{
    public static class LogManager
    {
        public static ILogger Logger { get; private set; } = null;

        public static ILoggerFactory LoggerFactory { get; private set; } = null;

        public static void SetLoggerFactory(ILoggerFactory loggerFactory, string categoryName)
        {
            if (loggerFactory == null)
                throw new ArgumentNullException(nameof(loggerFactory));

            if (string.IsNullOrWhiteSpace(categoryName))
                throw new ArgumentException("Category name cannot be null or empty.", nameof(categoryName));

            LogManager.Logger = loggerFactory.CreateLogger(categoryName);
            LogManager.LoggerFactory = loggerFactory;
        }
    }
}
