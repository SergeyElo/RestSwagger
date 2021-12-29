using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Logger
{
    public class Logger<T> : ILogger<T>
    {
        private readonly Microsoft.Extensions.Logging.ILogger _logger;

        public Logger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<T>();
        }

        public void Verbose(string messageTemplate, params object[] propertyValues)
        {
            _logger.Log(LogLevel.Trace, messageTemplate, propertyValues);
        }

        public void Debug(string messageTemplate, params object[] propertyValues)
        {
            _logger.Log(LogLevel.Debug, messageTemplate, propertyValues);
        }

        public void Fatal(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            _logger.Log(LogLevel.Critical, exception, messageTemplate, propertyValues);
        }

        public void Error(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            _logger.Log(LogLevel.Error, exception, messageTemplate, propertyValues);
        }

        public void Info(string messageTemplate, params object[] propertyValues)
        {
            _logger.Log(LogLevel.Information, messageTemplate, propertyValues);
        }

        public void Warn(string messageTemplate, params object[] propertyValues)
        {
            _logger.Log(LogLevel.Warning, messageTemplate, propertyValues);
        }

        public IDisposable WithScope<TState>(TState state)
        {
            return _logger.BeginScope(state);
        }

        public IDisposable WithScope(string msgFormat, params object[] propertyValues)
        {
            return _logger.BeginScope(msgFormat, propertyValues);
        }
    }
}
