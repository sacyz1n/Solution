using Microsoft.Extensions.Logging;

namespace Log
{
    public class NotifyLoggerProvider : ILoggerProvider
    {
        private readonly INotifiication _netNotifiication;

        public NotifyLoggerProvider(INotifiication netNotification)
        {
            this._netNotifiication = netNotification ?? throw new ArgumentNullException(nameof(netNotification));
        }

        public ILogger CreateLogger(string categoryName)
            => new NotifyLogger(categoryName, _netNotifiication);

        public void Dispose()
        {
        }
    }
}
