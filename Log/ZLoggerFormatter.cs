using Microsoft.Extensions.Logging;
using System.Buffers;
using System.Text;
using ZLogger;

namespace Log
{
    public class ZLoggerFormatter : IZLoggerFormatter
    {
        public bool WithLineBreak => true;

        public string ToLogLevelStr(LogLevel logLevel)
        {
            return logLevel switch
            {
                LogLevel.Trace => "TRACE",
                LogLevel.Debug => "DEBUG",
                LogLevel.Information => "INFO",
                LogLevel.Warning => "WARN",
                LogLevel.Error => "ERROR",
                LogLevel.Critical => "CRIT",
                _ => "UNKNOWN"
            };
        }

        public void FormatLogEntry(IBufferWriter<byte> writer, IZLoggerEntry entry)
        {
            var logInfo = entry.LogInfo;
            var logLevel = logInfo.LogLevel;
            var timeStamp = logInfo.Timestamp;
            var category = logInfo.Category;
            var message = entry.ToString();

            writer.Write(Encoding.UTF8.GetBytes($"{timeStamp:yyyy-MM-dd HH:mm:ss.fff}|{ToLogLevelStr(logLevel)}|{message}"));
        }
    }
}
