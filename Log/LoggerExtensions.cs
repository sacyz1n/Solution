using Microsoft.Extensions.Logging;

namespace Log
{
    public static class LoggerExtensions
    {
        internal const int NotifyLoggerResultSync = 1000;
        public static ILoggingBuilder AddNotifyLogger(this ILoggingBuilder builder, INotifiication netNotification)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            if (netNotification == null)
                throw new ArgumentNullException(nameof(netNotification));

            builder.AddProvider(new NotifyLoggerProvider(netNotification));
            return builder;
        }
    }
}
