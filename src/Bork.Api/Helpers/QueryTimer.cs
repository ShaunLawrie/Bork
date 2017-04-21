using NLog;
using System.Diagnostics;

namespace Bork.Api.Helpers
{
    public class QueryTimer
    {
        private readonly string _queryTag;
        private readonly Stopwatch _sw;
        private readonly ILogger _logger;

        public QueryTimer(string queryTag)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _queryTag = queryTag;
            _sw = new Stopwatch();
            _sw.Start();
        }

        public void LogQueryTime()
        {
            _sw.Stop();
            _logger.Info($"Query [{_queryTag}] took {_sw.ElapsedMilliseconds}ms");
        }
    }
}
