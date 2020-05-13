using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace limitR
{
    public class SimpleRateLimiter : IRateLimiter
    {
        private readonly int _maxCalls;
        private readonly TimeSpan _timeWindow;
        private static readonly ConcurrentDictionary<long, bool> _stackCalls = new ConcurrentDictionary<long, bool>();
        private object _lock = new object();

        public SimpleRateLimiter(int maxCalls)
            : this(maxCalls, TimeSpan.FromSeconds(1))
        {
        }

        public SimpleRateLimiter(int maxCalls, TimeSpan timeWindow)
        {
            _maxCalls = maxCalls;
            _timeWindow = timeWindow;
        }

        public void Invoke(Action call) => Invoke(call, _maxCalls);
        public void Invoke(Action call, int maxCalls)
        {
            CleanUp();
            if (IsAllowed(maxCalls))
                call.Invoke();
        }

        public T Invoke<T>(Func<T> call) => Invoke(call, _maxCalls);
        public T Invoke<T>(Func<T> call, int maxCalls)
        {
            CleanUp();
            if (IsAllowed(maxCalls))
                return call.Invoke();

            return default;
        }

        public async Task InvokeAsync(Func<Task> call) => await InvokeAsync(call, _maxCalls);
        public async Task InvokeAsync(Func<Task> call, int maxCalls)
        {
            _ = CleanUp();
            if (IsAllowed(maxCalls))
                await call.Invoke();
        }

        public async Task<T> InvokeAsync<T>(Func<Task<T>> call) => await InvokeAsync(call, _maxCalls);
        public async Task<T> InvokeAsync<T>(Func<Task<T>> call, int maxCalls)
        {
            _ = CleanUp();
            if (IsAllowed(maxCalls))
                return await call.Invoke();

            return default;
        }

        /// <summary>
        /// Checks if the request can be invoked, based on the amount of calls already registered
        /// </summary>        
        private bool IsAllowed(int maxCalls)
        {
            var isAllowed = false;
            lock (_lock)
            {
                var now = TimeProvider.Current.Now;
                var fromDate = now.Add(-_timeWindow).Ticks;
                if (_stackCalls.Count(kv => kv.Key >= fromDate) < maxCalls)
                    isAllowed = _stackCalls.TryAdd(now.Ticks, true);
            }

            return isAllowed;
        }

        /// <summary>
        /// Cleans up old calls
        /// </summary>
        private Task CleanUp()
        {
            var fromDate = TimeProvider.Current.Now.Add(-_timeWindow).Ticks;
            foreach (var key in _stackCalls.Keys.Where(k => k < fromDate))
                _stackCalls.TryRemove(key, out var _);
            return Task.CompletedTask;
        }
    }
}
