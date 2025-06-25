using Log.NetNotifiers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLogger;
using ZLogger.Providers;

namespace Log
{
    public class ZLoggerNetProvider : ILoggerProvider
    {
        private readonly ILoggerProvider _innerLogProvider;

        private readonly INetNotifiication _netNotifiication;

        public ZLoggerNetProvider(ILoggerProvider innerLogProvider, INetNotifiication netNotification)
        {
            this._innerLogProvider = innerLogProvider ?? throw new ArgumentNullException(nameof(innerLogProvider));
            this._netNotifiication = netNotification ?? throw new ArgumentNullException(nameof(netNotification));
        }

        public ILogger CreateLogger(string categoryName)
        {
            var innerLogger = this._innerLogProvider.CreateLogger(categoryName);
            return new NetNotifyLogger(categoryName, innerLogger, _netNotifiication);
        }

        public void Dispose()
        {
        }
    }
}
