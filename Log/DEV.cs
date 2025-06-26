using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text;

namespace Log
{
    public static class DEV
    {
        public static string GetStackTrace(int skipStackDepth)
        {
            // 성능 많이 잡아먹음!!
            var stackTrace = new StackTrace(true);
            var stringBuilder = new StringBuilder();
            for (int i = skipStackDepth; i < stackTrace.FrameCount; i++)
            {
                var sf = stackTrace.GetFrame(i);
                stringBuilder.AppendLine();
                stringBuilder.Append($"at {sf?.GetMethod()} in {sf?.GetFileName()}:line {sf?.GetFileLineNumber()}");
            }
            return stringBuilder.ToString();
        }

        [Conditional("DEBUG")]
        [Conditional("RELEASE")]
        public static void CHECK(bool condition, string message = "", int stackTraceSkipDepth = 2)
        {
            if (condition)
                return;

            var stackTrace = GetStackTrace(stackTraceSkipDepth);
            LogManager.Logger.LogCritical(new EventId(LoggerExtensions.NotifyLoggerResultSync), $"{message}, CallStack:{stackTrace}");
            Trace.Assert(condition, $"{message} [{stackTrace}]");
            Environment.Exit(1);
        }
    }
}
