namespace limitR.Middlewares
{
    public interface IRequestRateLimiter
    {
        bool CanPerformRequest(string apiKey);
        bool CanPerformRequest(string apiKey, int maxCalls);
    }
}
