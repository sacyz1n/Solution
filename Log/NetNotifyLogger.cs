using Log.NetNotifiers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLogger;

namespace Log
{
    public class NetNotifyLogger : ILogger
    {
        private readonly string _categoryName;

        private readonly ILogger _innerLogger;

        private readonly INetNotifiication _netNotification;


        public NetNotifyLogger(string categoryName, ILogger innerLogger, INetNotifiication netNotification)
        {
            _categoryName = categoryName ?? throw new ArgumentNullException(nameof(categoryName));
            _innerLogger = innerLogger ?? throw new ArgumentNullException(nameof(innerLogger));
            _netNotification = netNotification ?? throw new ArgumentNullException(nameof(netNotification));
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
            => null;

        public bool IsEnabled(LogLevel logLevel)
            => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            _innerLogger.Log(logLevel, eventId, state, exception, formatter);

            _netNotification.SendNotification(formatter(state, exception));
        }
    }
}
