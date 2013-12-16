using System.Diagnostics;

namespace SharpDox.Core
{
    public class Log4NetTraceListener : TraceListener
    {
        private readonly log4net.ILog _log;

        public Log4NetTraceListener()
        {
            _log = log4net.LogManager.GetLogger("Log4NetTracer");
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            switch (eventType)
            {
                case TraceEventType.Error:
                    _log.Error(message);
                    break;

                case TraceEventType.Warning:
                    _log.Warn(message);
                    break;

                case TraceEventType.Information:
                    _log.Info(message);
                    break;

                default:
                    _log.Debug(message);
                    break;
            }
        }

        public override void Write(string message)
        {
            _log.Debug(message);
        }

        public override void WriteLine(string message)
        {
            _log.Debug(message);
        }
    }
}
