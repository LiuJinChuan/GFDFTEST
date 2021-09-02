using GFDF.Infrastruct.Core;
using System;
[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace GFDF.Infrastruct.Impl
{
    public class LogHelper : ILogger
    {
        private static readonly log4net.ILog loginfo = log4net.LogManager.GetLogger("loginfo");
        private static readonly log4net.ILog logerror = log4net.LogManager.GetLogger("logerror");
        private static readonly log4net.ILog logdebug = log4net.LogManager.GetLogger("logdebug");
        private static readonly log4net.ILog logfatal = log4net.LogManager.GetLogger("logfatal");
        private static readonly log4net.ILog logwarn = log4net.LogManager.GetLogger("logwarn");

        public void Debug(string message)
        {
            if (logdebug.IsDebugEnabled)logdebug.Debug(message);
        }
        public void Debug(string format, params object[] args)
        {
            if (logdebug.IsDebugEnabled) logdebug.DebugFormat(format, args);
        }
        public void Debug(object message, Exception exception)
        {
            if (logdebug.IsDebugEnabled) logdebug.Debug(message, exception);
        }

        public void Fatal(string message)
        {
            if (logfatal.IsFatalEnabled) logfatal.Fatal(message);
        }
        public void Fatal(string format, params object[] args)
        {
            if (logfatal.IsFatalEnabled) logfatal.FatalFormat(format, args);
        }
        public void Fatal(object message, Exception exception)
        {
            if (logfatal.IsFatalEnabled) logfatal.Fatal(message, exception);
        }

        public void Error(Exception ex)
        {
            if (logerror.IsErrorEnabled) logerror.Error(ex);
        }
        public void Error(string format, params object[] args)
        {
            if (logerror.IsErrorEnabled) logerror.ErrorFormat(format, args);
        }
        public void Error(object message, Exception exception)
        {
            if (logerror.IsErrorEnabled) logerror.Error(message, exception);
        }

        public void Info(string message)
        {
            if (loginfo.IsInfoEnabled) loginfo.Info(message);
        }
        public void Info(string format, params object[] args)
        {
            if (loginfo.IsInfoEnabled) loginfo.InfoFormat(format, args);
        }
        public void Info(object message, Exception exception)
        {
            if (loginfo.IsInfoEnabled) loginfo.Info(message, exception);
        }

        public void Warn(string message)
        {
            if (logwarn.IsWarnEnabled) logwarn.Warn(message);
        }
        public void Warn(string format, params object[] args)
        {
            if (logwarn.IsWarnEnabled) logwarn.WarnFormat(format, args);
        }
        public void Warn(object message, Exception exception)
        {
            if (logwarn.IsWarnEnabled) logwarn.Warn(message, exception);
        }
    }
}
