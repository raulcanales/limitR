using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace limitR.Middlewares
{
    public class RequestRateLimiter : IRequestRateLimiter
    {
        private readonly int _maxCalls;
        private readonly TimeSpan _timeWindow;
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<long, bool>> _stackCalls;
        private object _lock = new object();

        public RequestRateLimiter(int maxCalls)
            : this(maxCalls, TimeSpan.FromSeconds(1))
        {
        }

        public RequestRateLimiter(int maxCalls, TimeSpan timeWindow)
        {
            _maxCalls = maxCalls;
            _timeWindow = timeWindow;
            _stackCalls = new ConcurrentDictionary<string, ConcurrentDictionary<long, bool>>();
        }

        public bool CanPerformRequest(string apiKey) => CanPerformRequest(apiKey, _maxCalls);
        public bool CanPerformRequest(string apiKey, int maxCalls)
        {
            CleanUp();
            var isAllowed = false;
            lock (_lock)
            {
                var now = Stopwatch.GetTimestamp();
                var fromDate = now - _timeWindow.Ticks;

                if (!_stackCalls.ContainsKey(apiKey))
                {                    
                    _stackCalls.TryAdd(apiKey, new ConcurrentDictionary<long, bool>());
                    isAllowed = _stackCalls[apiKey].TryAdd(now, true);
                }
                else if (_stackCalls[apiKey].Count(kv => kv.Key >= fromDate) < maxCalls)
                    isAllowed = _stackCalls[apiKey].TryAdd(now, true);
            }

            return isAllowed;
        }

        /// <summary>
        /// Cleans up old calls
        /// </summary>
        private Task CleanUp()
        {
            var fromDate = Stopwatch.GetTimestamp() - _timeWindow.Ticks;
            foreach (var apiKey in _stackCalls.Keys)
            {
                foreach(var key in _stackCalls[apiKey].Keys.Where(k => k < fromDate))
                {
                    _stackCalls[apiKey].TryRemove(key, out var _);
                }

                if (_stackCalls[apiKey].Keys.Count == 0)
                    _stackCalls.TryRemove(apiKey, out var _);
            }
            return Task.CompletedTask;
        }
    }
}
