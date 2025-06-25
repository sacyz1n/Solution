using Microsoft.Extensions.Logging;

namespace Log
{
    public class NotifyLogger : ILogger
    {
        private readonly string _categoryName;

        private readonly INotifiication _netNotification;

        public NotifyLogger(string categoryName, INotifiication netNotification)
        {
            _categoryName = categoryName ?? throw new ArgumentNullException(nameof(categoryName));
            _netNotification = netNotification ?? throw new ArgumentNullException(nameof(netNotification));
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
            => null;

        public bool IsEnabled(LogLevel logLevel)
            => logLevel >= LogLevel.Critical;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            _netNotification.SendNotification(formatter(state, exception));
        }
    }
}
