using System;
using System.Threading.Tasks;

namespace limitR
{
    /// <summary>
    /// Rate limiter interface, that allows to invoke different functions and actions, while limiting the invocations to a desired amount of times per given time interval.
    /// </summary>
    public interface IRateLimiter
    {
        /// <summary>
        /// Attempts to execute an <see cref="Action"/>. Does nothing if the limiter reached the maximum allowed calls for a given time interval.
        /// </summary>
        void Invoke(Action call);

        /// <summary>
        /// Attempts to execute an <see cref="Action"/>. Does nothing if the limiter reached the maximum allowed calls for a given time interval.
        /// </summary>
        /// <param name="call"></param>
        /// <param name="maxCalls">Overrides the maximum calls passed to the constructor</param>
        void Invoke(Action call, int maxCalls);

        /// <summary>
        /// Attempts to execute a <see cref="Func"/> that returns a result. Returns the default value of the expected type, if the limiter reached the maximum allowed calls for a given time interval.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="call"></param>        
        T Invoke<T>(Func<T> call);

        /// <summary>
        /// Attempts to execute a <see cref="Func"/> that returns a result. Returns the default value of the expected type, if the limiter reached the maximum allowed calls for a given time interval.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="call"></param>
        /// <param name="maxCalls">Overrides the maximum calls passed to the constructor</param>
        T Invoke<T>(Func<T> call, int maxCalls);

        /// <summary>
        /// Attempts to execute an awaitable async <see cref="Func"/> that returns a task. Returns the default value of the expected type, if the limiter reached the maximum allowed calls for a given time interval.
        /// </summary>
        Task InvokeAsync(Func<Task> call);

        /// <summary>
        /// Attempts to execute an awaitable async <see cref="Func"/> that returns a task. Returns the default value of the expected type, if the limiter reached the maximum allowed calls for a given time interval.
        /// </summary>
        /// <param name="call"></param>
        /// <param name="maxCalls">Overrides the maximum calls passed to the constructor</param>
        Task InvokeAsync(Func<Task> call, int maxCalls);

        /// <summary>
        /// Attempts to execute an awaitable async <see cref="Func"/> that returns a result. Returns the default value of the expected type, if the limiter reached the maximum allowed calls for a given time interval.
        /// </summary>
        Task<T> InvokeAsync<T>(Func<Task<T>> call);

        /// <summary>
        /// Attempts to execute an awaitable async <see cref="Func"/> that returns a result. Returns the default value of the expected type, if the limiter reached the maximum allowed calls for a given time interval.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="call"></param>
        /// <param name="maxCalls">Overrides the maximum calls passed to the constructor</param>
        Task<T> InvokeAsync<T>(Func<Task<T>> call, int maxCalls);
    }
}
