using System.Buffers;
using System.Text;
using ZLogger;

namespace Log
{
    public class ZLoggerFormatter : IZLoggerFormatter
    {
        public bool WithLineBreak => true;

        public void FormatLogEntry(IBufferWriter<byte> writer, IZLoggerEntry entry)
        {
            var logInfo = entry.LogInfo;
            var logLevel = logInfo.LogLevel;
            var timeStamp = logInfo.Timestamp;
            var category = logInfo.Category;
            var message = entry.ToString();

            writer.Write(Encoding.UTF8.GetBytes($"[{logLevel}] {timeStamp:yyyy-MM-dd HH:mm:ss.fff} {category} - {message}"));
        }
    }
}
